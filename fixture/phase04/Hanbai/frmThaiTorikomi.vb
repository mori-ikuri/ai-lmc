Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ受注取込
' 作成者     : 山田
'
' 履歴
' 2011/11/08 山田 新規作成
'                 現地(サンプル精機タイランド)の生産管理が出すCSVを読む。
'                 日本のCSVと形は似ているが、列が11ある。
'                 日付が dd/MM/yyyy、単価と通貨が入っている点が違う。
'                 日本の受注取込(frmJuchuTorikomi)は毎朝動いているので触らない
' 2012/02/13 山田 タイの納品を入れられるようにしたので、
'                 納品済みの受注は内容が変わっていても上書きしないようにした
'============================================================
Public Class frmThaiTorikomi

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmThaiTorikomi_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "タイ受注取込  [" & gUserName & "]"

        txtFile.Text = ""
        lblKekka.Text = ""

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '参照
    '------------------------------------------------------------
    Private Sub btnSansho_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSansho.Click

        ofdFile.InitialDirectory = TH_CSV_PATH
        ofdFile.FileName = ""
        ofdFile.Filter = "タイ受注CSV (THJUCHU_*.CSV)|THJUCHU_*.CSV|すべてのファイル (*.*)|*.*"

        If ofdFile.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        txtFile.Text = ofdFile.FileName

    End Sub

    '------------------------------------------------------------
    '取込
    '------------------------------------------------------------
    Private Sub btnTorikomi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTorikomi.Click

        Dim cn As SqlConnection = Nothing
        Dim sr As System.IO.StreamReader = Nothing
        Dim cm As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim dt As DataTable
        Dim strSql As String
        Dim strLine As String
        Dim strFld() As String
        Dim strFileNm As String
        Dim strDenpyoNo As String
        Dim strTokuisakiCd As String
        Dim strJuchubi As String
        Dim strNoki As String
        Dim strHinban As String
        Dim strHinmei As String
        Dim strTsuka As String
        Dim strKbn As String
        Dim strJotai As String
        Dim lngSuryo As Long
        Dim dblTanka As Double
        Dim intGyo As Integer
        Dim intToroku As Integer
        Dim intTeisei As Integer
        Dim intJufuku As Integer
        Dim intMitoroku As Integer
        Dim intYokakunin As Integer
        Dim intError As Integer

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Trim(txtFile.Text) = "" Then
                MsgBox("ファイルを指定してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            If System.IO.File.Exists(Trim(txtFile.Text)) = False Then
                MsgBox("ファイルがありません。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            strFileNm = System.IO.Path.GetFileName(Trim(txtFile.Text))

            cn = GetConnection()

            '同じファイルをもう一度選んでいないかみる ※取込ログは日本のと同じ表を使う
            strSql = "SELECT COUNT(*) FROM T_TORIKOMI_LOG WHERE FILE_NM = '" & EscQuote(strFileNm) & "'"
            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) > 0 Then
                If MsgBox(strFileNm & " は既に取り込んでいます。" & vbCrLf & "もう一度取り込みますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If
            End If

            '結果の一覧
            dt = New DataTable
            dt.Columns.Add("GYO")
            dt.Columns.Add("DENPYO_NO")
            dt.Columns.Add("NAIYO")

            sr = New System.IO.StreamReader(Trim(txtFile.Text), System.Text.Encoding.GetEncoding(932))

            Do Until sr.EndOfStream

                strLine = sr.ReadLine()
                intGyo = intGyo + 1

                If Trim(strLine) = "" Then
                    GoTo TsugiNoGyo
                End If

                strFld = Split(strLine, ",")

                'ヘッダは無い。列は
                '得意先/受注日/納期/品番/品名/数量/単価/通貨/?/?/伝票番号 の11
                If UBound(strFld) < 10 Then
                    intError = intError + 1
                    dt.Rows.Add(CStr(intGyo), "", "列が足りません")
                    GoTo TsugiNoGyo
                End If

                strTokuisakiCd = ToriQuote(strFld(0))
                strJuchubi = ToHizuke(ToriQuote(strFld(1)))
                strNoki = ToHizuke(ToriQuote(strFld(2)))
                strHinban = ToriQuote(strFld(3))
                strHinmei = ToriQuote(strFld(4))
                lngSuryo = Val(ToriQuote(strFld(5)))
                dblTanka = Val(ToriQuote(strFld(6)))
                strTsuka = ToriQuote(strFld(7))
                strKbn = ToriQuote(strFld(8))
                strJotai = ToriQuote(strFld(9))
                strDenpyoNo = ToriQuote(strFld(10))

                If strDenpyoNo = "" Then
                    intError = intError + 1
                    dt.Rows.Add(CStr(intGyo), "", "伝票番号がありません")
                    GoTo TsugiNoGyo
                End If

                If strJuchubi = "" Then
                    intError = intError + 1
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "受注日が読めません")
                    GoTo TsugiNoGyo
                End If

                If strTokuisakiCd = "" Then
                    intError = intError + 1
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "得意先コードがありません")
                    GoTo TsugiNoGyo
                End If

                'まだ名前を付けていない得意先でも取り込む。
                '落とすと向こうのデータが手元に残らないので、一覧に出して知らせるだけにする
                strSql = "SELECT COUNT(*) FROM M_TH_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "'"
                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) = 0 Then
                    intMitoroku = intMitoroku + 1
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "得意先 " & strTokuisakiCd & " の名前が登録されていません(タイ得意先登録で入れてください)")
                End If

                '通貨がバーツ以外のものが来たら、入れはするが一覧に出しておく
                If strTsuka <> "" And strTsuka <> "THB" Then
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "通貨が " & strTsuka & " になっています")
                End If

                '同じ伝票番号が既にあるかみる
                strSql = ""
                strSql = strSql & "SELECT TOKUISAKI_CD, CONVERT(varchar(10), JUCHUBI, 111) AS JUCHUBI, "
                strSql = strSql & "ISNULL(CONVERT(varchar(10), NOKI, 111), '') AS NOKI, "
                strSql = strSql & "ISNULL(HINBAN, '') AS HINBAN, ISNULL(HINMEI, '') AS HINMEI, SURYO, TANKA, "
                strSql = strSql & "ISNULL(TSUKA, '') AS TSUKA, ISNULL(KBN, '') AS KBN, ISNULL(JOTAI_KBN, '') AS JOTAI_KBN "
                strSql = strSql & "FROM T_TH_JUCHU WHERE DENPYO_NO = '" & EscQuote(strDenpyoNo) & "'"

                ds = New DataSet
                da = New SqlDataAdapter(strSql, cn)
                da.Fill(ds, "JUCHU")

                If ds.Tables("JUCHU").Rows.Count = 0 Then

                    '新しい伝票番号なので入れる
                    strSql = ""
                    strSql = strSql & "INSERT INTO T_TH_JUCHU "
                    strSql = strSql & "(DENPYO_NO, TOKUISAKI_CD, JUCHUBI, NOKI, HINBAN, HINMEI, SURYO, TANKA, TSUKA, KBN, JOTAI_KBN, TORIKOMI_FILE, TORIKOMI_DT, KOSHIN_USER, KOSHIN_DT) "
                    strSql = strSql & "VALUES ("
                    strSql = strSql & "'" & EscQuote(strDenpyoNo) & "',"
                    strSql = strSql & "'" & EscQuote(strTokuisakiCd) & "',"
                    strSql = strSql & "'" & strJuchubi & "',"
                    strSql = strSql & ToSqlDate(strNoki) & ","
                    strSql = strSql & "'" & EscQuote(strHinban) & "',"
                    strSql = strSql & "'" & EscQuote(strHinmei) & "',"
                    strSql = strSql & CStr(lngSuryo) & ","
                    strSql = strSql & CStr(dblTanka) & ","
                    strSql = strSql & "'" & EscQuote(strTsuka) & "',"
                    strSql = strSql & "'" & EscQuote(strKbn) & "',"
                    strSql = strSql & "'" & EscQuote(strJotai) & "',"
                    strSql = strSql & "'" & EscQuote(strFileNm) & "',"
                    strSql = strSql & "GETDATE(),"
                    strSql = strSql & "'" & gUserName & "',"
                    strSql = strSql & "GETDATE())"

                    cm = New SqlCommand(strSql, cn)
                    cm.ExecuteNonQuery()

                    intToroku = intToroku + 1

                Else

                    '既にある。中身が同じなら何もしない
                    If ToStr(ds.Tables("JUCHU").Rows(0)("TOKUISAKI_CD")) = strTokuisakiCd _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("JUCHUBI")) = strJuchubi _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("NOKI")) = strNoki _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("HINBAN")) = strHinban _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("HINMEI")) = strHinmei _
                       And Val(ToStr(ds.Tables("JUCHU").Rows(0)("SURYO"))) = lngSuryo _
                       And Val(ToStr(ds.Tables("JUCHU").Rows(0)("TANKA"))) = dblTanka _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("TSUKA")) = strTsuka _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("KBN")) = strKbn _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("JOTAI_KBN")) = strJotai Then

                        intJufuku = intJufuku + 1
                        GoTo TsugiNoGyo

                    End If

                    '2012/02/13 山田 納品を入れた後で受注が変わると数字がずれるので、
                    '                 その場合は更新せずに一覧へ出す。日本の受注取込と同じにした
                    strSql = "SELECT COUNT(*) FROM T_TH_NOHIN WHERE DENPYO_NO = '" & EscQuote(strDenpyoNo) & "' AND TORIKESHI_KBN = '0'"
                    cm = New SqlCommand(strSql, cn)

                    If Val(CStr(cm.ExecuteScalar())) > 0 Then
                        intYokakunin = intYokakunin + 1
                        dt.Rows.Add(CStr(intGyo), strDenpyoNo, "納品済みのため取り込みませんでした(内容が変わっています)")
                        GoTo TsugiNoGyo
                    End If

                    '同じ伝票番号がまた出てきたら訂正として上書きする
                    strSql = ""
                    strSql = strSql & "UPDATE T_TH_JUCHU SET "
                    strSql = strSql & "TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "',"
                    strSql = strSql & "JUCHUBI = '" & strJuchubi & "',"
                    strSql = strSql & "NOKI = " & ToSqlDate(strNoki) & ","
                    strSql = strSql & "HINBAN = '" & EscQuote(strHinban) & "',"
                    strSql = strSql & "HINMEI = '" & EscQuote(strHinmei) & "',"
                    strSql = strSql & "SURYO = " & CStr(lngSuryo) & ","
                    strSql = strSql & "TANKA = " & CStr(dblTanka) & ","
                    strSql = strSql & "TSUKA = '" & EscQuote(strTsuka) & "',"
                    strSql = strSql & "KBN = '" & EscQuote(strKbn) & "',"
                    strSql = strSql & "JOTAI_KBN = '" & EscQuote(strJotai) & "',"
                    strSql = strSql & "TORIKOMI_FILE = '" & EscQuote(strFileNm) & "',"
                    strSql = strSql & "TORIKOMI_DT = GETDATE(),"
                    strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
                    strSql = strSql & "KOSHIN_DT = GETDATE() "
                    strSql = strSql & "WHERE DENPYO_NO = '" & EscQuote(strDenpyoNo) & "'"

                    cm = New SqlCommand(strSql, cn)
                    cm.ExecuteNonQuery()

                    intTeisei = intTeisei + 1
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "内容が変わっていたので訂正しました")

                End If

