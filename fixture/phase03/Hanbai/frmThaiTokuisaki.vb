Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ得意先登録
' 作成者     : 山田
'
' 履歴
' 2011/11/08 山田 新規作成
'                 現地のCSVには得意先コードしか入っておらず会社名がどこにも無い。
'                 一覧は年明けまでもらえないとのことなので、こちらで名前を付ける
'============================================================
Public Class frmThaiTokuisaki

    'カレントの得意先コード(空なら新規)
    Private mstrTokuisakiCd As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmThaiTokuisaki_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "タイ得意先登録  [" & gUserName & "]"

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

            cn = GetConnection()

            If mstrTokuisakiCd = "" Then

                '新規 ※コードは現地のCSVに合わせて手で入れてもらう
                strSql = "SELECT COUNT(*) FROM M_TH_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(strCd) & "'"
                cm = New SqlCommand(strSql, cn)

                If Val(CStr(cm.ExecuteScalar())) > 0 Then
                    MsgBox("この得意先コードは既に登録されています。", MsgBoxStyle.Exclamation)
                    txtTokuisakiCd.Focus()
                    mblnShori = False
                    Exit Sub
                End If

                strSql = ""
                strSql = strSql & "INSERT INTO M_TH_TOKUISAKI "
                strSql = strSql & "(TOKUISAKI_CD, TOKUISAKI_NM, SHIYO_KBN, BIKO, KOSHIN_USER, KOSHIN_DT) "
                strSql = strSql & "VALUES ("
                strSql = strSql & "'" & EscQuote(strCd) & "',"
                strSql = strSql & "'" & EscQuote(Trim(txtTokuisakiNm.Text)) & "',"
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
                strSql = strSql & "UPDATE M_TH_TOKUISAKI SET "
                strSql = strSql & "TOKUISAKI_NM = '" & EscQuote(Trim(txtTokuisakiNm.Text)) & "',"
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
    '※取り込んだ受注が残るので、実際には消さずに区分を立てる
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
            strSql = strSql & "UPDATE M_TH_TOKUISAKI SET "
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
    '※まだ名前を付けていないコードが取込で入ってくるので、
    '  受注のほうにしか出てこないコードも一緒に並べて分かるようにしておく
    '------------------------------------------------------------
    Private Sub HyojiIchiran()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT M.TOKUISAKI_CD, M.TOKUISAKI_NM, "
            strSql = strSql & "CASE WHEN M.SHIYO_KBN = '1' THEN '使用しない' ELSE '' END AS SHIYO_NM, "
            strSql = strSql & "ISNULL(M.BIKO, '') AS BIKO, "
            strSql = strSql & "ISNULL((SELECT COUNT(*) FROM T_TH_JUCHU J WHERE J.TOKUISAKI_CD = M.TOKUISAKI_CD), 0) AS JUCHU_KENSU "
            strSql = strSql & "FROM M_TH_TOKUISAKI M "
            strSql = strSql & "WHERE 1 = 1 "

            If chkShiyoNashi.Checked = False Then
                strSql = strSql & "AND M.SHIYO_KBN = '0' "
            End If

            If Trim(txtKensaku.Text) <> "" Then
                strSql = strSql & "AND (M.TOKUISAKI_CD LIKE '%" & EscQuote(Trim(txtKensaku.Text)) & "%' "
                strSql = strSql & "OR M.TOKUISAKI_NM LIKE '%" & EscQuote(Trim(txtKensaku.Text)) & "%') "
            End If

            '受注には出てくるが、まだ名前を付けていないコード
            strSql = strSql & "UNION ALL "
            strSql = strSql & "SELECT DISTINCT J.TOKUISAKI_CD, '(名前未登録)', '', '', "
            strSql = strSql & "(SELECT COUNT(*) FROM T_TH_JUCHU K WHERE K.TOKUISAKI_CD = J.TOKUISAKI_CD) "
            strSql = strSql & "FROM T_TH_JUCHU J "
            strSql = strSql & "WHERE NOT EXISTS (SELECT * FROM M_TH_TOKUISAKI M2 WHERE M2.TOKUISAKI_CD = J.TOKUISAKI_CD) "

            If Trim(txtKensaku.Text) <> "" Then
                strSql = strSql & "AND J.TOKUISAKI_CD LIKE '%" & EscQuote(Trim(txtKensaku.Text)) & "%' "
            End If

            strSql = strSql & "ORDER BY 1"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "TOKUISAKI")

            dgvIchiran.DataSource = ds.Tables("TOKUISAKI")

            dgvIchiran.Columns(0).HeaderText = "コード"
            dgvIchiran.Columns(1).HeaderText = "得意先名"
            dgvIchiran.Columns(2).HeaderText = "使用"
            dgvIchiran.Columns(3).HeaderText = "備考"
            dgvIchiran.Columns(4).HeaderText = "受注件数"

            dgvIchiran.Columns(0).Width = 90
            dgvIchiran.Columns(1).Width = 240
            dgvIchiran.Columns(2).Width = 80
            dgvIchiran.Columns(3).Width = 250
            dgvIchiran.Columns(4).Width = 80
            dgvIchiran.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

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

        '名前を付けていない行は、新規のつもりでコードだけ入れておく
        If ToStr(r.Cells(1).Value) = "(名前未登録)" Then

            mstrTokuisakiCd = ""

            txtTokuisakiCd.Text = ToStr(r.Cells(0).Value)
            txtTokuisakiCd.ReadOnly = False
            txtTokuisakiNm.Text = ""
            txtBiko.Text = ""

            txtTokuisakiNm.Focus()

            Exit Sub

        End If

        mstrTokuisakiCd = ToStr(r.Cells(0).Value)

        txtTokuisakiCd.Text = mstrTokuisakiCd
        txtTokuisakiCd.ReadOnly = True
        txtTokuisakiNm.Text = ToStr(r.Cells(1).Value)
        txtBiko.Text = ToStr(r.Cells(3).Value)

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
        txtBiko.Text = ""

        txtTokuisakiCd.Focus()

    End Sub

#End Region

End Class
