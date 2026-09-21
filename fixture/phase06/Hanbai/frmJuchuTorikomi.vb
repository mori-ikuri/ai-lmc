Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 受注取込
' 作成者     : 山田
'
' 履歴
' 2008/06/09 山田 新規作成
' 2008/07/22 山田 同じ伝票番号がまた出てくるため、内容を見て更新するようにした
'                 納品を入れた後のものは更新せず一覧に出す
' 2008/08/05 山田 一度取り込んだファイルを選んだときに確認を出すようにした
' 2008/09/09 山田 得意先マスタに無いコードの行は取り込まないようにした
'============================================================
Public Class frmJuchuTorikomi

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmJuchuTorikomi_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "受注取込  [" & gUserName & "]"

        txtFile.Text = ""
        lblKekka.Text = ""

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '参照
    '------------------------------------------------------------
    Private Sub btnSansho_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSansho.Click

        ofdFile.InitialDirectory = CSV_PATH
        ofdFile.FileName = ""
        ofdFile.Filter = "受注CSV (JUCHU_*.CSV)|JUCHU_*.CSV|すべてのファイル (*.*)|*.*"

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
        Dim strKbn As String
        Dim strTorikeshi As String
        Dim lngSuryo As Long
        Dim intGyo As Integer
        Dim intToroku As Integer
        Dim intTeisei As Integer
        Dim intJufuku As Integer
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

            '2008/08/05 山田 同じファイルをもう一度選んでいないかみる
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

                'ヘッダは無い。列は 得意先/受注日/納期/品番/品名/数量/?/取消/伝票番号 の9つ
                If UBound(strFld) < 8 Then
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
                strKbn = ToriQuote(strFld(6))
                strTorikeshi = ToriQuote(strFld(7))
                strDenpyoNo = ToriQuote(strFld(8))

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

                '2008/09/09 山田 得意先が無いと単価も締日も引けないので取り込まない
                strSql = "SELECT COUNT(*) FROM M_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "'"
                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) = 0 Then
                    intError = intError + 1
                    dt.Rows.Add(CStr(intGyo), strDenpyoNo, "得意先 " & strTokuisakiCd & " が登録されていません")
                    GoTo TsugiNoGyo
                End If

                '同じ伝票番号が既にあるかみる
                strSql = ""
                strSql = strSql & "SELECT TOKUISAKI_CD, CONVERT(varchar(10), JUCHUBI, 111) AS JUCHUBI, "
                strSql = strSql & "ISNULL(CONVERT(varchar(10), NOKI, 111), '') AS NOKI, "
                strSql = strSql & "ISNULL(HINBAN, '') AS HINBAN, ISNULL(HINMEI, '') AS HINMEI, SURYO, "
                strSql = strSql & "ISNULL(KBN, '') AS KBN, TORIKESHI_KBN "
                strSql = strSql & "FROM T_JUCHU WHERE DENPYO_NO = '" & EscQuote(strDenpyoNo) & "'"

                ds = New DataSet
                da = New SqlDataAdapter(strSql, cn)
                da.Fill(ds, "JUCHU")

                If ds.Tables("JUCHU").Rows.Count = 0 Then

                    '新しい伝票番号なので入れる
                    strSql = ""
                    strSql = strSql & "INSERT INTO T_JUCHU "
                    strSql = strSql & "(DENPYO_NO, TOKUISAKI_CD, JUCHUBI, NOKI, HINBAN, HINMEI, SURYO, KBN, TORIKESHI_KBN, TORIKOMI_FILE, TORIKOMI_DT, KOSHIN_USER, KOSHIN_DT) "
                    strSql = strSql & "VALUES ("
                    strSql = strSql & "'" & EscQuote(strDenpyoNo) & "',"
                    strSql = strSql & "'" & EscQuote(strTokuisakiCd) & "',"
                    strSql = strSql & "'" & strJuchubi & "',"
                    strSql = strSql & ToSqlDate(strNoki) & ","
                    strSql = strSql & "'" & EscQuote(strHinban) & "',"
                    strSql = strSql & "'" & EscQuote(strHinmei) & "',"
                    strSql = strSql & CStr(lngSuryo) & ","
                    strSql = strSql & "'" & EscQuote(strKbn) & "',"
                    strSql = strSql & "'" & EscQuote(strTorikeshi) & "',"
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
                       And ToStr(ds.Tables("JUCHU").Rows(0)("KBN")) = strKbn _
                       And ToStr(ds.Tables("JUCHU").Rows(0)("TORIKESHI_KBN")) = strTorikeshi Then

                        intJufuku = intJufuku + 1
                        GoTo TsugiNoGyo

                    End If

                    '2008/07/22 山田 納品を入れた後で受注が変わると請求とずれるので、
                    '                 その場合は更新せずに一覧へ出して田島さんに見てもらう
                    strSql = "SELECT COUNT(*) FROM T_NOHIN WHERE DENPYO_NO = '" & EscQuote(strDenpyoNo) & "' AND TORIKESHI_KBN = '0'"
                    cm = New SqlCommand(strSql, cn)

                    If Val(CStr(cm.ExecuteScalar())) > 0 Then
                        intYokakunin = intYokakunin + 1
                        dt.Rows.Add(CStr(intGyo), strDenpyoNo, "納品済みのため取り込みませんでした(内容が変わっています)")
                        GoTo TsugiNoGyo
                    End If

                    '訂正として上書きする
                    strSql = ""
                    strSql = strSql & "UPDATE T_JUCHU SET "
                    strSql = strSql & "TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "',"
                    strSql = strSql & "JUCHUBI = '" & strJuchubi & "',"
                    strSql = strSql & "NOKI = " & ToSqlDate(strNoki) & ","
                    strSql = strSql & "HINBAN = '" & EscQuote(strHinban) & "',"
                    strSql = strSql & "HINMEI = '" & EscQuote(strHinmei) & "',"
                    strSql = strSql & "SURYO = " & CStr(lngSuryo) & ","
                    strSql = strSql & "KBN = '" & EscQuote(strKbn) & "',"
                    strSql = strSql & "TORIKESHI_KBN = '" & EscQuote(strTorikeshi) & "',"
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
            dgvKekka.Columns(1).Width = 100
            dgvKekka.Columns(2).Width = 570

            lblKekka.Text = "読んだ行 " & CStr(intGyo) & " 件   " & _
                            "取込 " & CStr(intToroku) & " 件   " & _
                            "訂正 " & CStr(intTeisei) & " 件   " & _
                            "重複 " & CStr(intJufuku) & " 件   " & _
                            "要確認 " & CStr(intYokakunin) & " 件   " & _
                            "エラー " & CStr(intError) & " 件"

            If intYokakunin > 0 Or intError > 0 Then
                MsgBox("取込が終わりました。取り込めなかった行があります。一覧を見てください。", MsgBoxStyle.Exclamation)
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
    'yyyyMMdd を yyyy/MM/dd にする  読めなければ空
    '------------------------------------------------------------
    Private Function ToHizuke(ByVal strVal As String) As String

        Dim strWk As String

        strWk = Trim(strVal)

        If Len(strWk) <> 8 Then
            Return ""
        End If

        If IsNumeric(strWk) = False Then
            Return ""
        End If

        strWk = Mid(strWk, 1, 4) & "/" & Mid(strWk, 5, 2) & "/" & Mid(strWk, 7, 2)

        If IsDate(strWk) = False Then
            Return ""
        End If

        Return strWk

    End Function

#End Region

End Class