TsugiNoGyo:

            Loop

            sr.Close()
            sr = Nothing

            '取込ログ
            strSql = "SELECT COUNT(*) FROM T_TORIKOMI_LOG WHERE FILE_NM = '" & EscQuote(strFileNm) & "'"
            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) = 0 Then
                strSql = ""
                strSql = strSql & "INSERT INTO T_TORIKOMI_LOG (FILE_NM, TORIKOMI_DT, KENSU, TORIKOMI_USER) "
                strSql = strSql & "VALUES ('" & EscQuote(strFileNm) & "', GETDATE(), " & CStr(intToroku) & ", '" & gUserName & "')"
            Else
                strSql = ""
                strSql = strSql & "UPDATE T_TORIKOMI_LOG SET "
                strSql = strSql & "TORIKOMI_DT = GETDATE(), KENSU = " & CStr(intToroku) & ", TORIKOMI_USER = '" & gUserName & "' "
                strSql = strSql & "WHERE FILE_NM = '" & EscQuote(strFileNm) & "'"
            End If

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            '結果
            dgvKekka.DataSource = dt

            dgvKekka.Columns(0).HeaderText = "行"
            dgvKekka.Columns(1).HeaderText = "伝票番号"
            dgvKekka.Columns(2).HeaderText = "内容"

            dgvKekka.Columns(0).Width = 50
            dgvKekka.Columns(1).Width = 110
            dgvKekka.Columns(2).Width = 560

            lblKekka.Text = "読んだ行 " & CStr(intGyo) & " 件   " & _
                            "取込 " & CStr(intToroku) & " 件   " & _
                            "訂正 " & CStr(intTeisei) & " 件   " & _
                            "重複 " & CStr(intJufuku) & " 件   " & _
                            "要確認 " & CStr(intYokakunin) & " 件   " & _
                            "得意先未登録 " & CStr(intMitoroku) & " 件   " & _
                            "エラー " & CStr(intError) & " 件"

            If intError > 0 Or intYokakunin > 0 Then
                MsgBox("取込が終わりました。取り込めなかった行があります。一覧を見てください。", MsgBoxStyle.Exclamation)
            ElseIf intMitoroku > 0 Then
                MsgBox("取込が終わりました。名前が登録されていない得意先があります。一覧を見てください。", MsgBoxStyle.Exclamation)
            Else
                MsgBox("取込が終わりました。", MsgBoxStyle.Information)
            End If

        Catch ex As Exception

            MsgBox("取込でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not sr Is Nothing Then
                sr.Close()
            End If
            If Not cn Is Nothing Then
                cn.Close()
            End If
            mblnShori = False

        End Try

    End Sub

    '------------------------------------------------------------
    '閉じる
    '------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Me.Close()

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '前後の " を取る
    '------------------------------------------------------------
    Private Function ToriQuote(ByVal strVal As String) As String

        Dim strWk As String

        strWk = Trim(strVal)

        If Len(strWk) >= 2 Then
            'フォームにも Left / Right があってそのままでは通らない
            If Microsoft.VisualBasic.Left(strWk, 1) = """" And Microsoft.VisualBasic.Right(strWk, 1) = """" Then
                strWk = Mid(strWk, 2, Len(strWk) - 2)
            End If
        End If

        Return Trim(strWk)

    End Function

    '------------------------------------------------------------
    'dd/MM/yyyy を yyyy/MM/dd にする  読めなければ空
    '※向こうのファイルは日が先に来ている。20/10/2011 のような行があるので
    '  CDate に任せると月と取り違える。自分で切って組み立てる
    '------------------------------------------------------------
    Private Function ToHizuke(ByVal strVal As String) As String

        Dim strWk As String
        Dim strBu() As String
        Dim strHi As String

        strWk = Trim(strVal)

        If strWk = "" Then
            Return ""
        End If

        strBu = Split(strWk, "/")

        If UBound(strBu) <> 2 Then
            Return ""
        End If

        If IsNumeric(strBu(0)) = False Or IsNumeric(strBu(1)) = False Or IsNumeric(strBu(2)) = False Then
            Return ""
        End If

        If Len(Trim(strBu(2))) <> 4 Then
            Return ""
        End If

        strHi = Trim(strBu(2)) & "/" & Format(Val(strBu(1)), "00") & "/" & Format(Val(strBu(0)), "00")

        If IsDate(strHi) = False Then
            Return ""
        End If

        Return strHi

    End Function

#End Region

End Class
