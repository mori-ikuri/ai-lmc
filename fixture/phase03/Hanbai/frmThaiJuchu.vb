Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ受注一覧
' 作成者     : 山田
'
' 履歴
' 2011/11/08 山田 新規作成
'                 取り込んだ現地の受注を見るだけの画面。
'                 金額はCSVの単価で出しているのでバーツのまま。円には直していない
'============================================================
Public Class frmThaiJuchu

#Region " 画面初期処理 "

    'Form_Load中は一覧を出しなおさない
    Private mblnLoad As Boolean = False

    Private Sub frmThaiJuchu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "タイ受注一覧  [" & gUserName & "]"

        mblnLoad = True

        Call SetTokuisakiCombo()

        '先月の頭から今日まで出しておく
        dtpFrom.Value = DateAdd("m", -1, DateSerial(Year(Now), Month(Now), 1))
        dtpTo.Value = Now

        mblnLoad = False

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '得意先コンボをつくる
    '※名前をまだ付けていないコードも出したいので、受注のほうから拾う
    '------------------------------------------------------------
    Private Sub SetTokuisakiCombo()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim dr As DataRow
        Dim strSql As String

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT DISTINCT J.TOKUISAKI_CD, "
            strSql = strSql & "J.TOKUISAKI_CD + '  ' + ISNULL(M.TOKUISAKI_NM, '') AS HYOJI "
            strSql = strSql & "FROM T_TH_JUCHU J "
            strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = J.TOKUISAKI_CD "
            strSql = strSql & "ORDER BY J.TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TOKUISAKI")

            'すべてを頭に足す
            dr = ds.Tables("TOKUISAKI").NewRow()
            dr("TOKUISAKI_CD") = ""
            dr("HYOJI") = "すべて"
            ds.Tables("TOKUISAKI").Rows.InsertAt(dr, 0)

            cboTokuisaki.DataSource = ds.Tables("TOKUISAKI")
            cboTokuisaki.DisplayMember = "HYOJI"
            cboTokuisaki.ValueMember = "TOKUISAKI_CD"
            cboTokuisaki.SelectedIndex = 0

        Catch ex As Exception

            MsgBox("得意先の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '検索
    '------------------------------------------------------------
    Private Sub btnKensaku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaku.Click

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '得意先を変えたら出しなおす
    '------------------------------------------------------------
    Private Sub cboTokuisaki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTokuisaki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '閉じる
    '------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Me.Close()

    End Sub

#End Region

#Region " 一覧 "

    '------------------------------------------------------------
    '一覧表示
    '------------------------------------------------------------
    Private Sub HyojiIchiran()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim dblGokei As Double
        Dim i As Integer

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT J.DENPYO_NO, J.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') AS TOKUISAKI_NM, "
            strSql = strSql & "CONVERT(varchar(10), J.JUCHUBI, 111) AS JUCHUBI, "
            strSql = strSql & "ISNULL(CONVERT(varchar(10), J.NOKI, 111), '') AS NOKI, "
            strSql = strSql & "ISNULL(J.HINBAN, '') AS HINBAN, ISNULL(J.HINMEI, '') AS HINMEI, "
            strSql = strSql & "J.SURYO, J.TANKA, J.SURYO * J.TANKA AS KINGAKU, "
            strSql = strSql & "ISNULL(J.TSUKA, '') AS TSUKA, "
            strSql = strSql & "ISNULL(J.KBN, '') AS KBN, ISNULL(J.JOTAI_KBN, '') AS JOTAI_KBN, "
            strSql = strSql & "ISNULL(J.TORIKOMI_FILE, '') AS TORIKOMI_FILE "
            strSql = strSql & "FROM T_TH_JUCHU J "
            strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = J.TOKUISAKI_CD "
            strSql = strSql & "WHERE 1 = 1 "

            If ToStr(cboTokuisaki.SelectedValue) <> "" Then
                strSql = strSql & "AND J.TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "
            End If

            If chkKikan.Checked = True Then
                strSql = strSql & "AND J.JUCHUBI >= '" & Format(dtpFrom.Value, "yyyy/MM/dd") & "' "
                strSql = strSql & "AND J.JUCHUBI <= '" & Format(dtpTo.Value, "yyyy/MM/dd") & "' "
            End If

            If Trim(txtHinban.Text) <> "" Then
                strSql = strSql & "AND J.HINBAN LIKE '%" & EscQuote(Trim(txtHinban.Text)) & "%' "
            End If

            strSql = strSql & "ORDER BY J.JUCHUBI, J.DENPYO_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "JUCHU")

            dgvIchiran.DataSource = ds.Tables("JUCHU")

            dgvIchiran.Columns(0).HeaderText = "伝票番号"
            dgvIchiran.Columns(1).HeaderText = "得意先"
            dgvIchiran.Columns(2).HeaderText = "得意先名"
            dgvIchiran.Columns(3).HeaderText = "受注日"
            dgvIchiran.Columns(4).HeaderText = "納期"
            dgvIchiran.Columns(5).HeaderText = "品番"
            dgvIchiran.Columns(6).HeaderText = "品名"
            dgvIchiran.Columns(7).HeaderText = "数量"
            dgvIchiran.Columns(8).HeaderText = "単価"
            dgvIchiran.Columns(9).HeaderText = "金額"
            dgvIchiran.Columns(10).HeaderText = "通貨"
            dgvIchiran.Columns(11).HeaderText = "区分"
            dgvIchiran.Columns(12).HeaderText = "状態"
            dgvIchiran.Columns(13).HeaderText = "取込ファイル"

            dgvIchiran.Columns(0).Width = 95
            dgvIchiran.Columns(1).Width = 65
            dgvIchiran.Columns(2).Width = 130
            dgvIchiran.Columns(3).Width = 80
            dgvIchiran.Columns(4).Width = 80
            dgvIchiran.Columns(5).Width = 90
            dgvIchiran.Columns(6).Width = 120
            dgvIchiran.Columns(7).Width = 60
            dgvIchiran.Columns(8).Width = 70
            dgvIchiran.Columns(9).Width = 90
            dgvIchiran.Columns(10).Width = 45
            dgvIchiran.Columns(11).Width = 40
            dgvIchiran.Columns(12).Width = 40
            dgvIchiran.Columns(13).Width = 160

            dgvIchiran.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(7).DefaultCellStyle.Format = "#,##0"
            dgvIchiran.Columns(8).DefaultCellStyle.Format = "#,##0.00"
            dgvIchiran.Columns(9).DefaultCellStyle.Format = "#,##0.00"

            '合計 ※通貨は混ぜずにそのまま足している。今のところバーツしか来ていない
            dblGokei = 0

            For i = 0 To ds.Tables("JUCHU").Rows.Count - 1
                dblGokei = dblGokei + Val(ToStr(ds.Tables("JUCHU").Rows(i)("KINGAKU")))
            Next

            lblKensu.Text = "件数 " & CStr(ds.Tables("JUCHU").Rows.Count) & " 件   " & _
                            "金額計 " & Format(dblGokei, "#,##0.00") & "  ※CSVの通貨のまま"

        Catch ex As Exception

            MsgBox("一覧の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

#End Region

End Class
