Option Strict Off
Option Explicit On

Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

'============================================================
' システム名 : 販売
' 機能名     : 請求書発行
' 作成者     : 山田
'
' 履歴
' 2009/03/23 山田 新規作成
'                 Crystal Reports は初めて使う。
'                 レポート(rptSeikyusho.rpt)はexeと同じフォルダに置いて、
'                 起動時に読み込んでいる。項目は下のSetMeisai()で作る表の
'                 列名に合わせてあるので、レポート側を直すときは注意すること
' 2009/04/06 山田 明細が15行を超えたら次の紙に回すようにした
'                 用紙の途中で切れないよう、15行に足りない分は空行を足している
' 2009/04/13 山田 印刷したら印刷日を入れるようにした
' 2009/04/20 山田 レポートに渡す表の列を SetMeisai() の頭にまとめて書いた。
'                 rptはテキストで中身が見られないので、紙のどこに何を置くかは
'                 src の「請求書レイアウト覚書.txt」に書いてある。
'                 列名・型・行数を変えるとレポート側も直すことになるので注意
' 2009/04/22 山田 請求書発行の一覧に得意先の備考を出すようにした
'                 「※請求書2部送付」のような注意書きが備考に入っているため
'============================================================
Public Class frmSeikyusho

    '1枚に入る明細の行数 ※今の請求書の紙に合わせてある
    Private Const MEISAI_GYO As Integer = 15

    '選択中の請求書番号
    Private mstrSeikyuNo As String = ""

    '表示中のレポート
    Private mrpt As ReportDocument = Nothing

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中は月を変えても出しなおさない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmSeikyusho_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer

        Me.Text = "請求書発行  [" & gUserName & "]"

        mblnLoad = True

        cboTsuki.Items.Clear()
        For i = 1 To 12
            cboTsuki.Items.Add(CStr(i))
        Next

        txtNen.Text = CStr(Year(DateAdd("m", -1, Now)))
        cboTsuki.Text = CStr(Month(DateAdd("m", -1, Now)))

        mblnLoad = False

        Call HyojiIchiran()

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '一覧を出す
    '------------------------------------------------------------
    Private Sub btnKensaku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaku.Click

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '月を変えたら出しなおす
    '------------------------------------------------------------
    Private Sub cboTsuki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTsuki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '未印刷だけのチェックを変えたら出しなおす
    '------------------------------------------------------------
    Private Sub chkMiInsatsu_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMiInsatsu.CheckedChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '画面に出す
    '------------------------------------------------------------
    Private Sub btnHyoji_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHyoji.Click

        If mstrSeikyuNo = "" Then
            MsgBox("一覧から請求書を選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If SetReport() = False Then
            Exit Sub
        End If

        crvSeikyusho.ReportSource = mrpt

    End Sub

    '------------------------------------------------------------
    '印刷
    '------------------------------------------------------------
    Private Sub btnInsatsu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsatsu.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strInsatsu As String

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If mstrSeikyuNo = "" Then
                MsgBox("一覧から請求書を選択してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            cn = GetConnection()

            '2009/04/13 山田 二度出しに気付けるように、印刷済みなら確認を出す
            strSql = "SELECT ISNULL(CONVERT(varchar(10), INSATSU_DT, 111), '') FROM T_SEIKYU WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"
            cm = New SqlCommand(strSql, cn)
            strInsatsu = ToStr(cm.ExecuteScalar())

            If strInsatsu <> "" Then
                If MsgBox("請求書 " & mstrSeikyuNo & " は " & strInsatsu & " に印刷しています。" & vbCrLf & _
                          "もう一度印刷しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If
            End If

            If SetReport() = False Then
                mblnShori = False
                Exit Sub
            End If

            crvSeikyusho.ReportSource = mrpt

            mrpt.PrintToPrinter(1, False, 0, 0)

            strSql = ""
            strSql = strSql & "UPDATE T_SEIKYU SET "
            strSql = strSql & "INSATSU_DT = GETDATE(),"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("印刷しました。", MsgBoxStyle.Information)

            Call HyojiIchiran()

        Catch ex As Exception

            MsgBox("印刷でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

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

    '------------------------------------------------------------
    '画面を閉じるときにレポートを片付ける
    '※閉じたあとも開きっぱなしになるとファイルがつかまれたままになる
    '------------------------------------------------------------
    Private Sub frmSeikyusho_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

        If Not mrpt Is Nothing Then
            mrpt.Close()
            mrpt.Dispose()
            mrpt = Nothing
        End If

    End Sub

#End Region

#Region " 一覧 "

    '------------------------------------------------------------
    '締め済みの請求の一覧
    '------------------------------------------------------------
    Private Sub HyojiIchiran()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim strYm As String
        Dim i As Integer

        mstrSeikyuNo = ""

        If Check() = False Then
            Exit Sub
        End If

        strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT S.SEIKYU_NO, S.SEIKYUSAKI_CD, T.TOKUISAKI_NM, "
            strSql = strSql & "CONVERT(varchar(10), S.SHIMEBI, 111) AS SHIMEBI, "
            strSql = strSql & "CONVERT(varchar(10), S.HAKKOBI, 111) AS HAKKOBI, "
            strSql = strSql & "S.URIAGE_GAKU, S.SHOHIZEI, S.SEIKYU_GAKU, "
            strSql = strSql & "ISNULL(CONVERT(varchar(10), S.INSATSU_DT, 111), '') AS INSATSUBI, "
            strSql = strSql & "ISNULL(T.BIKO, '') AS BIKO "
            strSql = strSql & "FROM T_SEIKYU S "
            strSql = strSql & "LEFT JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = S.SEIKYUSAKI_CD "
            strSql = strSql & "WHERE S.SHIME_YM = '" & strYm & "' "
            strSql = strSql & "AND S.TORIKESHI_KBN = '0' "

            If chkMiInsatsu.Checked = True Then
                strSql = strSql & "AND S.INSATSU_DT IS NULL "
            End If

            strSql = strSql & "ORDER BY S.SEIKYU_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "SEIKYU")

            dgvSeikyu.DataSource = ds.Tables("SEIKYU")

            dgvSeikyu.Columns(0).HeaderText = "請求書No"
            dgvSeikyu.Columns(1).HeaderText = "請求先"
            dgvSeikyu.Columns(2).HeaderText = "請求先名"
            dgvSeikyu.Columns(3).HeaderText = "締め日"
            dgvSeikyu.Columns(4).HeaderText = "発行日"
            dgvSeikyu.Columns(5).HeaderText = "今回売上額"
            dgvSeikyu.Columns(6).HeaderText = "消費税"
            dgvSeikyu.Columns(7).HeaderText = "今回請求額"
            dgvSeikyu.Columns(8).HeaderText = "印刷日"
            dgvSeikyu.Columns(9).HeaderText = "得意先の備考"

            dgvSeikyu.Columns(0).Width = 80
            dgvSeikyu.Columns(1).Width = 60
            dgvSeikyu.Columns(2).Width = 180
            dgvSeikyu.Columns(3).Width = 90
            dgvSeikyu.Columns(4).Width = 90
            dgvSeikyu.Columns(5).Width = 100
            dgvSeikyu.Columns(6).Width = 90
            dgvSeikyu.Columns(7).Width = 100
            dgvSeikyu.Columns(8).Width = 90
            dgvSeikyu.Columns(9).Width = 220

            For i = 5 To 7
                dgvSeikyu.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvSeikyu.Columns(i).DefaultCellStyle.Format = "#,##0"
            Next

            lblKensu.Text = "件数 " & CStr(ds.Tables("SEIKYU").Rows.Count) & " 件"

        Catch ex As Exception

            MsgBox("請求の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '一覧クリック
    '------------------------------------------------------------
    Private Sub dgvSeikyu_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSeikyu.CellClick

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        mstrSeikyuNo = ToStr(dgvSeikyu.Rows(e.RowIndex).Cells(0).Value)

    End Sub

#End Region

#Region " 帳票 "

    '------------------------------------------------------------
    'レポートに値を渡す
    '------------------------------------------------------------
    Private Function SetReport() As Boolean

        Dim dt As DataTable
        Dim strRpt As String

        strRpt = Application.StartupPath & "\rptSeikyusho.rpt"

        If System.IO.File.Exists(strRpt) = False Then
            MsgBox("請求書のレポートがありません。" & vbCrLf & strRpt, MsgBoxStyle.Critical)
            Return False
        End If

        dt = SetMeisai()

        If dt Is Nothing Then
            Return False
        End If

        Try

            If Not mrpt Is Nothing Then
                mrpt.Close()
                mrpt.Dispose()
                mrpt = Nothing
            End If

            mrpt = New ReportDocument
            mrpt.Load(strRpt)
            mrpt.SetDataSource(dt)

        Catch ex As Exception

            MsgBox("請求書の作成でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            Return False

        End Try

        Return True

    End Function

    '------------------------------------------------------------
    'レポートに渡す表を作る
    '
    '  表の名前 : SEIKYUSHO   ※rptSeikyusho.rpt はこの名前で作ってある
    '  行数     : 15行 × 枚数。明細が15に足りない分は空行で埋めるので、
    '             明細が0件でも15行になる。枚数は下の MaisuOf() で出す
    '  型       : 全部 String。無償支給の行と空行で単価・金額を空欄にするため、
    '             また表示の丸めをこちら側で確定させるため、数値では渡さない
    '
    '  列(17列) 内容                          例
    '  ------------------------------------------------------------------
    '  SEIKYU_NO       請求書番号             09-0001
    '  HAKKOBI         発行日(和暦)           平成21年4月5日
    '  TOKUISAKI_NM    請求先名               ｻﾝﾌﾟﾙ自動車(株)
    '  ZENKAI_GAKU     前回請求額             1,834,900
    '  NYUKIN_GAKU     入金額                 1,800,000
    '  KURIKOSHI_GAKU  繰越額                 34,900
    '  URIAGE_GAKU     今回売上額             1,926,585
    '  SHOHIZEI        消費税                 96,329
    '  SEIKYU_GAKU     今回請求額             2,057,814
    '  PAGE_NO         何枚目か(1から)        1
    '  JIYO            次葉へ ※最後の紙は空  次葉へ
    '  GOKEI           計 ※最後の紙だけ入る  1,926,585
    '  HIZUKE          明細の日付             3/ 5
    '  HINMEI          品名                   ﾌﾞﾗｹｯﾄ  L
    '  SURYO           数量                   1,400
    '  TANKA           単価 ※無償は空        137
    '  KINGAKU         金額 ※無償は空        191,800
    '
    '  SEIKYU_NO から SEIKYU_GAKU までは全部の行に同じ値を入れてある。
    '  レポート側では PAGE_NO でグループにして、グループヘッダに置くこと。
    '
    '  ※計(GOKEI)はこちらで入れた値をそのまま出すこと。レポートで KINGAKU を
    '    合計しないこと。明細の金額は端数のある値を表示だけ丸めているので、
    '    足し上げると計と数円ずれることがある(経理のExcelも同じ)
    '------------------------------------------------------------
    Private Function SetMeisai() As DataTable

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim dsN As DataSet
        Dim dt As DataTable
        Dim dr As DataRow
        Dim strSql As String
        Dim strTokuisakiNm As String
        Dim strHakkobi As String
        Dim strZenkai As String
        Dim strNyukin As String
        Dim strKurikoshi As String
        Dim strUriage As String
        Dim strZei As String
        Dim strSeikyu As String
        Dim datHakkobi As Date
        Dim intKensu As Integer
        Dim intMaisu As Integer
        Dim intPage As Integer
        Dim intGyo As Integer
        Dim i As Integer

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT S.SEIKYU_NO, T.TOKUISAKI_NM, S.HAKKOBI, "
            strSql = strSql & "S.ZENKAI_GAKU, S.NYUKIN_GAKU, S.KURIKOSHI_GAKU, S.URIAGE_GAKU, S.SHOHIZEI, S.SEIKYU_GAKU "
            strSql = strSql & "FROM T_SEIKYU S "
            strSql = strSql & "LEFT JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = S.SEIKYUSAKI_CD "
            strSql = strSql & "WHERE S.SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "S")

            If ds.Tables("S").Rows.Count = 0 Then
                MsgBox("請求が見つかりません。", MsgBoxStyle.Exclamation)
                Return Nothing
            End If

            strTokuisakiNm = ToStr(ds.Tables("S").Rows(0)("TOKUISAKI_NM"))
            datHakkobi = CDate(ToStr(ds.Tables("S").Rows(0)("HAKKOBI")))
            strHakkobi = Wareki(datHakkobi)

            strZenkai = Format(Val(ToStr(ds.Tables("S").Rows(0)("ZENKAI_GAKU"))), "#,##0")
            strNyukin = Format(Val(ToStr(ds.Tables("S").Rows(0)("NYUKIN_GAKU"))), "#,##0")
            strKurikoshi = Format(Val(ToStr(ds.Tables("S").Rows(0)("KURIKOSHI_GAKU"))), "#,##0")
            strUriage = Format(Val(ToStr(ds.Tables("S").Rows(0)("URIAGE_GAKU"))), "#,##0")
            strZei = Format(Val(ToStr(ds.Tables("S").Rows(0)("SHOHIZEI"))), "#,##0")
            strSeikyu = Format(Val(ToStr(ds.Tables("S").Rows(0)("SEIKYU_GAKU"))), "#,##0")

            '明細 ※締めたときに請求書番号を書いた納品がそのまま明細になる
            strSql = ""
            strSql = strSql & "SELECT CONVERT(varchar(10), N.NOHINBI, 111) AS NOHINBI, "
            strSql = strSql & "ISNULL(N.HINMEI, '') AS HINMEI, N.SURYO, N.TANKA, N.KINGAKU, N.MUSHO_KBN "
            strSql = strSql & "FROM T_NOHIN N "
            strSql = strSql & "WHERE N.SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "' "
            strSql = strSql & "AND N.TORIKESHI_KBN = '0' "
            strSql = strSql & "ORDER BY N.NOHINBI, N.NOHIN_NO"

            dsN = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(dsN, "N")

            intKensu = dsN.Tables("N").Rows.Count

            dt = New DataTable("SEIKYUSHO")
            dt.Columns.Add("SEIKYU_NO", GetType(String))
            dt.Columns.Add("HAKKOBI", GetType(String))
            dt.Columns.Add("TOKUISAKI_NM", GetType(String))
            dt.Columns.Add("ZENKAI_GAKU", GetType(String))
            dt.Columns.Add("NYUKIN_GAKU", GetType(String))
            dt.Columns.Add("KURIKOSHI_GAKU", GetType(String))
            dt.Columns.Add("URIAGE_GAKU", GetType(String))
            dt.Columns.Add("SHOHIZEI", GetType(String))
            dt.Columns.Add("SEIKYU_GAKU", GetType(String))
            dt.Columns.Add("PAGE_NO", GetType(String))
            dt.Columns.Add("JIYO", GetType(String))
            dt.Columns.Add("GOKEI", GetType(String))
            dt.Columns.Add("HIZUKE", GetType(String))
            dt.Columns.Add("HINMEI", GetType(String))
            dt.Columns.Add("SURYO", GetType(String))
            dt.Columns.Add("TANKA", GetType(String))
            dt.Columns.Add("KINGAKU", GetType(String))

            '2009/04/06 山田 15行で1枚。足りない分は空行で埋めて、最後の紙以外は次葉へと出す
            intMaisu = MaisuOf(intKensu)

            For intPage = 1 To intMaisu

                For intGyo = 1 To MEISAI_GYO

                    i = (intPage - 1) * MEISAI_GYO + intGyo - 1

                    dr = dt.NewRow()

                    dr("SEIKYU_NO") = mstrSeikyuNo
                    dr("HAKKOBI") = strHakkobi
                    dr("TOKUISAKI_NM") = strTokuisakiNm
                    dr("ZENKAI_GAKU") = strZenkai
                    dr("NYUKIN_GAKU") = strNyukin
                    dr("KURIKOSHI_GAKU") = strKurikoshi
                    dr("URIAGE_GAKU") = strUriage
                    dr("SHOHIZEI") = strZei
                    dr("SEIKYU_GAKU") = strSeikyu
                    dr("PAGE_NO") = CStr(intPage)

                    If intPage < intMaisu Then
                        dr("JIYO") = "次葉へ"
                        dr("GOKEI") = ""
                    Else
                        dr("JIYO") = ""
                        dr("GOKEI") = strUriage
                    End If

                    If i <= intKensu - 1 Then

                        dr("HIZUKE") = Hizuke(CDate(ToStr(dsN.Tables("N").Rows(i)("NOHINBI"))))
                        dr("HINMEI") = ToStr(dsN.Tables("N").Rows(i)("HINMEI"))
                        dr("SURYO") = Format(Val(ToStr(dsN.Tables("N").Rows(i)("SURYO"))), "#,##0")

                        '無償支給は経理のExcelでも単価と金額を空にしている
                        If ToStr(dsN.Tables("N").Rows(i)("MUSHO_KBN")) = "1" Then
                            dr("TANKA") = ""
                            dr("KINGAKU") = ""
                        Else
                            dr("TANKA") = Format(Val(ToStr(dsN.Tables("N").Rows(i)("TANKA"))), "#,##0.##")
                            dr("KINGAKU") = Format(Val(ToStr(dsN.Tables("N").Rows(i)("KINGAKU"))), "#,##0")
                        End If

                    Else

                        dr("HIZUKE") = ""
                        dr("HINMEI") = ""
                        dr("SURYO") = ""
                        dr("TANKA") = ""
                        dr("KINGAKU") = ""

                    End If

                    dt.Rows.Add(dr)

                Next

            Next

            Return dt

        Catch ex As Exception

            MsgBox("請求書のデータ作成でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
            Return Nothing

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Function

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '明細の件数から紙の枚数を出す ※15行で1枚。0件でも1枚は出す
    '------------------------------------------------------------
    Private Function MaisuOf(ByVal intKensu As Integer) As Integer

        If intKensu <= 0 Then
            Return 1
        End If

        Return Int((intKensu - 1) / MEISAI_GYO) + 1

    End Function

    '------------------------------------------------------------
    '発行日を和暦にする ※今の請求書が和暦なので合わせる
    '------------------------------------------------------------
    Private Function Wareki(ByVal datHi As Date) As String

        'System.Windows.Forms にも Day があってそのままでは通らない
        Return "平成" & CStr(Year(datHi) - 1988) & "年" & CStr(Month(datHi)) & "月" & CStr(Microsoft.VisualBasic.Day(datHi)) & "日"

    End Function

    '------------------------------------------------------------
    '明細の日付 ※今の請求書が 3/ 5 のように桁を揃えてある
    '------------------------------------------------------------
    Private Function Hizuke(ByVal datHi As Date) As String

        Return CStr(Month(datHi)) & "/" & Microsoft.VisualBasic.Right("  " & CStr(Microsoft.VisualBasic.Day(datHi)), 2)

    End Function

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If IsNumeric(txtNen.Text) = False Then
            MsgBox("年が数字ではありません。", MsgBoxStyle.Exclamation)
            txtNen.Focus()
            Return False
        End If

        If Val(txtNen.Text) < 2000 Or Val(txtNen.Text) > 2099 Then
            MsgBox("年は西暦4桁で入力してください。", MsgBoxStyle.Exclamation)
            txtNen.Focus()
            Return False
        End If

        If cboTsuki.Text = "" Then
            MsgBox("月を選択してください。", MsgBoxStyle.Exclamation)
            cboTsuki.Focus()
            Return False
        End If

        Return True

    End Function

#End Region

End Class
