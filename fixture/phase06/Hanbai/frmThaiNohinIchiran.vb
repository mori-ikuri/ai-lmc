Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ納品一覧
' 作成者     : 山田
'
' 履歴
' 2012/02/13 山田 新規作成
'                 経理でタイの納めた分を見るための画面。
'                 タイ受注一覧(frmThaiJuchu)は橋本さんが使っているので触らない。
'                 金額はCSVの通貨(バーツ)のまま。丸めていないし円にも直していない
'============================================================
Public Class frmThaiNohinIchiran

    'Form_Load中は一覧を出しなおさない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmThaiNohinIchiran_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "タイ納品一覧  [" & gUserName & "]"

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
    '※名前をまだ付けていないコードにも納品が入るので、受注のほうから拾う
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
    'まとめ方を変えたら出しなおす
    '------------------------------------------------------------
    Private Sub rdoMeisai_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoMeisai.CheckedChanged, rdoTokuisaki.CheckedChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        '2つのラジオが両方とも変更で入ってくるので、オンになったほうだけで動かす
        If sender.Checked = False Then
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

        If rdoTokuisaki.Checked = True Then
            Call HyojiTokuisakiBetsu()
        Else
            Call HyojiMeisai()
        End If

    End Sub

    '------------------------------------------------------------
    '明細で出す
    '------------------------------------------------------------
    Private Sub HyojiMeisai()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim dblGokei As Double
        Dim lngSuryo As Long
        Dim i As Integer

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT N.NOHIN_NO, "
            strSql = strSql & "CONVERT(varchar(10), N.NOHINBI, 111) AS NOHINBI, "
            strSql = strSql & "N.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') AS TOKUISAKI_NM, "
            strSql = strSql & "N.DENPYO_NO, "
            strSql = strSql & "ISNULL(N.HINBAN, '') AS HINBAN, ISNULL(N.HINMEI, '') AS HINMEI, "
            strSql = strSql & "N.SURYO, N.TANKA, N.KINGAKU, ISNULL(N.TSUKA, '') AS TSUKA, "
            strSql = strSql & "ISNULL(N.BIKO, '') AS BIKO "
            strSql = strSql & "FROM T_TH_NOHIN N "
            strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = N.TOKUISAKI_CD "
            strSql = strSql & "WHERE N.TORIKESHI_KBN = '0' "

            If ToStr(cboTokuisaki.SelectedValue) <> "" Then
                strSql = strSql & "AND N.TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "
            End If

            strSql = strSql & "AND N.NOHINBI >= '" & Format(dtpFrom.Value, "yyyy/MM/dd") & "' "
            strSql = strSql & "AND N.NOHINBI <= '" & Format(dtpTo.Value, "yyyy/MM/dd") & "' "

            If Trim(txtHinban.Text) <> "" Then
                strSql = strSql & "AND N.HINBAN LIKE '%" & EscQuote(Trim(txtHinban.Text)) & "%' "
            End If

            strSql = strSql & "ORDER BY N.NOHINBI, N.NOHIN_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NOHIN")

            dgvIchiran.DataSource = ds.Tables("NOHIN")

            dgvIchiran.Columns(0).HeaderText = "納品No"
            dgvIchiran.Columns(1).HeaderText = "納品日"
            dgvIchiran.Columns(2).HeaderText = "得意先"
            dgvIchiran.Columns(3).HeaderText = "得意先名"
            dgvIchiran.Columns(4).HeaderText = "伝票番号"
            dgvIchiran.Columns(5).HeaderText = "品番"
            dgvIchiran.Columns(6).HeaderText = "品名"
            dgvIchiran.Columns(7).HeaderText = "数量"
            dgvIchiran.Columns(8).HeaderText = "単価"
            dgvIchiran.Columns(9).HeaderText = "金額"
            dgvIchiran.Columns(10).HeaderText = "通貨"
            dgvIchiran.Columns(11).HeaderText = "備考"

            dgvIchiran.Columns(0).Width = 70
            dgvIchiran.Columns(1).Width = 80
            dgvIchiran.Columns(2).Width = 65
            dgvIchiran.Columns(3).Width = 130
            dgvIchiran.Columns(4).Width = 95
            dgvIchiran.Columns(5).Width = 90
            dgvIchiran.Columns(6).Width = 120
            dgvIchiran.Columns(7).Width = 60
            dgvIchiran.Columns(8).Width = 70
            dgvIchiran.Columns(9).Width = 95
            dgvIchiran.Columns(10).Width = 45
            dgvIchiran.Columns(11).Width = 120

            dgvIchiran.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(7).DefaultCellStyle.Format = "#,##0"
            dgvIchiran.Columns(8).DefaultCellStyle.Format = "#,##0.00"
            dgvIchiran.Columns(9).DefaultCellStyle.Format = "#,##0.00"

            '合計 ※通貨は混ぜずにそのまま足している。今のところバーツしか来ていない
            dblGokei = 0
            lngSuryo = 0

            For i = 0 To ds.Tables("NOHIN").Rows.Count - 1
                lngSuryo = lngSuryo + Val(ToStr(ds.Tables("NOHIN").Rows(i)("SURYO")))
                dblGokei = dblGokei + Val(ToStr(ds.Tables("NOHIN").Rows(i)("KINGAKU")))
            Next

            lblKensu.Text = "件数 " & CStr(ds.Tables("NOHIN").Rows.Count) & " 件   " & _
                            "数量計 " & Format(lngSuryo, "#,##0") & "   " & _
                            "金額計 " & Format(dblGokei, "#,##0.00") & "  ※CSVの通貨のまま"

        Catch ex As Exception

            MsgBox("一覧の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '得意先ごとにまとめて出す
    '※田島さんは月の合計だけ見たいとのことなのでこちらも用意する
    '------------------------------------------------------------
    Private Sub HyojiTokuisakiBetsu()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim dblGokei As Double
        Dim lngSuryo As Long
        Dim i As Integer

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT N.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') AS TOKUISAKI_NM, "
            strSql = strSql & "COUNT(*) AS KENSU, SUM(N.SURYO) AS SURYO, SUM(N.KINGAKU) AS KINGAKU "
            strSql = strSql & "FROM T_TH_NOHIN N "
            strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = N.TOKUISAKI_CD "
            strSql = strSql & "WHERE N.TORIKESHI_KBN = '0' "

            If ToStr(cboTokuisaki.SelectedValue) <> "" Then
                strSql = strSql & "AND N.TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "
            End If

            strSql = strSql & "AND N.NOHINBI >= '" & Format(dtpFrom.Value, "yyyy/MM/dd") & "' "
            strSql = strSql & "AND N.NOHINBI <= '" & Format(dtpTo.Value, "yyyy/MM/dd") & "' "

            If Trim(txtHinban.Text) <> "" Then
                strSql = strSql & "AND N.HINBAN LIKE '%" & EscQuote(Trim(txtHinban.Text)) & "%' "
            End If

            strSql = strSql & "GROUP BY N.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') "
            strSql = strSql & "ORDER BY N.TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NOHIN")

            dgvIchiran.DataSource = ds.Tables("NOHIN")

            dgvIchiran.Columns(0).HeaderText = "得意先"
            dgvIchiran.Columns(1).HeaderText = "得意先名"
            dgvIchiran.Columns(2).HeaderText = "件数"
            dgvIchiran.Columns(3).HeaderText = "数量"
            dgvIchiran.Columns(4).HeaderText = "金額"

            dgvIchiran.Columns(0).Width = 90
            dgvIchiran.Columns(1).Width = 240
            dgvIchiran.Columns(2).Width = 80
            dgvIchiran.Columns(3).Width = 110
            dgvIchiran.Columns(4).Width = 140

            dgvIchiran.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(3).DefaultCellStyle.Format = "#,##0"
            dgvIchiran.Columns(4).DefaultCellStyle.Format = "#,##0.00"

            dblGokei = 0
            lngSuryo = 0

            For i = 0 To ds.Tables("NOHIN").Rows.Count - 1
                lngSuryo = lngSuryo + Val(ToStr(ds.Tables("NOHIN").Rows(i)("SURYO")))
                dblGokei = dblGokei + Val(ToStr(ds.Tables("NOHIN").Rows(i)("KINGAKU")))
            Next

            lblKensu.Text = "得意先 " & CStr(ds.Tables("NOHIN").Rows.Count) & " 社   " & _
                            "数量計 " & Format(lngSuryo, "#,##0") & "   " & _
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
