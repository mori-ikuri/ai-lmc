Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 得意先登録
' 作成者     : 山田
'
' 履歴
' 2008/04/28 山田 新規作成
' 2008/05/19 山田 請求先を追加(得意先コードで持つ)
' 2008/06/02 山田 使用しない得意先を一覧に出さないようにした
'============================================================
Public Class frmTokuisaki

    'カレントの得意先コード(空なら新規)
    Private mstrTokuisakiCd As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmTokuisaki_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "得意先登録  [" & gUserName & "]"

        '締日
        '※経理のExcelに出てくるのがこの3つ。増えたらここに足すこと
        cboShimebi.Items.Clear()
        cboShimebi.Items.Add("末日")
        cboShimebi.Items.Add("20日")
        cboShimebi.Items.Add("25日")

        '丸め
        cboMarume.Items.Clear()
        cboMarume.Items.Add("切り捨て")
        cboMarume.Items.Add("四捨五入")

        Call ClearNyuryoku()
        Call HyojiIchiran()

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
    '登録(新規・更新兼用)
    '------------------------------------------------------------
    Private Sub btnToroku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToroku.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strCd As String
        Dim intShimebi As Integer
        Dim strMarume As String

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            strCd = Trim(txtTokuisakiCd.Text)

            '締日 ※末日は31で持つ
            If cboShimebi.Text = "末日" Then
                intShimebi = 31
            Else
                intShimebi = Val(cboShimebi.Text)
            End If

            If cboMarume.Text = "四捨五入" Then
                strMarume = "1"
            Else
                strMarume = "0"
            End If

            cn = GetConnection()

            '請求先が得意先に登録されているかみる
            If Trim(txtSeikyusakiCd.Text) <> "" Then

                strSql = "SELECT COUNT(*) FROM M_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(Trim(txtSeikyusakiCd.Text)) & "'"
                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) = 0 Then
                    MsgBox("請求先コードが得意先に登録されていません。", MsgBoxStyle.Exclamation)
                    txtSeikyusakiCd.Focus()
                    mblnShori = False
                    Exit Sub
                End If

            End If

            If mstrTokuisakiCd = "" Then

                '新規 ※コードは手で入れてもらうので、既にあるかみる
                strSql = "SELECT COUNT(*) FROM M_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(strCd) & "'"
                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) > 0 Then
                    MsgBox("この得意先コードは既に登録されています。", MsgBoxStyle.Exclamation)
                    txtTokuisakiCd.Focus()
                    mblnShori = False
                    Exit Sub
                End If

                strSql = ""
                strSql = strSql & "INSERT INTO M_TOKUISAKI "
                strSql = strSql & "(TOKUISAKI_CD, TOKUISAKI_NM, SEIKYUSAKI_CD, SHIMEBI, SHIHARAI, MARUME_KBN, SHIYO_KBN, BIKO, KOSHIN_USER, KOSHIN_DT) "
                strSql = strSql & "VALUES ("
                strSql = strSql & "'" & EscQuote(strCd) & "',"
                strSql = strSql & "'" & EscQuote(Trim(txtTokuisakiNm.Text)) & "',"
                If Trim(txtSeikyusakiCd.Text) = "" Then
                    strSql = strSql & "NULL,"
                Else
                    strSql = strSql & "'" & EscQuote(Trim(txtSeikyusakiCd.Text)) & "',"
                End If
                strSql = strSql & CStr(intShimebi) & ","
                strSql = strSql & "'" & EscQuote(Trim(txtShiharai.Text)) & "',"
                strSql = strSql & "'" & strMarume & "',"
                strSql = strSql & "'0',"
                strSql = strSql & "'" & EscQuote(Trim(txtBiko.Text)) & "',"
                strSql = strSql & "'" & gUserName & "',"
                strSql = strSql & "GETDATE())"

                cm = New SqlCommand(strSql, cn)
                cm.ExecuteNonQuery()

                mstrTokuisakiCd = strCd
                txtTokuisakiCd.ReadOnly = True

                MsgBox("登録しました。", MsgBoxStyle.Information)

            Else

                '更新
                strSql = ""
                strSql = strSql & "UPDATE M_TOKUISAKI SET "
                strSql = strSql & "TOKUISAKI_NM = '" & EscQuote(Trim(txtTokuisakiNm.Text)) & "',"
                If Trim(txtSeikyusakiCd.Text) = "" Then
                    strSql = strSql & "SEIKYUSAKI_CD = NULL,"
                Else
                    strSql = strSql & "SEIKYUSAKI_CD = '" & EscQuote(Trim(txtSeikyusakiCd.Text)) & "',"
                End If
                strSql = strSql & "SHIMEBI = " & CStr(intShimebi) & ","
                strSql = strSql & "SHIHARAI = '" & EscQuote(Trim(txtShiharai.Text)) & "',"
                strSql = strSql & "MARUME_KBN = '" & strMarume & "',"
                strSql = strSql & "BIKO = '" & EscQuote(Trim(txtBiko.Text)) & "',"
                strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
                strSql = strSql & "KOSHIN_DT = GETDATE() "
                strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(mstrTokuisakiCd) & "'"

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
    '2008/06/02 山田 受注も納品も残るので、実際には消さずに区分を立てる
    '------------------------------------------------------------
    Private Sub btnShiyoNashi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShiyoNashi.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        If mstrTokuisakiCd = "" Then
            MsgBox("一覧から選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MsgBox("使用しないにします。よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "UPDATE M_TOKUISAKI SET "
            strSql = strSql & "SHIYO_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE TOKUISAKI_CD = '" & EscQuote(mstrTokuisakiCd) & "'"

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

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT T.TOKUISAKI_CD, T.TOKUISAKI_NM, T.SEIKYUSAKI_CD, S.TOKUISAKI_NM AS SEIKYUSAKI_NM, "
            strSql = strSql & "CASE WHEN T.SHIMEBI = 31 THEN '末日' ELSE CAST(T.SHIMEBI AS varchar(2)) + '日' END AS SHIMEBI_NM, "
            strSql = strSql & "T.SHIHARAI, "
            strSql = strSql & "CASE WHEN T.MARUME_KBN = '1' THEN '四捨五入' ELSE '切り捨て' END AS MARUME_NM, "
            strSql = strSql & "CASE WHEN T.SHIYO_KBN = '1' THEN '使用しない' ELSE '' END AS SHIYO_NM, "
            strSql = strSql & "T.BIKO "
            strSql = strSql & "FROM M_TOKUISAKI T "
            strSql = strSql & "LEFT JOIN M_TOKUISAKI S ON S.TOKUISAKI_CD = T.SEIKYUSAKI_CD "
            strSql = strSql & "WHERE 1 = 1 "

            '2008/06/02 山田 ふだんは使っている得意先だけ出す
            If chkShiyoNashi.Checked = False Then
                strSql = strSql & "AND T.SHIYO_KBN = '0' "
            End If

            If Trim(txtKensakuNm.Text) <> "" Then
                strSql = strSql & "AND T.TOKUISAKI_NM LIKE '%" & EscQuote(Trim(txtKensakuNm.Text)) & "%' "
            End If

            strSql = strSql & "ORDER BY T.TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TOKUISAKI")

            dgvIchiran.DataSource = ds.Tables("TOKUISAKI")

            dgvIchiran.Columns(0).HeaderText = "コード"
            dgvIchiran.Columns(1).HeaderText = "得意先名"
            dgvIchiran.Columns(2).HeaderText = "請求先"
            dgvIchiran.Columns(3).HeaderText = "請求先名"
            dgvIchiran.Columns(4).HeaderText = "締日"
            dgvIchiran.Columns(5).HeaderText = "支払条件"
            dgvIchiran.Columns(6).HeaderText = "丸め"
            dgvIchiran.Columns(7).HeaderText = "使用"
            dgvIchiran.Columns(8).HeaderText = "備考"

            dgvIchiran.Columns(0).Width = 60
            dgvIchiran.Columns(1).Width = 160
            dgvIchiran.Columns(2).Width = 60
            dgvIchiran.Columns(3).Width = 130
            dgvIchiran.Columns(4).Width = 50
            dgvIchiran.Columns(5).Width = 110
            dgvIchiran.Columns(6).Width = 60
            dgvIchiran.Columns(7).Width = 70

            lblKensu.Text = "件数 " & CStr(ds.Tables("TOKUISAKI").Rows.Count) & " 件"

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

        mstrTokuisakiCd = ToStr(r.Cells(0).Value)

        txtTokuisakiCd.Text = mstrTokuisakiCd
        txtTokuisakiCd.ReadOnly = True
        txtTokuisakiNm.Text = ToStr(r.Cells(1).Value)
        txtSeikyusakiCd.Text = ToStr(r.Cells(2).Value)
        cboShimebi.Text = ToStr(r.Cells(4).Value)
        txtShiharai.Text = ToStr(r.Cells(5).Value)
        cboMarume.Text = ToStr(r.Cells(6).Value)
        txtBiko.Text = ToStr(r.Cells(8).Value)

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If Trim(txtTokuisakiCd.Text) = "" Then
            MsgBox("得意先コードを入力してください。", MsgBoxStyle.Exclamation)
            txtTokuisakiCd.Focus()
            Return False
        End If

        If Trim(txtTokuisakiNm.Text) = "" Then
            MsgBox("得意先名を入力してください。", MsgBoxStyle.Exclamation)
            txtTokuisakiNm.Focus()
            Return False
        End If

        If Len(Trim(txtTokuisakiNm.Text)) > 40 Then
            MsgBox("得意先名が長すぎます。", MsgBoxStyle.Exclamation)
            txtTokuisakiNm.Focus()
            Return False
        End If

        '請求先を自分にすると請求のときに堂々巡りになるので空にしてもらう
        If Trim(txtSeikyusakiCd.Text) <> "" And Trim(txtSeikyusakiCd.Text) = Trim(txtTokuisakiCd.Text) Then
            MsgBox("請求先が自分自身になっています。自分あてなら空欄にしてください。", MsgBoxStyle.Exclamation)
            txtSeikyusakiCd.Focus()
            Return False
        End If

        If cboShimebi.Text = "" Then
            MsgBox("締日を選択してください。", MsgBoxStyle.Exclamation)
            cboShimebi.Focus()
            Return False
        End If

        If cboMarume.Text = "" Then
            MsgBox("丸めを選択してください。", MsgBoxStyle.Exclamation)
            cboMarume.Focus()
            Return False
        End If

        Return True

    End Function

    '------------------------------------------------------------
    '入力欄クリア
    '------------------------------------------------------------
    Private Sub ClearNyuryoku()

        mstrTokuisakiCd = ""

        txtTokuisakiCd.Text = ""
        txtTokuisakiCd.ReadOnly = False
        txtTokuisakiNm.Text = ""
        txtSeikyusakiCd.Text = ""
        cboShimebi.Text = "末日"
        txtShiharai.Text = ""
        cboMarume.Text = "切り捨て"
        txtBiko.Text = ""

        txtTokuisakiCd.Focus()

    End Sub

#End Region

End Class
