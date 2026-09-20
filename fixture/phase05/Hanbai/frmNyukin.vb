Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 入金入力
' 作成者     : 山田
'
' 履歴
' 2009/03/02 山田 新規作成
'                 どの納品に充てるかは経理でも見ていないとのことなので、
'                 請求先ごとに入った金額を記録するだけにしてある
' 2009/03/30 山田 請求残を超える入金のときに確認を出すようにした
'============================================================
Public Class frmNyukin

    '選択中の入金No(取消用)
    Private mstrNyukinNo As String = ""

    '直近の請求の今回請求額と締め日
    Private mdblSeikyuGaku As Double = 0
    Private mstrSeikyuShimebi As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中は得意先を変えても出しなおさない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmNyukin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "入金入力  [" & gUserName & "]"

        '入金の方法
        '※経理で使っている言い方に合わせてある。増えたらここに足すこと
        cboHoho.Items.Clear()
        cboHoho.Items.Add("振込")
        cboHoho.Items.Add("現金")
        cboHoho.Items.Add("手形")
        cboHoho.Items.Add("相殺")
        cboHoho.Items.Add("その他")

        mblnLoad = True
        Call SetSeikyusakiCombo()
        mblnLoad = False

        Call ClearNyuryoku()
        Call HyojiSeikyu()
        Call HyojiNyukin()

    End Sub

    '------------------------------------------------------------
    '請求先コンボをつくる
    '※請求先が空の得意先が、そのまま自分あての請求先になる
    '------------------------------------------------------------
    Private Sub SetSeikyusakiCombo()

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
            strSql = strSql & "AND (SEIKYUSAKI_CD IS NULL OR SEIKYUSAKI_CD = '') "
            strSql = strSql & "ORDER BY TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TOKUISAKI")

            cboSeikyusaki.DataSource = ds.Tables("TOKUISAKI")
            cboSeikyusaki.DisplayMember = "HYOJI"
            cboSeikyusaki.ValueMember = "TOKUISAKI_CD"

            If ds.Tables("TOKUISAKI").Rows.Count > 0 Then
                cboSeikyusaki.SelectedIndex = 0
            End If

        Catch ex As Exception

            MsgBox("請求先の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '請求先を変えたら出しなおす
    '------------------------------------------------------------
    Private Sub cboSeikyusaki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSeikyusaki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call ClearNyuryoku()
        Call HyojiSeikyu()
        Call HyojiNyukin()

    End Sub

    '------------------------------------------------------------
    '登録
    '------------------------------------------------------------
    Private Sub btnToroku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToroku.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strNyukinNo As String
        Dim dblZan As Double

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            '2009/03/30 山田 多く入っていることもあるとのことなので、止めずに確認だけにする
            dblZan = ZanGaku()

            If Val(txtKingaku.Text) > dblZan Then

                If MsgBox("請求の残 " & Format(dblZan, "#,##0") & " 円を超えます。" & vbCrLf & _
                          "このまま登録しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            cn = GetConnection()

            strNyukinNo = Saiban(cn)

            If strNyukinNo = "" Then
                MsgBox("入金Noの採番に失敗しました。もう一度実行してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            strSql = ""
            strSql = strSql & "INSERT INTO T_NYUKIN "
            strSql = strSql & "(NYUKIN_NO, NYUKINBI, SEIKYUSAKI_CD, KINGAKU, HOHO, BIKO, TORIKESHI_KBN, KOSHIN_USER, KOSHIN_DT) "
            strSql = strSql & "VALUES ("
            strSql = strSql & "'" & strNyukinNo & "',"
            strSql = strSql & "'" & Format(dtpNyukinbi.Value, "yyyy/MM/dd") & "',"
            strSql = strSql & "'" & EscQuote(ToStr(cboSeikyusaki.SelectedValue)) & "',"
            strSql = strSql & Val(txtKingaku.Text) & ","
            strSql = strSql & "'" & EscQuote(cboHoho.Text) & "',"
            strSql = strSql & "'" & EscQuote(Trim(txtBiko.Text)) & "',"
            strSql = strSql & "'0',"
            strSql = strSql & "'" & gUserName & "',"
            strSql = strSql & "GETDATE())"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("登録しました。入金No = " & strNyukinNo, MsgBoxStyle.Information)

            txtKingaku.Text = ""
            txtBiko.Text = ""

            Call HyojiSeikyu()
            Call HyojiNyukin()

            txtKingaku.Focus()

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
    '入金の取消
    '※納品と同じで、実際には消さずに取消区分を立てる
    '------------------------------------------------------------
    Private Sub btnTorikeshi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTorikeshi.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        If mstrNyukinNo = "" Then
            MsgBox("下の一覧から取り消す入金を選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MsgBox("入金No " & mstrNyukinNo & " を取り消します。" & vbCrLf & _
                  "締め済みの請求に入っている入金だと、請求書の入金額と合わなくなります。" & vbCrLf & vbCrLf & _
                  "よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "UPDATE T_NYUKIN SET "
            strSql = strSql & "TORIKESHI_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE NYUKIN_NO = '" & EscQuote(mstrNyukinNo) & "'"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("取り消しました。", MsgBoxStyle.Information)

            mstrNyukinNo = ""

            Call HyojiSeikyu()
            Call HyojiNyukin()

        Catch ex As Exception

            MsgBox("取消でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

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
    '請求の一覧と残高
    '------------------------------------------------------------
    Private Sub HyojiSeikyu()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim i As Integer

        mdblSeikyuGaku = 0
        mstrSeikyuShimebi = ""
        lblZan.Text = ""

        If cboSeikyusaki.SelectedValue Is Nothing Then
            dgvSeikyu.DataSource = Nothing
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT SEIKYU_NO, "
            strSql = strSql & "CONVERT(varchar(10), SHIMEBI, 111) AS SHIMEBI, "
            strSql = strSql & "CONVERT(varchar(10), HAKKOBI, 111) AS HAKKOBI, "
            strSql = strSql & "ZENKAI_GAKU, NYUKIN_GAKU, KURIKOSHI_GAKU, URIAGE_GAKU, SHOHIZEI, SEIKYU_GAKU, "
            strSql = strSql & "ISNULL(CONVERT(varchar(10), INSATSU_DT, 111), '') AS INSATSUBI "
            strSql = strSql & "FROM T_SEIKYU "
            strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(ToStr(cboSeikyusaki.SelectedValue)) & "' "
            strSql = strSql & "AND TORIKESHI_KBN = '0' "
            strSql = strSql & "ORDER BY SHIMEBI DESC"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "SEIKYU")

            dgvSeikyu.DataSource = ds.Tables("SEIKYU")

            dgvSeikyu.Columns(0).HeaderText = "請求書No"
            dgvSeikyu.Columns(1).HeaderText = "締め日"
            dgvSeikyu.Columns(2).HeaderText = "発行日"
            dgvSeikyu.Columns(3).HeaderText = "前回請求額"
            dgvSeikyu.Columns(4).HeaderText = "入金額"
            dgvSeikyu.Columns(5).HeaderText = "繰越額"
            dgvSeikyu.Columns(6).HeaderText = "今回売上額"
            dgvSeikyu.Columns(7).HeaderText = "消費税"
            dgvSeikyu.Columns(8).HeaderText = "今回請求額"
            dgvSeikyu.Columns(9).HeaderText = "印刷日"

            dgvSeikyu.Columns(0).Width = 75
            dgvSeikyu.Columns(1).Width = 85
            dgvSeikyu.Columns(2).Width = 85
            dgvSeikyu.Columns(3).Width = 90
            dgvSeikyu.Columns(4).Width = 90
            dgvSeikyu.Columns(5).Width = 90
            dgvSeikyu.Columns(6).Width = 90
            dgvSeikyu.Columns(7).Width = 80
            dgvSeikyu.Columns(8).Width = 90
            dgvSeikyu.Columns(9).Width = 85

            For i = 3 To 8
                dgvSeikyu.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvSeikyu.Columns(i).DefaultCellStyle.Format = "#,##0"
            Next

            '一番新しい請求が今の残高のもと
            If ds.Tables("SEIKYU").Rows.Count > 0 Then
                mdblSeikyuGaku = Val(ToStr(ds.Tables("SEIKYU").Rows(0)("SEIKYU_GAKU")))
                mstrSeikyuShimebi = ToStr(ds.Tables("SEIKYU").Rows(0)("SHIMEBI"))
            End If

            Call HyojiZan()

        Catch ex As Exception

            MsgBox("請求の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '入金の一覧
    '------------------------------------------------------------
    Private Sub HyojiNyukin()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        If cboSeikyusaki.SelectedValue Is Nothing Then
            dgvNyukin.DataSource = Nothing
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT NYUKIN_NO, "
            strSql = strSql & "CONVERT(varchar(10), NYUKINBI, 111) AS NYUKINBI, "
            strSql = strSql & "KINGAKU, ISNULL(HOHO, '') AS HOHO, ISNULL(BIKO, '') AS BIKO "
            strSql = strSql & "FROM T_NYUKIN "
            strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(ToStr(cboSeikyusaki.SelectedValue)) & "' "
            strSql = strSql & "AND TORIKESHI_KBN = '0' "
            strSql = strSql & "ORDER BY NYUKINBI DESC, NYUKIN_NO DESC"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NYUKIN")

            dgvNyukin.DataSource = ds.Tables("NYUKIN")

            dgvNyukin.Columns(0).HeaderText = "入金No"
            dgvNyukin.Columns(1).HeaderText = "入金日"
            dgvNyukin.Columns(2).HeaderText = "金額"
            dgvNyukin.Columns(3).HeaderText = "方法"
            dgvNyukin.Columns(4).HeaderText = "備考"

            dgvNyukin.Columns(0).Width = 80
            dgvNyukin.Columns(1).Width = 90
            dgvNyukin.Columns(2).Width = 100
            dgvNyukin.Columns(3).Width = 70
            dgvNyukin.Columns(4).Width = 400

            dgvNyukin.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNyukin.Columns(2).DefaultCellStyle.Format = "#,##0"

            lblKensu.Text = "入金 " & CStr(ds.Tables("NYUKIN").Rows.Count) & " 件"

        Catch ex As Exception

            MsgBox("入金の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '入金一覧クリック(取消するものを選ぶ)
    '------------------------------------------------------------
    Private Sub dgvNyukin_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNyukin.CellClick

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        mstrNyukinNo = ToStr(dgvNyukin.Rows(e.RowIndex).Cells(0).Value)

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '残高の表示
    '※一番新しい請求の今回請求額から、その締め日より後の入金を引いたもの
    '------------------------------------------------------------
    Private Sub HyojiZan()

        Dim dblZan As Double

        If mstrSeikyuShimebi = "" Then
            lblZan.Text = "請求がまだありません"
            Exit Sub
        End If

        dblZan = ZanGaku()

        lblZan.Text = "請求額 " & Format(mdblSeikyuGaku, "#,##0") & " 円   " & _
                      "その後の入金 " & Format(mdblSeikyuGaku - dblZan, "#,##0") & " 円   " & _
                      "残 " & Format(dblZan, "#,##0") & " 円"

    End Sub

    '------------------------------------------------------------
    '残っている金額
    '------------------------------------------------------------
    Private Function ZanGaku() As Double

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim dblNyukin As Double

        If mstrSeikyuShimebi = "" Then
            Return 0
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT ISNULL(SUM(KINGAKU), 0) FROM T_NYUKIN "
            strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(ToStr(cboSeikyusaki.SelectedValue)) & "' "
            strSql = strSql & "AND TORIKESHI_KBN = '0' "
            strSql = strSql & "AND NYUKINBI > '" & mstrSeikyuShimebi & "'"

            cm = New SqlCommand(strSql, cn)
            dblNyukin = Val(CStr(cm.ExecuteScalar()))

        Catch ex As Exception

            MsgBox("残高の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

        Return mdblSeikyuGaku - dblNyukin

    End Function

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If cboSeikyusaki.SelectedValue Is Nothing Then
            MsgBox("請求先を選択してください。", MsgBoxStyle.Exclamation)
            Return False
        End If

        If Trim(txtKingaku.Text) = "" Then
            MsgBox("入金額を入力してください。", MsgBoxStyle.Exclamation)
            txtKingaku.Focus()
            Return False
        End If

        If IsNumeric(txtKingaku.Text) = False Then
            MsgBox("入金額が数字ではありません。", MsgBoxStyle.Exclamation)
            txtKingaku.Focus()
            Return False
        End If

        If Val(txtKingaku.Text) <= 0 Then
            MsgBox("入金額は1以上を入力してください。", MsgBoxStyle.Exclamation)
            txtKingaku.Focus()
            Return False
        End If

        If cboHoho.Text = "" Then
            MsgBox("方法を選択してください。", MsgBoxStyle.Exclamation)
            cboHoho.Focus()
            Return False
        End If

        Return True

    End Function

    '------------------------------------------------------------
    '入金No採番
    '※納品と同じやり方
    '------------------------------------------------------------
    Private Function Saiban(ByVal cn As SqlConnection) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object
        Dim lngMax As Long
        Dim strNo As String
        Dim i As Integer

        For i = 1 To 3

            strSql = "SELECT MAX(NYUKIN_NO) FROM T_NYUKIN"

            cm = New SqlCommand(strSql, cn)
            objRet = cm.ExecuteScalar()

            If IsDBNull(objRet) Or objRet Is Nothing Then
                lngMax = 0
            Else
                lngMax = Val(CStr(objRet))
            End If

            strNo = Format(lngMax + 1, "000000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM T_NYUKIN WHERE NYUKIN_NO = '" & strNo & "'"
            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) = 0 Then
                Return strNo
            End If

            System.Threading.Thread.Sleep(200)

        Next

        Return ""

    End Function

    '------------------------------------------------------------
    '入力欄クリア
    '------------------------------------------------------------
    Private Sub ClearNyuryoku()

        mstrNyukinNo = ""

        dtpNyukinbi.Value = Now
        txtKingaku.Text = ""
        cboHoho.Text = "振込"
        txtBiko.Text = ""

        txtKingaku.Focus()

    End Sub

#End Region

End Class
