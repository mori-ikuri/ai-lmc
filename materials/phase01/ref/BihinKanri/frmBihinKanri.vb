Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 備品管理
' 機能名     : 備品登録・照会
' 作成者     : 山田
'
' 履歴
' 2006/05/22 山田 新規作成
' 2006/08/30 山田 品名に'が入るとエラーになるため対応
' 2006/11/02 山田 分類コードを廃止(総務で使っていないため)
' 2007/03/12 山田 数量マイナス入力を禁止(総務 佐野さんより依頼)
' 2007/09/20 山田 備品コードがまれに重複するためリトライを入れた
' 2008/01/15 山田 廃棄済みを一覧に出さないようにした
'============================================================
Public Class frmBihinKanri

    'カレントの備品コード(空なら新規)
    Private mstrBihinCd As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Private mintBunruiCd As Integer     '2006/11/02 分類廃止のため未使用

#Region " 画面初期処理 "

    Private Sub frmBihinKanri_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gUserName = Environment.UserName

        Me.Text = "備品管理  [" & gUserName & "]"

        '保管場所コンボ
        '※総務の一覧に合わせてある。増えたらここに足すこと
        cboBasho.Items.Clear()
        cboBasho.Items.Add("本社1F")
        cboBasho.Items.Add("本社2F")
        cboBasho.Items.Add("本社3F")
        cboBasho.Items.Add("倉庫")
        cboBasho.Items.Add("工場")
        cboBasho.SelectedIndex = 0

        dtpShutokubi.Value = Now

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

            cn = GetConnection()

            If mstrBihinCd = "" Then

                '新規
                strCd = Saiban(cn)
                If strCd = "" Then
                    MsgBox("コードの採番に失敗しました。もう一度実行してください。", MsgBoxStyle.Exclamation)
                    mblnShori = False
                    Exit Sub
                End If

                strSql = ""
                strSql = strSql & "INSERT INTO M_BIHIN "
                strSql = strSql & "(BIHIN_CD, BIHIN_NM, KIKAKU, SURYO, BASHO, SHUTOKUBI, KINGAKU, BIKO, HAIKI_KBN, KOSHIN_USER, KOSHIN_DT) "
                strSql = strSql & "VALUES ("
                strSql = strSql & "'" & strCd & "',"
                strSql = strSql & "'" & EscQuote(Trim(txtBihinNm.Text)) & "',"
                strSql = strSql & "'" & EscQuote(Trim(txtKikaku.Text)) & "',"
                strSql = strSql & Val(txtSuryo.Text) & ","
                strSql = strSql & "'" & cboBasho.Text & "',"
                strSql = strSql & "'" & Format(dtpShutokubi.Value, "yyyy/MM/dd") & "',"
                strSql = strSql & Val(txtKingaku.Text) & ","
                strSql = strSql & "'" & EscQuote(Trim(txtBiko.Text)) & "',"
                strSql = strSql & "'0',"
                strSql = strSql & "'" & gUserName & "',"
                strSql = strSql & "GETDATE())"

                cm = New SqlCommand(strSql, cn)
                cm.ExecuteNonQuery()

                mstrBihinCd = strCd
                txtBihinCd.Text = strCd

                MsgBox("登録しました。備品コード = " & strCd, MsgBoxStyle.Information)

            Else

                '更新
                strSql = ""
                strSql = strSql & "UPDATE M_BIHIN SET "
                strSql = strSql & "BIHIN_NM = '" & EscQuote(Trim(txtBihinNm.Text)) & "',"
                strSql = strSql & "KIKAKU = '" & EscQuote(Trim(txtKikaku.Text)) & "',"
                strSql = strSql & "SURYO = " & Val(txtSuryo.Text) & ","
                strSql = strSql & "BASHO = '" & cboBasho.Text & "',"
                strSql = strSql & "SHUTOKUBI = '" & Format(dtpShutokubi.Value, "yyyy/MM/dd") & "',"
                strSql = strSql & "KINGAKU = " & Val(txtKingaku.Text) & ","
                strSql = strSql & "BIKO = '" & EscQuote(Trim(txtBiko.Text)) & "',"
                strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
                strSql = strSql & "KOSHIN_DT = GETDATE() "
                strSql = strSql & "WHERE BIHIN_CD = '" & mstrBihinCd & "'"

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
    '削除
    '2008/01/15 山田 実際には消さずに廃棄区分を立てる
    '                 過去の棚卸で使うため物理削除はしないこと
    '------------------------------------------------------------
    Private Sub btnSakujo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSakujo.Click

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim strSql As String

        If mstrBihinCd = "" Then
            MsgBox("一覧から選択してください。", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MsgBox("廃棄にします。よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "UPDATE M_BIHIN SET "
            strSql = strSql & "HAIKI_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE BIHIN_CD = '" & mstrBihinCd & "'"

            cm = New SqlCommand(strSql, cn)
            cm.ExecuteNonQuery()

            MsgBox("廃棄にしました。", MsgBoxStyle.Information)

            Call ClearNyuryoku()
            Call HyojiIchiran()

        Catch ex As Exception

            MsgBox("削除でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

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
            strSql = strSql & "SELECT BIHIN_CD, BIHIN_NM, KIKAKU, SURYO, BASHO, SHUTOKUBI, KINGAKU, BIKO "
            strSql = strSql & "FROM M_BIHIN "
            strSql = strSql & "WHERE HAIKI_KBN = '0' "

            If Trim(txtKensakuNm.Text) <> "" Then
                strSql = strSql & "AND BIHIN_NM LIKE '%" & EscQuote(Trim(txtKensakuNm.Text)) & "%' "
            End If

            If cboKensakuBasho.Text <> "" And cboKensakuBasho.Text <> "(すべて)" Then
                strSql = strSql & "AND BASHO = '" & cboKensakuBasho.Text & "' "
            End If

            strSql = strSql & "ORDER BY BIHIN_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "BIHIN")

            dgvIchiran.DataSource = ds.Tables("BIHIN")

            dgvIchiran.Columns(0).HeaderText = "コード"
            dgvIchiran.Columns(1).HeaderText = "品名"
            dgvIchiran.Columns(2).HeaderText = "規格"
            dgvIchiran.Columns(3).HeaderText = "数量"
            dgvIchiran.Columns(4).HeaderText = "保管場所"
            dgvIchiran.Columns(5).HeaderText = "取得日"
            dgvIchiran.Columns(6).HeaderText = "金額"
            dgvIchiran.Columns(7).HeaderText = "備考"

            dgvIchiran.Columns(0).Width = 80
            dgvIchiran.Columns(1).Width = 180
            dgvIchiran.Columns(2).Width = 120
            dgvIchiran.Columns(3).Width = 60
            dgvIchiran.Columns(4).Width = 80
            dgvIchiran.Columns(5).Width = 90
            dgvIchiran.Columns(6).Width = 90

            lblKensu.Text = "件数 " & CStr(ds.Tables("BIHIN").Rows.Count) & " 件"

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

        mstrBihinCd = ToStr(r.Cells(0).Value)

        txtBihinCd.Text = mstrBihinCd
        txtBihinNm.Text = ToStr(r.Cells(1).Value)
        txtKikaku.Text = ToStr(r.Cells(2).Value)
        txtSuryo.Text = ToStr(r.Cells(3).Value)
        cboBasho.Text = ToStr(r.Cells(4).Value)
        txtKingaku.Text = ToStr(r.Cells(6).Value)
        txtBiko.Text = ToStr(r.Cells(7).Value)

        If IsDate(ToStr(r.Cells(5).Value)) Then
            dtpShutokubi.Value = CDate(ToStr(r.Cells(5).Value))
        Else
            dtpShutokubi.Value = Now
        End If

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '入力チェック
    '------------------------------------------------------------
    Private Function Check() As Boolean

        If Trim(txtBihinNm.Text) = "" Then
            MsgBox("品名を入力してください。", MsgBoxStyle.Exclamation)
            txtBihinNm.Focus()
            Return False
        End If

        If Len(Trim(txtBihinNm.Text)) > 40 Then
            MsgBox("品名が長すぎます。", MsgBoxStyle.Exclamation)
            txtBihinNm.Focus()
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

        '2007/03/12 山田 マイナスが入って棚卸が合わなくなったため
        If Val(txtSuryo.Text) < 0 Then
            MsgBox("数量にマイナスは入力できません。", MsgBoxStyle.Exclamation)
            txtSuryo.Focus()
            Return False
        End If

        If Trim(txtKingaku.Text) <> "" Then
            If IsNumeric(txtKingaku.Text) = False Then
                MsgBox("金額が数字ではありません。", MsgBoxStyle.Exclamation)
                txtKingaku.Focus()
                Return False
            End If
        End If

        '2006/11/02 分類は廃止
        'If cboBunrui.SelectedIndex = -1 Then
        '    MsgBox("分類を選択してください。", MsgBoxStyle.Exclamation)
        '    Return False
        'End If

        Return True

    End Function

    '------------------------------------------------------------
    '備品コード採番
    '2007/09/20 山田 同時に登録するとまれに重複するのでリトライを入れた
    '------------------------------------------------------------
    Private Function Saiban(ByVal cn As SqlConnection) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object
        Dim lngMax As Long
        Dim strCd As String
        Dim i As Integer

        For i = 1 To 3

            strSql = "SELECT MAX(BIHIN_CD) FROM M_BIHIN"

            cm = New SqlCommand(strSql, cn)
            objRet = cm.ExecuteScalar()

            If IsDBNull(objRet) Or objRet Is Nothing Then
                lngMax = 0
            Else
                lngMax = Val(CStr(objRet))
            End If

            strCd = Format(lngMax + 1, "000000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM M_BIHIN WHERE BIHIN_CD = '" & strCd & "'"
            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) = 0 Then
                Return strCd
            End If

            System.Threading.Thread.Sleep(200)

        Next

        Return ""

    End Function

    '------------------------------------------------------------
    '入力欄クリア
    '------------------------------------------------------------
    Private Sub ClearNyuryoku()

        mstrBihinCd = ""

        txtBihinCd.Text = ""
        txtBihinNm.Text = ""
        txtKikaku.Text = ""
        txtSuryo.Text = "1"
        txtKingaku.Text = ""
        txtBiko.Text = ""
        cboBasho.SelectedIndex = 0
        dtpShutokubi.Value = Now

        txtBihinNm.Focus()

    End Sub

#End Region

End Class
