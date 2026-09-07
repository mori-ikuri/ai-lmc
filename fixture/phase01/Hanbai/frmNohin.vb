Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 納品入力
' 作成者     : 山田
'
' 履歴
' 2008/07/14 山田 新規作成
' 2008/08/18 山田 受注数量を超えるときに確認を出すようにした
'                 分納で多めに出ることがあるとのことなので、止めずに確認だけにした
' 2008/09/01 山田 無償支給の分を入れられるようにした
' 2008/09/16 山田 納品の取消を追加。実際には消さずに取消区分を立てる
'============================================================
Public Class frmNohin

    '選択中の受注
    Private mstrDenpyoNo As String = ""

    '選択中の納品(取消用)
    Private mstrNohinNo As String = ""

    '得意先の丸め 0:切り捨て 1:四捨五入
    Private mstrMarumeKbn As String = "0"

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中と転記中は金額を計算しない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmNohin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "納品入力  [" & gUserName & "]"

        mblnLoad = True
        Call SetTokuisakiCombo()
        mblnLoad = False

        Call SetMarumeKbn()
        Call ClearNyuryoku()
        Call HyojiJuchu()

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

    '------------------------------------------------------------
    '得意先の丸めを読んでおく
    '------------------------------------------------------------
    Private Sub SetMarumeKbn()

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        mstrMarumeKbn = "0"

        If cboTokuisaki.SelectedValue Is Nothing Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = "SELECT MARUME_KBN FROM M_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "'"

            cm = New SqlCommand(strSql, cn)
            mstrMarumeKbn = ToStr(cm.ExecuteScalar())

            If mstrMarumeKbn = "" Then
                mstrMarumeKbn = "0"
            End If

            If mstrMarumeKbn = "1" Then
                lblMarume.Text = "丸め 四捨五入"
            Else
                lblMarume.Text = "丸め 切り捨て"
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
    '得意先を変えたら受注を出しなおす
    '------------------------------------------------------------
    Private Sub cboTokuisaki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTokuisaki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call SetMarumeKbn()
        Call ClearNyuryoku()
        Call HyojiJuchu()

    End Sub

    '------------------------------------------------------------
    '検索
    '------------------------------------------------------------
    Private Sub btnKensaku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaku.Click

        Call ClearNyuryoku()
        Call HyojiJuchu()

    End Sub

    '------------------------------------------------------------
    '登録
    '------------------------------------------------------------
    Private Sub btnToroku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToroku.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strNohinNo As String
        Dim strMusho As String
        Dim lngJuchuSuryo As Long
        Dim lngNohinZumi As Long

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            cn = GetConnection()

            '受注の数量と、今までに入れた納品を見る
            strSql = "SELECT SURYO FROM T_JUCHU WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "'"
            cm = New SqlCommand(strSql, cn)
            lngJuchuSuryo = Val(ToStr(cm.ExecuteScalar()))

            strSql = "SELECT ISNULL(SUM(SURYO), 0) FROM T_NOHIN WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "' AND TORIKESHI_KBN = '0'"
            cm = New SqlCommand(strSql, cn)
            lngNohinZumi = Val(ToStr(cm.ExecuteScalar()))

            '2008/08/18 山田 分納で多めに出ることがあるので、止めずに確認だけにする
            If lngNohinZumi + Val(txtSuryo.Text) > lngJuchuSuryo Then

                If MsgBox("受注 " & CStr(lngJuchuSuryo) & " に対して納品が " & CStr(lngNohinZumi + Val(txtSuryo.Text)) & " になります。" & vbCrLf & _
                          "このまま登録しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            strNohinNo = Saiban(cn)

            If strNohinNo = "" Then
                MsgBox("納品Noの採番に失敗しました。もう一度実行してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            If chkMusho.Checked = True Then
                strMusho = "1"
            Else
                strMusho = "0"
            End If

            strSql = ""
            strSql = strSql & "INSERT INTO T_NOHIN "
            strSql = strSql & "(NOHIN_NO, NOHINBI, DENPYO_NO, TOKUISAKI_CD, HINBAN, HINMEI, SURYO, TANKA, KINGAKU, MUSHO_KBN, TORIKESHI_KBN, BIKO, KOSHIN_USER, KOSHIN_DT) "
            strSql = strSql & "VALUES ("
            strSql = strSql & "'" & strNohinNo & "',"
            strSql = strSql & "'" & Format(dtpNohinbi.Value, "yyyy/MM/dd") & "',"
            strSql = strSql & "'" & EscQuote(mstrDenpyoNo) & "',"
            strSql = strSql & "'" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "',"
            strSql = strSql & "'" & EscQuote(Trim(txtHinban.Text)) & "',"
            strSql = strSql & "'" & EscQuote(Trim(txtHinmei.Text)) & "',"
            strSql = strSql & Val(txtSuryo.Text) & ","
            strSql = strSql & Val(txtTanka.Text) & ","
            strSql = strSql & Val(txtKingaku.Text) & ","
            strSql = strSql & "'" & strMusho & "',"
            strSql = strSql & "'0',"
            strSql = strSql & "'" & EscQuote(Trim(txtBiko.Text)) & "',"
            strSql = strSql & "'" & gUserName & "',"
            strSql = strSql & "GETDATE())"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("登録しました。納品No = " & strNohinNo, MsgBoxStyle.Information)

            Call HyojiJuchu()
            Call HyojiNohin()

            '続けて分納を入れることがあるので、伝票番号は残したまま数量だけ空にする
            txtSuryo.Text = ""
            txtKingaku.Text = ""
            txtBiko.Text = ""
            txtSuryo.Focus()

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
    '納品の取消
    '2008/09/16 山田 締めた後に見返すことがあるので物理削除はしないこと
    '------------------------------------------------------------
    Private Sub btnTorikeshi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTorikeshi.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        If mstrNohinNo = "" Then
            MsgBox("下の一覧から取り消す納品を選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MsgBox("納品No " & mstrNohinNo & " を取り消します。よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "UPDATE T_NOHIN SET "
            strSql = strSql & "TORIKESHI_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE NOHIN_NO = '" & EscQuote(mstrNohinNo) & "'"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("取り消しました。", MsgBoxStyle.Information)

            mstrNohinNo = ""

            Call HyojiJuchu()
            Call HyojiNohin()

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
    '受注一覧
    '------------------------------------------------------------
    Private Sub HyojiJuchu()

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
            strSql = strSql & "SELECT J.DENPYO_NO, "
            strSql = strSql & "CONVERT(varchar(10), J.JUCHUBI, 111) AS JUCHUBI, "
            strSql = strSql & "ISNULL(CONVERT(varchar(10), J.NOKI, 111), '') AS NOKI, "
            strSql = strSql & "J.HINBAN, J.HINMEI, J.SURYO, "
            strSql = strSql & "ISNULL((SELECT SUM(N.SURYO) FROM T_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) AS NOHINZUMI, "
            strSql = strSql & "J.SURYO - ISNULL((SELECT SUM(N.SURYO) FROM T_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) AS ZAN "
            strSql = strSql & "FROM T_JUCHU J "
            strSql = strSql & "WHERE J.TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "

            '取消の行は納品しないので出さない
            strSql = strSql & "AND J.TORIKESHI_KBN = 'N' "

            If chkZanAri.Checked = True Then
                strSql = strSql & "AND J.SURYO - ISNULL((SELECT SUM(N.SURYO) FROM T_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) > 0 "
            End If

            strSql = strSql & "ORDER BY J.NOKI, J.DENPYO_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "JUCHU")

            dgvJuchu.DataSource = ds.Tables("JUCHU")

            dgvJuchu.Columns(0).HeaderText = "伝票番号"
            dgvJuchu.Columns(1).HeaderText = "受注日"
            dgvJuchu.Columns(2).HeaderText = "納期"
            dgvJuchu.Columns(3).HeaderText = "品番"
            dgvJuchu.Columns(4).HeaderText = "品名"
            dgvJuchu.Columns(5).HeaderText = "受注数"
            dgvJuchu.Columns(6).HeaderText = "納品済"
            dgvJuchu.Columns(7).HeaderText = "残"

            dgvJuchu.Columns(0).Width = 90
            dgvJuchu.Columns(1).Width = 80
            dgvJuchu.Columns(2).Width = 80
            dgvJuchu.Columns(3).Width = 110
            dgvJuchu.Columns(4).Width = 200
            dgvJuchu.Columns(5).Width = 70
            dgvJuchu.Columns(6).Width = 70
            dgvJuchu.Columns(7).Width = 70

            dgvJuchu.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            lblKensu.Text = "件数 " & CStr(ds.Tables("JUCHU").Rows.Count) & " 件"

        Catch ex As Exception

            MsgBox("受注の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '選んだ受注の納品一覧
    '------------------------------------------------------------
    Private Sub HyojiNohin()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        If mstrDenpyoNo = "" Then
            dgvNohin.DataSource = Nothing
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT NOHIN_NO, "
            strSql = strSql & "CONVERT(varchar(10), NOHINBI, 111) AS NOHINBI, "
            strSql = strSql & "SURYO, TANKA, KINGAKU, "
            strSql = strSql & "CASE WHEN MUSHO_KBN = '1' THEN '無償' ELSE '' END AS MUSHO_NM, "
            strSql = strSql & "BIKO "
            strSql = strSql & "FROM T_NOHIN "
            strSql = strSql & "WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "' "
            strSql = strSql & "AND TORIKESHI_KBN = '0' "
            strSql = strSql & "ORDER BY NOHINBI, NOHIN_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NOHIN")

            dgvNohin.DataSource = ds.Tables("NOHIN")

            dgvNohin.Columns(0).HeaderText = "納品No"
            dgvNohin.Columns(1).HeaderText = "納品日"
            dgvNohin.Columns(2).HeaderText = "数量"
            dgvNohin.Columns(3).HeaderText = "単価"
            dgvNohin.Columns(4).HeaderText = "金額"
            dgvNohin.Columns(5).HeaderText = "無償"
            dgvNohin.Columns(6).HeaderText = "備考"

            dgvNohin.Columns(0).Width = 80
            dgvNohin.Columns(1).Width = 90
            dgvNohin.Columns(2).Width = 70
            dgvNohin.Columns(3).Width = 80
            dgvNohin.Columns(4).Width = 90
            dgvNohin.Columns(5).Width = 50
            dgvNohin.Columns(6).Width = 300

            dgvNohin.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(3).DefaultCellStyle.Format = "#,##0.00"
            dgvNohin.Columns(4).DefaultCellStyle.Format = "#,##0"

        Catch ex As Exception

            MsgBox("納品の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '受注一覧クリックで入力欄へ転記
    '------------------------------------------------------------
    Private Sub dgvJuchu_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvJuchu.CellClick

        Dim r As DataGridViewRow

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        r = dgvJuchu.Rows(e.RowIndex)

        mblnLoad = True

        mstrDenpyoNo = ToStr(r.Cells(0).Value)
        mstrNohinNo = ""

        txtDenpyoNo.Text = mstrDenpyoNo
        txtHinban.Text = ToStr(r.Cells(3).Value)
        txtHinmei.Text = ToStr(r.Cells(4).Value)

        '残っている分をとりあえず入れておく。分納なら打ち直してもらう
        If Val(ToStr(r.Cells(7).Value)) > 0 Then
            txtSuryo.Text = ToStr(r.Cells(7).Value)
        Else
            txtSuryo.Text = ""
        End If

        chkMusho.Checked = False
        txtBiko.Text = ""

        Call SetTanka()

        mblnLoad = False

        Call KingakuKeisan()
        Call HyojiNohin()

        txtSuryo.Focus()

    End Sub

    '------------------------------------------------------------
    '納品一覧クリック(取消するものを選ぶ)
    '------------------------------------------------------------
    Private Sub dgvNohin_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNohin.CellClick

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        mstrNohinNo = ToStr(dgvNohin.Rows(e.RowIndex).Cells(0).Value)

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '単価マスタから単価と品名を持ってくる
    '------------------------------------------------------------
    Private Sub SetTanka()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        lblChui.Text = ""

        If cboTokuisaki.SelectedValue Is Nothing Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT ISNULL(HINMEI, '') AS HINMEI, TANKA FROM M_TANKA "
            strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "
            strSql = strSql & "AND HINBAN = '" & EscQuote(Trim(txtHinban.Text)) & "' "
            strSql = strSql & "AND SHIYO_KBN = '0'"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TANKA")

            If ds.Tables("TANKA").Rows.Count = 0 Then
                txtTanka.Text = ""
                lblChui.Text = "※この品番の単価が登録されていません。単価登録で入れてください"
                Exit Sub
            End If

            '請求書に出す品名は単価マスタのほうを使う
            If ToStr(ds.Tables("TANKA").Rows(0)("HINMEI")) <> "" Then
                txtHinmei.Text = ToStr(ds.Tables("TANKA").Rows(0)("HINMEI"))
            End If

            txtTanka.Text = ToStr(ds.Tables("TANKA").Rows(0)("TANKA"))

        Catch ex As Exception

            MsgBox("単価の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '無償支給
    '2008/09/01 山田 無償の分は金額を持たせない
    '------------------------------------------------------------
    Private Sub chkMusho_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMusho.CheckedChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        If chkMusho.Checked = True Then
            txtTanka.Text = "0"
            txtTanka.ReadOnly = True
        Else
            txtTanka.ReadOnly = False
            Call SetTanka()
        End If

        Call KingakuKeisan()

    End Sub

    '------------------------------------------------------------
    '数量か単価が変わったら金額を出しなおす
    '------------------------------------------------------------
    Private Sub txtSuryo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSuryo.TextChanged, txtTanka.TextChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call KingakuKeisan()

    End Sub

    '------------------------------------------------------------
    '金額計算
    '※丸めは得意先ごと。1行ずつ掛けて整数で持つ
    '------------------------------------------------------------
    Private Sub KingakuKeisan()

        Dim dblKingaku As Double

        If IsNumeric(txtSuryo.Text) = False Or IsNumeric(txtTanka.Text) = False Then
            txtKingaku.Text = ""
            Exit Sub
        End If

        dblKingaku = Val(txtSuryo.Text) * Val(txtTanka.Text)

        txtKingaku.Text = CStr(Marume(dblKingaku, mstrMarumeKbn))

    End Sub

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If mstrDenpyoNo = "" Then
            MsgBox("受注を一覧から選択してください。", MsgBoxStyle.Exclamation)
            Return False
        End If

        If Trim(txtSuryo.Text) = "" Then
            MsgBox("数量を入力してください。", MsgBoxStyle.Exclamation)
            txtSuryo.Focus()
            Return False
        End If

        If IsNumeric(txtSuryo.Text) = False Then
            MsgBox("数量が数字ではありません。", MsgBoxStyle.Exclamation)
            txtSuryo.Focus()
            Return False
        End If

        If Val(txtSuryo.Text) <= 0 Then
            MsgBox("数量は1以上を入力してください。", MsgBoxStyle.Exclamation)
            txtSuryo.Focus()
            Return False
        End If

        If Trim(txtTanka.Text) = "" Then
            MsgBox("単価を入力してください。単価登録で登録されていないかもしれません。", MsgBoxStyle.Exclamation)
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

        '無償支給以外で単価が0のときは打ち忘れのことがある
        If chkMusho.Checked = False And Val(txtTanka.Text) = 0 Then
            If MsgBox("単価が0です。このまま登録しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                txtTanka.Focus()
                Return False
            End If
        End If

        Return True

    End Function

    '------------------------------------------------------------
    '納品No採番
    '※備品管理と同じやり方。同時に登録するとまれに重複するのでリトライを入れてある
    '------------------------------------------------------------
    Private Function Saiban(ByVal cn As SqlConnection) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object
        Dim lngMax As Long
        Dim strNo As String
        Dim i As Integer

        For i = 1 To 3

            strSql = "SELECT MAX(NOHIN_NO) FROM T_NOHIN"

            cm = New SqlCommand(strSql, cn)
            objRet = cm.ExecuteScalar()

            If IsDBNull(objRet) Or objRet Is Nothing Then
                lngMax = 0
            Else
                lngMax = Val(CStr(objRet))
            End If

            strNo = Format(lngMax + 1, "000000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM T_NOHIN WHERE NOHIN_NO = '" & strNo & "'"
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

        mblnLoad = True

        mstrDenpyoNo = ""
        mstrNohinNo = ""

        txtDenpyoNo.Text = ""
        txtHinban.Text = ""
        txtHinmei.Text = ""
        txtSuryo.Text = ""
        txtTanka.Text = ""
        txtTanka.ReadOnly = False
        txtKingaku.Text = ""
        txtBiko.Text = ""
        chkMusho.Checked = False
        dtpNohinbi.Value = Now
        lblChui.Text = ""

        dgvNohin.DataSource = Nothing

        mblnLoad = False

    End Sub

#End Region

End Class
