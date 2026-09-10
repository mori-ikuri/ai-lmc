Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ納品入力
' 作成者     : 山田
'
' 履歴
' 2012/02/13 山田 新規作成
'                 タイの受注に対して納めた分を入れる画面。
'                 日本の納品入力(frmNohin)は毎月動いているので触らない。
'                 金額はバーツのまま。丸めていないし円にも直していない
'============================================================
Public Class frmThaiNohin

    '選択中の受注
    Private mstrDenpyoNo As String = ""

    '選択中の納品(取消用)
    Private mstrNohinNo As String = ""

    '選択中の受注の通貨
    Private mstrTsuka As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中と転記中は金額を計算しない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmThaiNohin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "タイ納品入力  [" & gUserName & "]"

        mblnLoad = True
        Call SetTokuisakiCombo()
        mblnLoad = False

        Call ClearNyuryoku()
        Call HyojiJuchu()

    End Sub

    '------------------------------------------------------------
    '得意先コンボをつくる
    '※名前をまだ付けていないコードにも納品が入るので、受注のほうから拾う
    '------------------------------------------------------------
    Private Sub SetTokuisakiCombo()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT DISTINCT J.TOKUISAKI_CD, "
            strSql = strSql & "J.TOKUISAKI_CD + '  ' + ISNULL(M.TOKUISAKI_NM, '(名前未登録)') AS HYOJI "
            strSql = strSql & "FROM T_TH_JUCHU J "
            strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = J.TOKUISAKI_CD "
            strSql = strSql & "ORDER BY J.TOKUISAKI_CD"

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
    '得意先を変えたら受注を出しなおす
    '------------------------------------------------------------
    Private Sub cboTokuisaki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTokuisaki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

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
        Dim strJotai As String
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

            '一覧を出した後で取込が走って取消になっていることがあるので、もう一度みる
            strSql = "SELECT ISNULL(JOTAI_KBN, '') FROM T_TH_JUCHU WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "'"
            cm = New SqlCommand(strSql, cn)
            strJotai = ToStr(cm.ExecuteScalar())

            If strJotai = "X" Then
                MsgBox("この受注は取り消されています。検索しなおしてください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            '受注の数量と、今までに入れた納品を見る
            strSql = "SELECT SURYO FROM T_TH_JUCHU WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "'"
            cm = New SqlCommand(strSql, cn)
            lngJuchuSuryo = Val(ToStr(cm.ExecuteScalar()))

            strSql = "SELECT ISNULL(SUM(SURYO), 0) FROM T_TH_NOHIN WHERE DENPYO_NO = '" & EscQuote(mstrDenpyoNo) & "' AND TORIKESHI_KBN = '0'"
            cm = New SqlCommand(strSql, cn)
            lngNohinZumi = Val(ToStr(cm.ExecuteScalar()))

            '日本と同じで、多めに出ることがあるので止めずに確認だけにする
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

            strSql = ""
            strSql = strSql & "INSERT INTO T_TH_NOHIN "
            strSql = strSql & "(NOHIN_NO, NOHINBI, DENPYO_NO, TOKUISAKI_CD, HINBAN, HINMEI, SURYO, TANKA, KINGAKU, TSUKA, TORIKESHI_KBN, BIKO, KOSHIN_USER, KOSHIN_DT) "
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
            strSql = strSql & "'" & EscQuote(mstrTsuka) & "',"
            strSql = strSql & "'0',"
            strSql = strSql & "'" & EscQuote(Trim(txtBiko.Text)) & "',"
            strSql = strSql & "'" & gUserName & "',"
            strSql = strSql & "GETDATE())"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("登録しました。納品No = " & strNohinNo, MsgBoxStyle.Information)

            Call HyojiJuchu()
            Call HyojiNohin()

            '続けて分けて入れることがあるので、伝票番号は残したまま数量だけ空にする
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
    '※日本と同じで物理削除はしない
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
            strSql = strSql & "UPDATE T_TH_NOHIN SET "
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
    '※10列目が X の受注は取り消されたものとみて出さない
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
            strSql = strSql & "ISNULL(J.HINBAN, '') AS HINBAN, ISNULL(J.HINMEI, '') AS HINMEI, J.SURYO, "
            strSql = strSql & "ISNULL((SELECT SUM(N.SURYO) FROM T_TH_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) AS NOHINZUMI, "
            strSql = strSql & "J.SURYO - ISNULL((SELECT SUM(N.SURYO) FROM T_TH_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) AS ZAN, "
            strSql = strSql & "J.TANKA, ISNULL(J.TSUKA, '') AS TSUKA, ISNULL(J.KBN, '') AS KBN "
            strSql = strSql & "FROM T_TH_JUCHU J "
            strSql = strSql & "WHERE J.TOKUISAKI_CD = '" & EscQuote(ToStr(cboTokuisaki.SelectedValue)) & "' "

            '取り消された受注には納品しないので出さない
            strSql = strSql & "AND ISNULL(J.JOTAI_KBN, '') <> 'X' "

            If chkZanAri.Checked = True Then
                strSql = strSql & "AND J.SURYO - ISNULL((SELECT SUM(N.SURYO) FROM T_TH_NOHIN N WHERE N.DENPYO_NO = J.DENPYO_NO AND N.TORIKESHI_KBN = '0'), 0) > 0 "
            End If

            '納期が空で出てくる行があるので、そのぶんは先に並ぶ
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
            dgvJuchu.Columns(8).HeaderText = "単価"
            dgvJuchu.Columns(9).HeaderText = "通貨"
            dgvJuchu.Columns(10).HeaderText = "区分"

            dgvJuchu.Columns(0).Width = 95
            dgvJuchu.Columns(1).Width = 80
            dgvJuchu.Columns(2).Width = 80
            dgvJuchu.Columns(3).Width = 110
            dgvJuchu.Columns(4).Width = 180
            dgvJuchu.Columns(5).Width = 70
            dgvJuchu.Columns(6).Width = 70
            dgvJuchu.Columns(7).Width = 70
            dgvJuchu.Columns(8).Width = 80
            dgvJuchu.Columns(9).Width = 45
            dgvJuchu.Columns(10).Width = 40

            dgvJuchu.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvJuchu.Columns(8).DefaultCellStyle.Format = "#,##0.00"

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
            strSql = strSql & "SURYO, TANKA, KINGAKU, ISNULL(TSUKA, '') AS TSUKA, "
            strSql = strSql & "ISNULL(BIKO, '') AS BIKO "
            strSql = strSql & "FROM T_TH_NOHIN "
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
            dgvNohin.Columns(5).HeaderText = "通貨"
            dgvNohin.Columns(6).HeaderText = "備考"

            dgvNohin.Columns(0).Width = 80
            dgvNohin.Columns(1).Width = 90
            dgvNohin.Columns(2).Width = 70
            dgvNohin.Columns(3).Width = 80
            dgvNohin.Columns(4).Width = 100
            dgvNohin.Columns(5).Width = 45
            dgvNohin.Columns(6).Width = 300

            dgvNohin.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(2).DefaultCellStyle.Format = "#,##0"
            dgvNohin.Columns(3).DefaultCellStyle.Format = "#,##0.00"
            dgvNohin.Columns(4).DefaultCellStyle.Format = "#,##0.00"

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
        mstrTsuka = ToStr(r.Cells(9).Value)

        txtDenpyoNo.Text = mstrDenpyoNo
        txtHinban.Text = ToStr(r.Cells(3).Value)
        txtHinmei.Text = ToStr(r.Cells(4).Value)

        '実際にいつ出したかは向こうの納品書を見ていないので分からない(経理 田島さん)。
        '納期を入れておいて、違うなら打ち直してもらう。納期が空の行は今日にする
        If ToStr(r.Cells(2).Value) <> "" Then
            dtpNohinbi.Value = CDate(ToStr(r.Cells(2).Value))
        Else
            dtpNohinbi.Value = Now
        End If

        '残っている分をとりあえず入れておく。分けて出しているなら打ち直してもらう
        If Val(ToStr(r.Cells(7).Value)) > 0 Then
            txtSuryo.Text = ToStr(r.Cells(7).Value)
        Else
            txtSuryo.Text = ""
        End If

        '単価はCSVに入っているものを使う。向こうで決めている単価とのこと(営業 中村さん)
        txtTanka.Text = ToStr(r.Cells(8).Value)

        txtBiko.Text = ""

        If mstrTsuka <> "" And mstrTsuka <> "THB" Then
            lblChui.Text = "※この受注の通貨は " & mstrTsuka & " です"
        Else
            lblChui.Text = ""
        End If

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
    '※バーツは小数点以下2桁ある。丸めはしていない
    '------------------------------------------------------------
    Private Sub KingakuKeisan()

        Dim dblKingaku As Double

        If IsNumeric(txtSuryo.Text) = False Or IsNumeric(txtTanka.Text) = False Then
            txtKingaku.Text = ""
            Exit Sub
        End If

        dblKingaku = Val(txtSuryo.Text) * Val(txtTanka.Text)

        txtKingaku.Text = Format(dblKingaku, "0.00")

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

        '単価が0のときは打ち忘れのことがある
        If Val(txtTanka.Text) = 0 Then
            If MsgBox("単価が0です。このまま登録しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                txtTanka.Focus()
                Return False
            End If
        End If

        Return True

    End Function

    '------------------------------------------------------------
    '納品No採番
    '※日本の納品入力と同じやり方。表が別なので番号も別になる
    '------------------------------------------------------------
    Private Function Saiban(ByVal cn As SqlConnection) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object
        Dim lngMax As Long
        Dim strNo As String
        Dim i As Integer

        For i = 1 To 3

            strSql = "SELECT MAX(NOHIN_NO) FROM T_TH_NOHIN"

            cm = New SqlCommand(strSql, cn)
            objRet = cm.ExecuteScalar()

            If IsDBNull(objRet) Or objRet Is Nothing Then
                lngMax = 0
            Else
                lngMax = Val(CStr(objRet))
            End If

            strNo = Format(lngMax + 1, "000000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM T_TH_NOHIN WHERE NOHIN_NO = '" & strNo & "'"
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
        mstrTsuka = ""

        txtDenpyoNo.Text = ""
        txtHinban.Text = ""
        txtHinmei.Text = ""
        txtSuryo.Text = ""
        txtTanka.Text = ""
        txtKingaku.Text = ""
        txtBiko.Text = ""
        dtpNohinbi.Value = Now
        lblChui.Text = ""

        dgvNohin.DataSource = Nothing

        mblnLoad = False

    End Sub

#End Region

End Class
