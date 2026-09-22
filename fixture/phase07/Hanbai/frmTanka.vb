Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 単価登録
' 作成者     : 山田
'
' 履歴
' 2008/05/12 山田 新規作成
' 2008/07/28 山田 品名を持たせるようにした
'                 受注CSVの品名に品番がそのまま入っている行があり、
'                 そのままでは請求書に出せないため
'============================================================
Public Class frmTanka

    'カレントの品番(空なら新規)
    Private mstrHinban As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中は得意先を変えても検索しない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmTanka_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "単価登録  [" & gUserName & "]"

        mblnLoad = True
        Call SetTokuisakiCombo()
        mblnLoad = False

        Call ClearNyuryoku()
        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '得意先コンボをつくる
    '------------------------------------------------------------
    Private Sub SetTokuisakiCombo()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT TOKUISAKI_CD, TOKUISAKI_CD + '  ' + TOKUISAKI_NM AS HYOJI "
            strSql = strSql & "FROM M_TOKUISAKI "
            strSql = strSql & "WHERE SHIYO_KBN = '0' "
            strSql = strSql & "ORDER BY TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TOKUISAKI")

            cboTokuisaki.DataSource = ds.Tables("TOKUISAKI")
            cboTokuisaki.DisplayMember = "HYOJI"
            cboTokuisaki.ValueMember = "TOKUISAKI_CD"

            If ds.Tables("TOKUISAKI").Rows.Count > 0 Then
                cboTokuisaki.SelectedIndex = 0
            End If

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
    '得意先を変えたら一覧を出しなおす
    '------------------------------------------------------------
    Private Sub cboTokuisaki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTokuisaki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call ClearNyuryoku()
        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '検索
    '------------------------------------------------------------
    Private Sub btnKensaku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaku.Click

        Call HyojiIchiran()

    End Sub

    '------------------------------------------------------------
    '登録(新規・更新兼用)
    '------------------------------------------------------------
    Private Sub btnToroku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToroku.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strTokuisakiCd As String
        Dim strHinban As String

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            strTokuisakiCd = ToStr(cboTokuisaki.SelectedValue)
            strHinban = Trim(txtHinban.Text)

            cn = GetConnection()

            If mstrHinban = "" Then

                '新規 ※得意先と品番でひとつ
                strSql = ""
                strSql = strSql & "SELECT COUNT(*) FROM M_TANKA "
                strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "' "
                strSql = strSql & "AND HINBAN = '" & EscQuote(strHinban) & "'"

                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) > 0 Then
                    MsgBox("この得意先の品番は既に登録されています。一覧から選んでください。", MsgBoxStyle.Exclamation)
                    txtHinban.Focus()
                    mblnShori = False
                    Exit Sub
                End If

                strSql = ""
                strSql = strSql & "INSERT INTO M_TANKA "
                strSql = strSql & "(TOKUISAKI_CD, HINBAN, HINMEI, TANKA, SHIYO_KBN, KOSHIN_USER, KOSHIN_DT) "
                strSql = strSql & "VALUES ("
                strSql = strSql & "'" & EscQuote(strTokuisakiCd) & "',"
                strSql = strSql & "'" & EscQuote(strHinban) & "',"
                strSql = strSql & "'" & EscQuote(Trim(txtHinmei.Text)) & "',"
                strSql = strSql & Val(txtTanka.Text) & ","
                strSql = strSql & "'0',"
                strSql = strSql & "'" & gUserName & "',"
                strSql = strSql & "GETDATE())"

                cm = New SqlCommand(strSql, cn)
                cm.ExecuteNonQuery()

                mstrHinban = strHinban
                txtHinban.ReadOnly = True

                MsgBox("登録しました。", MsgBoxStyle.Information)

            Else

                '更新
                strSql = ""
                strSql = strSql & "UPDATE M_TANKA SET "
                strSql = strSql & "HINMEI = '" & EscQuote(Trim(txtHinmei.Text)) & "',"
                strSql = strSql & "TANKA = " & Val(txtTanka.Text) & ","
                strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
                strSql = strSql & "KOSHIN_DT = GETDATE() "
                strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(strTokuisakiCd) & "' "
                strSql = strSql & "AND HINBAN = '" & EscQuote(mstrHinban) & "'"

                cm = New SqlCommand(strSql, cn)
                cm.ExecuteNonQuery()

                MsgBox("更新しました。", MsgBoxStyle.Information)

            End If

            Call HyojiIchiran()

        Catch ex As Exception

            MsgBox("登録でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If
            mblnShori = False

        End Try

    End Sub

    '------------------------------------------------------------
    '使用しない
    '※過去の納品が単価を持っているので実際には消さない
    '------------------------------------------------------------
    Private Sub btnShiyoNashi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShiyoNashi.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        If mstrHinban = "" Then
            MsgBox("一覧から選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MsgBox("使用しないにします。よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "UPDATE M_TANKA SET "
            strSql = strSql & "SHIYO_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "
            strSql = strSql & "AND HINBAN = '" & EscQuote(mstrHinban) & "'"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("使用しないにしました。", MsgBoxStyle.Information)

            Call ClearNyuryoku()
            Call HyojiIchiran()

        Catch ex As Exception

            MsgBox("更新でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    'クリア
    '------------------------------------------------------------
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        Call ClearNyuryoku()

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

        If cboTokuisaki.SelectedValue Is Nothing Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT HINBAN, HINMEI, TANKA, "
            strSql = strSql & "CASE WHEN SHIYO_KBN = '1' THEN '使用しない' ELSE '' END AS SHIYO_NM "
            strSql = strSql & "FROM M_TANKA "
            strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "

            If Trim(txtKensakuHinban.Text) <> "" Then
                strSql = strSql & "AND HINBAN LIKE '%" & EscQuote(Trim(txtKensakuHinban.Text)) & "%' "
            End If

            strSql = strSql & "ORDER BY HINBAN"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TANKA")

            dgvIchiran.DataSource = ds.Tables("TANKA")

            dgvIchiran.Columns(0).HeaderText = "品番"
            dgvIchiran.Columns(1).HeaderText = "品名"
            dgvIchiran.Columns(2).HeaderText = "単価"
            dgvIchiran.Columns(3).HeaderText = "使用"

            dgvIchiran.Columns(0).Width = 140
            dgvIchiran.Columns(1).Width = 240
            dgvIchiran.Columns(2).Width = 100
            dgvIchiran.Columns(3).Width = 80

            dgvIchiran.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvIchiran.Columns(2).DefaultCellStyle.Format = "#,##0.00"

            lblKensu.Text = "件数 " & CStr(ds.Tables("TANKA").Rows.Count) & " 件"

        Catch ex As Exception

            MsgBox("一覧の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '一覧クリックで入力欄へ転記
    '------------------------------------------------------------
    Private Sub dgvIchiran_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvIchiran.CellClick

        Dim r As DataGridViewRow

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        r = dgvIchiran.Rows(e.RowIndex)

        mstrHinban = ToStr(r.Cells(0).Value)

        txtHinban.Text = mstrHinban
        txtHinban.ReadOnly = True
        txtHinmei.Text = ToStr(r.Cells(1).Value)
        txtTanka.Text = ToStr(r.Cells(2).Value)

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If cboTokuisaki.SelectedValue Is Nothing Then
            MsgBox("得意先を選択してください。", MsgBoxStyle.Exclamation)
            cboTokuisaki.Focus()
            Return False
        End If

        If Trim(txtHinban.Text) = "" Then
            MsgBox("品番を入力してください。", MsgBoxStyle.Exclamation)
            txtHinban.Focus()
            Return False
        End If

        If Trim(txtTanka.Text) = "" Then
            MsgBox("単価を入力してください。", MsgBoxStyle.Exclamation)
            txtTanka.Focus()
            Return False
        End If

        If IsNumeric(txtTanka.Text) = False Then
            MsgBox("単価が数字ではありません。", MsgBoxStyle.Exclamation)
            txtTanka.Focus()
            Return False
        End If

        If Val(txtTanka.Text) < 0 Then
            MsgBox("単価にマイナスは入力できません。", MsgBoxStyle.Exclamation)
            txtTanka.Focus()
            Return False
        End If

        Return True

    End Function

    '------------------------------------------------------------
    '入力欄クリア
    '------------------------------------------------------------
    Private Sub ClearNyuryoku()

        mstrHinban = ""

        txtHinban.Text = ""
        txtHinban.ReadOnly = False
        txtHinmei.Text = ""
        txtTanka.Text = ""

        txtHinban.Focus()

    End Sub

#End Region

End Class
