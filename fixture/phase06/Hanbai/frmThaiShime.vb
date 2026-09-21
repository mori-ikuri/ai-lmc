Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : タイ締め
' 作成者     : 山田
'
' 履歴
' 2013/05/13 山田 新規作成
'                 タイの納品を月末で締めて、月ごとの合計を固める画面。
'                 タイ納品一覧は見るたびに集計しなおすので、遅れて入った分や
'                 打ち間違いを直した分で数字が動く。締めたらこの表の数字を出す。
'                 請求書は向こうで出しているので作らない。入金も無い。
'                 日本の締め処理(frmShime)は毎月動いているので触らない。
'                 現地の締めは月末とのこと(営業 中村さん)なので、
'                 得意先ごとではなく月ごとに全部まとめて締める
'============================================================
Public Class frmThaiShime

    '選択中の得意先
    Private mstrTokuisakiCd As String = ""

    '選択中の行の締めNo(空なら未締め)
    Private mstrShimeNo As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中は月を変えても出しなおさない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmThaiShime_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer

        Me.Text = "タイ締め  [" & gUserName & "]"

        mblnLoad = True

        cboTsuki.Items.Clear()
        For i = 1 To 12
            cboTsuki.Items.Add(CStr(i))
        Next

        '前の月を締めることが多いので前月を出しておく
        txtNen.Text = CStr(Year(DateAdd("m", -1, Now)))
        cboTsuki.Text = CStr(Month(DateAdd("m", -1, Now)))

        mblnLoad = False

        Call HyojiTaisho()

    End Sub

#End Region

#Region " ボタン処理 "

    '------------------------------------------------------------
    '対象を出す
    '------------------------------------------------------------
    Private Sub btnKensaku_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKensaku.Click

        Call HyojiTaisho()

    End Sub

    '------------------------------------------------------------
    '年月を変えたら出しなおす
    '------------------------------------------------------------
    Private Sub cboTsuki_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTsuki.SelectedIndexChanged

        If mblnLoad = True Then
            Exit Sub
        End If

        Call HyojiTaisho()

    End Sub

    '------------------------------------------------------------
    '締める
    '※月の全得意先をまとめて締める。得意先ごとに1件できる
    '------------------------------------------------------------
    Private Sub btnShime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShime.Click

        Dim cn As SqlConnection = Nothing
        Dim tr As SqlTransaction = Nothing
        Dim cm As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim strYm As String
        Dim strCd As String
        Dim strShimeNo As String
        Dim datKaishibi As Date
        Dim datShimebi As Date
        Dim lngKensu As Long
        Dim dblGokei As Double
        Dim dblKingaku As Double
        Dim i As Integer

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

            datKaishibi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text), 1)
            datShimebi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text) + 1, 0)

            cn = GetConnection()

            '同じ月をもう一度締めていないか、ここでもう一度みる
            If ShimeZumi(cn, strYm) = True Then
                MsgBox(strYm & " は既に締められています。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            '月が終わる前に締めると、月末に入れる分が入らない
            If datShimebi >= DateSerial(Year(Now), Month(Now), Microsoft.VisualBasic.Day(Now)) Then

                If MsgBox(Format(datKaishibi, "yyyy年M月") & " はまだ終わっていません。" & vbCrLf & _
                          "橋本さんの入力が終わっているか確かめてください。" & vbCrLf & vbCrLf & _
                          "このまま締めますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            '対象の納品を得意先ごとにまとめる ※締め済みの分は締めNoが入っているので拾わない
            strSql = ""
            strSql = strSql & "SELECT N.TOKUISAKI_CD, COUNT(*) AS KENSU, SUM(N.SURYO) AS SURYO, SUM(N.KINGAKU) AS GOKEI "
            strSql = strSql & "FROM T_TH_NOHIN N "
            strSql = strSql & TaishoJoken(datKaishibi, datShimebi)
            strSql = strSql & "GROUP BY N.TOKUISAKI_CD "
            strSql = strSql & "ORDER BY N.TOKUISAKI_CD"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "N")

            If ds.Tables("N").Rows.Count = 0 Then
                MsgBox(Format(datKaishibi, "yyyy/MM/dd") & " ～ " & Format(datShimebi, "yyyy/MM/dd") & " に納品がありません。" & vbCrLf & _
                       "締めるものがありません。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            '丸めは1行ずつではなく、得意先ごとの合計に1回だけ掛ける。日本と同じ
            lngKensu = 0
            dblGokei = 0

            For i = 0 To ds.Tables("N").Rows.Count - 1
                lngKensu = lngKensu + Val(ToStr(ds.Tables("N").Rows(i)("KENSU")))
                dblGokei = dblGokei + Marume(Val(ToStr(ds.Tables("N").Rows(i)("GOKEI"))), "1")
            Next

            If MsgBox(Format(datKaishibi, "yyyy年M月") & " のタイ納品を締めます。" & vbCrLf & _
                      "期間 " & Format(datKaishibi, "yyyy/MM/dd") & " ～ " & Format(datShimebi, "yyyy/MM/dd") & vbCrLf & _
                      "得意先 " & CStr(ds.Tables("N").Rows.Count) & " 社   納品 " & CStr(lngKensu) & " 件   合計 " & Format(dblGokei, "#,##0") & " バーツ" & vbCrLf & vbCrLf & _
                      "締めた後は、この月の納品は入れられなくなります。" & vbCrLf & _
                      "締めてよろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                mblnShori = False
                Exit Sub
            End If

            '締めを作るのと納品への書き込みは、片方だけ通ると数字が合わなくなるのでまとめて行う
            tr = cn.BeginTransaction()

            For i = 0 To ds.Tables("N").Rows.Count - 1

                strCd = ToStr(ds.Tables("N").Rows(i)("TOKUISAKI_CD"))
                dblKingaku = Marume(Val(ToStr(ds.Tables("N").Rows(i)("GOKEI"))), "1")

                strShimeNo = Saiban(cn, tr)

                If strShimeNo = "" Then
                    tr.Rollback()
                    tr = Nothing
                    MsgBox("締めNoの採番に失敗しました。もう一度実行してください。", MsgBoxStyle.Exclamation)
                    mblnShori = False
                    Exit Sub
                End If

                strSql = ""
                strSql = strSql & "INSERT INTO T_TH_SHIME "
                strSql = strSql & "(SHIME_NO, SHIME_YM, TOKUISAKI_CD, KAISHIBI, SHIMEBI, KENSU, SURYO, KINGAKU, "
                strSql = strSql & "TORIKESHI_KBN, KOSHIN_USER, KOSHIN_DT) "
                strSql = strSql & "VALUES ("
                strSql = strSql & "'" & strShimeNo & "',"
                strSql = strSql & "'" & strYm & "',"
                strSql = strSql & "'" & EscQuote(strCd) & "',"
                strSql = strSql & "'" & Format(datKaishibi, "yyyy/MM/dd") & "',"
                strSql = strSql & "'" & Format(datShimebi, "yyyy/MM/dd") & "',"
                strSql = strSql & CStr(Val(ToStr(ds.Tables("N").Rows(i)("KENSU")))) & ","
                strSql = strSql & CStr(Val(ToStr(ds.Tables("N").Rows(i)("SURYO")))) & ","
                strSql = strSql & CStr(dblKingaku) & ","
                strSql = strSql & "'0',"
                strSql = strSql & "'" & gUserName & "',"
                strSql = strSql & "GETDATE())"

                cm = New SqlCommand(strSql, cn, tr)
                cm.ExecuteNonQuery()

                '締めた納品に締めNoを書く。これが入っている納品はもう拾わないし、取り消せない
                strSql = ""
                strSql = strSql & "UPDATE N SET N.SHIME_NO = '" & strShimeNo & "',"
                strSql = strSql & "N.KOSHIN_USER = '" & gUserName & "',"
                strSql = strSql & "N.KOSHIN_DT = GETDATE() "
                strSql = strSql & "FROM T_TH_NOHIN N "
                strSql = strSql & TaishoJoken(datKaishibi, datShimebi)
                strSql = strSql & "AND N.TOKUISAKI_CD = '" & EscQuote(strCd) & "'"

                cm = New SqlCommand(strSql, cn, tr)
                cm.ExecuteNonQuery()

            Next

            tr.Commit()
            tr = Nothing

            MsgBox("締めました。" & vbCrLf & Format(datKaishibi, "yyyy年M月") & " の合計 " & Format(dblGokei, "#,##0") & " バーツ", MsgBoxStyle.Information)

            Call HyojiTaisho()

        Catch ex As Exception

            If Not tr Is Nothing Then
                tr.Rollback()
                tr = Nothing
            End If

            MsgBox("締めでエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If
            mblnShori = False

        End Try

    End Sub

    '------------------------------------------------------------
    '締め取消
    '※締めたあとに納品を直したいときはこれで戻してもらう。日本と同じ運用(経理 田島さん)。
    '  月ごとにまとめて締めているので、取消も月ごと。得意先1社だけは取り消せない
    '------------------------------------------------------------
    Private Sub btnTorikeshi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTorikeshi.Click

        Dim cn As SqlConnection = Nothing
        Dim tr As SqlTransaction = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strYm As String

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

            cn = GetConnection()

            If ShimeZumi(cn, strYm) = False Then
                MsgBox(strYm & " はまだ締めていません。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            If MsgBox(Mid(strYm, 1, 4) & "年" & CStr(Val(Mid(strYm, 5, 2))) & "月 のタイ締めを取り消します。" & vbCrLf & _
                      "この月の全得意先の締めが戻り、締め直すまで数字が動くようになります。" & vbCrLf & _
                      "社長に報告した後なら、直したあとに必ず締め直してください。" & vbCrLf & vbCrLf & _
                      "取り消しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                mblnShori = False
                Exit Sub
            End If

            tr = cn.BeginTransaction()

            '納品を未締めに戻す
            strSql = ""
            strSql = strSql & "UPDATE T_TH_NOHIN SET SHIME_NO = NULL,"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE SHIME_NO IN (SELECT SHIME_NO FROM T_TH_SHIME "
            strSql = strSql & "WHERE SHIME_YM = '" & strYm & "' AND TORIKESHI_KBN = '0')"

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            '締めは消さずに取消区分を立てる
            strSql = ""
            strSql = strSql & "UPDATE T_TH_SHIME SET TORIKESHI_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE SHIME_YM = '" & strYm & "' AND TORIKESHI_KBN = '0'"

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            tr.Commit()
            tr = Nothing

            MsgBox("締めを取り消しました。納品を直したら、もう一度締めてください。", MsgBoxStyle.Information)

            Call HyojiTaisho()

        Catch ex As Exception

            If Not tr Is Nothing Then
                tr.Rollback()
                tr = Nothing
            End If

            MsgBox("締め取消でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

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

#End Region

#Region " 一覧 "

    '------------------------------------------------------------
    'その月の得意先ごとの合計を出す
    '※締め済みなら締めに入れた数字を、未締めなら今の納品を集計して出す
    '------------------------------------------------------------
    Private Sub HyojiTaisho()

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim dt As DataTable
        Dim dr As DataRow
        Dim strSql As String
        Dim strYm As String
        Dim strShimeDt As String
        Dim datKaishibi As Date
        Dim datShimebi As Date
        Dim lngKensu As Long
        Dim lngSuryo As Long
        Dim dblGokei As Double
        Dim dblKingaku As Double
        Dim intFurui As Integer
        Dim i As Integer

        mstrTokuisakiCd = ""
        mstrShimeNo = ""
        dgvNohin.DataSource = Nothing
        lblChui.Text = ""
        lblJotai.Text = ""

        If Check() = False Then
            Exit Sub
        End If

        strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

        datKaishibi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text), 1)
        datShimebi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text) + 1, 0)

        Try

            cn = GetConnection()

            dt = New DataTable
            dt.Columns.Add("TOKUISAKI_CD")
            dt.Columns.Add("TOKUISAKI_NM")
            dt.Columns.Add("KENSU", GetType(Integer))
            dt.Columns.Add("SURYO", GetType(Long))
            dt.Columns.Add("GOKEI", GetType(Double))
            dt.Columns.Add("KINGAKU", GetType(Double))
            dt.Columns.Add("SHIME_NO")

            If ShimeZumi(cn, strYm) = True Then

                '締め済みは締めに入れた数字をそのまま出す
                strSql = ""
                strSql = strSql & "SELECT S.SHIME_NO, S.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') AS TOKUISAKI_NM, "
                strSql = strSql & "S.KENSU, S.SURYO, S.KINGAKU, "
                strSql = strSql & "ISNULL((SELECT SUM(N.KINGAKU) FROM T_TH_NOHIN N WHERE N.SHIME_NO = S.SHIME_NO), 0) AS GOKEI, "
                strSql = strSql & "CONVERT(varchar(16), S.KOSHIN_DT, 120) AS SHIME_DT "
                strSql = strSql & "FROM T_TH_SHIME S "
                strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = S.TOKUISAKI_CD "
                strSql = strSql & "WHERE S.SHIME_YM = '" & strYm & "' AND S.TORIKESHI_KBN = '0' "
                strSql = strSql & "ORDER BY S.TOKUISAKI_CD"

                ds = New DataSet
                da = New SqlDataAdapter(strSql, cn)
                da.Fill(ds, "S")

                strShimeDt = ""

                For i = 0 To ds.Tables("S").Rows.Count - 1

                    dr = dt.NewRow()
                    dr("TOKUISAKI_CD") = ToStr(ds.Tables("S").Rows(i)("TOKUISAKI_CD"))
                    dr("TOKUISAKI_NM") = ToStr(ds.Tables("S").Rows(i)("TOKUISAKI_NM"))
                    dr("KENSU") = Val(ToStr(ds.Tables("S").Rows(i)("KENSU")))
                    dr("SURYO") = Val(ToStr(ds.Tables("S").Rows(i)("SURYO")))
                    dr("GOKEI") = Val(ToStr(ds.Tables("S").Rows(i)("GOKEI")))
                    dr("KINGAKU") = Val(ToStr(ds.Tables("S").Rows(i)("KINGAKU")))
                    dr("SHIME_NO") = ToStr(ds.Tables("S").Rows(i)("SHIME_NO"))
                    dt.Rows.Add(dr)

                    strShimeDt = ToStr(ds.Tables("S").Rows(i)("SHIME_DT"))

                Next

                lblJotai.Text = "締め済み  (" & strShimeDt & " 締め)  この月の数字はもう動きません"
                lblJotai.ForeColor = System.Drawing.Color.Blue

            Else

                strSql = ""
                strSql = strSql & "SELECT N.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') AS TOKUISAKI_NM, "
                strSql = strSql & "COUNT(*) AS KENSU, SUM(N.SURYO) AS SURYO, SUM(N.KINGAKU) AS GOKEI "
                strSql = strSql & "FROM T_TH_NOHIN N "
                strSql = strSql & "LEFT JOIN M_TH_TOKUISAKI M ON M.TOKUISAKI_CD = N.TOKUISAKI_CD "
                strSql = strSql & TaishoJoken(datKaishibi, datShimebi)
                strSql = strSql & "GROUP BY N.TOKUISAKI_CD, ISNULL(M.TOKUISAKI_NM, '') "
                strSql = strSql & "ORDER BY N.TOKUISAKI_CD"

                ds = New DataSet
                da = New SqlDataAdapter(strSql, cn)
                da.Fill(ds, "N")

                For i = 0 To ds.Tables("N").Rows.Count - 1

                    dr = dt.NewRow()
                    dr("TOKUISAKI_CD") = ToStr(ds.Tables("N").Rows(i)("TOKUISAKI_CD"))
                    dr("TOKUISAKI_NM") = ToStr(ds.Tables("N").Rows(i)("TOKUISAKI_NM"))
                    dr("KENSU") = Val(ToStr(ds.Tables("N").Rows(i)("KENSU")))
                    dr("SURYO") = Val(ToStr(ds.Tables("N").Rows(i)("SURYO")))
                    dr("GOKEI") = Val(ToStr(ds.Tables("N").Rows(i)("GOKEI")))
                    '丸めは得意先ごとの合計に1回だけ掛ける
                    dr("KINGAKU") = Marume(Val(ToStr(ds.Tables("N").Rows(i)("GOKEI"))), "1")
                    dr("SHIME_NO") = ""
                    dt.Rows.Add(dr)

                Next

                lblJotai.Text = "未締め  今の納品を集計しています。締めるまで数字は動きます"
                lblJotai.ForeColor = System.Drawing.Color.Red

                '締めの期間より前に未締めの納品が残っていないかみる
                strSql = ""
                strSql = strSql & "SELECT COUNT(*) FROM T_TH_NOHIN N "
                strSql = strSql & "WHERE N.TORIKESHI_KBN = '0' "
                strSql = strSql & "AND N.SHIME_NO IS NULL "
                strSql = strSql & "AND N.NOHINBI < '" & Format(datKaishibi, "yyyy/MM/dd") & "'"

                cm = New SqlCommand(strSql, cn)
                intFurui = Val(CStr(cm.ExecuteScalar()))

                If intFurui > 0 Then
                    lblChui.Text = "※この月より前に、まだ締めていない納品が " & CStr(intFurui) & " 件あります"
                End If

            End If

            dgvTaisho.DataSource = dt

            dgvTaisho.Columns(0).HeaderText = "得意先"
            dgvTaisho.Columns(1).HeaderText = "得意先名"
            dgvTaisho.Columns(2).HeaderText = "件数"
            dgvTaisho.Columns(3).HeaderText = "数量"
            dgvTaisho.Columns(4).HeaderText = "金額計(小数のまま)"
            dgvTaisho.Columns(5).HeaderText = "締め金額(バーツ)"
            dgvTaisho.Columns(6).HeaderText = "締めNo"

            dgvTaisho.Columns(0).Width = 90
            dgvTaisho.Columns(1).Width = 240
            dgvTaisho.Columns(2).Width = 60
            dgvTaisho.Columns(3).Width = 90
            dgvTaisho.Columns(4).Width = 140
            dgvTaisho.Columns(5).Width = 130
            dgvTaisho.Columns(6).Width = 70

            dgvTaisho.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvTaisho.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvTaisho.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvTaisho.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvTaisho.Columns(3).DefaultCellStyle.Format = "#,##0"
            dgvTaisho.Columns(4).DefaultCellStyle.Format = "#,##0.00"
            dgvTaisho.Columns(5).DefaultCellStyle.Format = "#,##0"

            '月の合計 ※通貨は混ぜずにそのまま足している。今のところバーツしか来ていない
            lngKensu = 0
            lngSuryo = 0
            dblGokei = 0
            dblKingaku = 0

            For i = 0 To dt.Rows.Count - 1
                lngKensu = lngKensu + Val(ToStr(dt.Rows(i)("KENSU")))
                lngSuryo = lngSuryo + Val(ToStr(dt.Rows(i)("SURYO")))
                dblGokei = dblGokei + Val(ToStr(dt.Rows(i)("GOKEI")))
                dblKingaku = dblKingaku + Val(ToStr(dt.Rows(i)("KINGAKU")))
            Next

            lblKensu.Text = "得意先 " & CStr(dt.Rows.Count) & " 社   " & _
                            "納品 " & CStr(lngKensu) & " 件   " & _
                            "数量計 " & Format(lngSuryo, "#,##0") & "   " & _
                            "金額計 " & Format(dblGokei, "#,##0.00")

            lblGokei.Text = Format(datKaishibi, "yyyy年M月") & " の合計   " & Format(dblKingaku, "#,##0") & " バーツ  ※CSVの通貨のまま"

        Catch ex As Exception

            MsgBox("締め対象の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '一覧クリックで、その得意先の納品を下に出す
    '------------------------------------------------------------
    Private Sub dgvTaisho_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTaisho.CellClick

        Dim r As DataGridViewRow

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        r = dgvTaisho.Rows(e.RowIndex)

        mstrTokuisakiCd = ToStr(r.Cells(0).Value)
        mstrShimeNo = ToStr(r.Cells(6).Value)

        Call HyojiNohin()

    End Sub

    '------------------------------------------------------------
    '選んだ得意先の対象納品
    '------------------------------------------------------------
    Private Sub HyojiNohin()

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim datKaishibi As Date
        Dim datShimebi As Date

        If mstrTokuisakiCd = "" Then
            dgvNohin.DataSource = Nothing
            Exit Sub
        End If

        datKaishibi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text), 1)
        datShimebi = DateSerial(Val(txtNen.Text), Val(cboTsuki.Text) + 1, 0)

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT CONVERT(varchar(10), N.NOHINBI, 111) AS NOHINBI, "
            strSql = strSql & "N.NOHIN_NO, N.DENPYO_NO, ISNULL(N.HINBAN, '') AS HINBAN, ISNULL(N.HINMEI, '') AS HINMEI, "
            strSql = strSql & "N.SURYO, N.TANKA, N.KINGAKU, ISNULL(N.TSUKA, '') AS TSUKA, ISNULL(N.BIKO, '') AS BIKO "
            strSql = strSql & "FROM T_TH_NOHIN N "

            If mstrShimeNo = "" Then
                strSql = strSql & TaishoJoken(datKaishibi, datShimebi)
                strSql = strSql & "AND N.TOKUISAKI_CD = '" & EscQuote(mstrTokuisakiCd) & "' "
            Else
                strSql = strSql & "WHERE N.SHIME_NO = '" & EscQuote(mstrShimeNo) & "' "
            End If

            strSql = strSql & "ORDER BY N.NOHINBI, N.NOHIN_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NOHIN")

            dgvNohin.DataSource = ds.Tables("NOHIN")

            dgvNohin.Columns(0).HeaderText = "納品日"
            dgvNohin.Columns(1).HeaderText = "納品No"
            dgvNohin.Columns(2).HeaderText = "伝票番号"
            dgvNohin.Columns(3).HeaderText = "品番"
            dgvNohin.Columns(4).HeaderText = "品名"
            dgvNohin.Columns(5).HeaderText = "数量"
            dgvNohin.Columns(6).HeaderText = "単価"
            dgvNohin.Columns(7).HeaderText = "金額"
            dgvNohin.Columns(8).HeaderText = "通貨"
            dgvNohin.Columns(9).HeaderText = "備考"

            dgvNohin.Columns(0).Width = 80
            dgvNohin.Columns(1).Width = 70
            dgvNohin.Columns(2).Width = 95
            dgvNohin.Columns(3).Width = 100
            dgvNohin.Columns(4).Width = 150
            dgvNohin.Columns(5).Width = 60
            dgvNohin.Columns(6).Width = 75
            dgvNohin.Columns(7).Width = 95
            dgvNohin.Columns(8).Width = 45
            dgvNohin.Columns(9).Width = 130

            dgvNohin.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(5).DefaultCellStyle.Format = "#,##0"
            dgvNohin.Columns(6).DefaultCellStyle.Format = "#,##0.00"
            dgvNohin.Columns(7).DefaultCellStyle.Format = "#,##0.00"

        Catch ex As Exception

            MsgBox("納品の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

#End Region

#Region " 内部処理 "

    '------------------------------------------------------------
    '締め対象の納品を絞る条件
    '※一覧を出すときと締めるときで条件が違うと数字が合わなくなるので、ここにまとめてある。
    '  締めの対象は納品日(経理 田島さん)
    '------------------------------------------------------------
    Private Function TaishoJoken(ByVal datKaishibi As Date, ByVal datShimebi As Date) As String

        Dim strSql As String

        strSql = ""
        strSql = strSql & "WHERE N.TORIKESHI_KBN = '0' "
        strSql = strSql & "AND N.SHIME_NO IS NULL "
        strSql = strSql & "AND N.NOHINBI BETWEEN '" & Format(datKaishibi, "yyyy/MM/dd") & "' AND '" & Format(datShimebi, "yyyy/MM/dd") & "' "

        Return strSql

    End Function

    '------------------------------------------------------------
    'その月が締め済みか
    '------------------------------------------------------------
    Private Function ShimeZumi(ByVal cn As SqlConnection, ByVal strYm As String) As Boolean

        Dim cm As SqlCommand
        Dim strSql As String

        strSql = ""
        strSql = strSql & "SELECT COUNT(*) FROM T_TH_SHIME "
        strSql = strSql & "WHERE SHIME_YM = '" & strYm & "' AND TORIKESHI_KBN = '0'"

        cm = New SqlCommand(strSql, cn)

        If Val(CStr(cm.ExecuteScalar())) > 0 Then
            Return True
        End If

        Return False

    End Function

    '------------------------------------------------------------
    '締めNo採番
    '※タイ納品入力と同じやり方。取り消した締めも残してあるので、その番号は欠番になる
    '------------------------------------------------------------
    Private Function Saiban(ByVal cn As SqlConnection, ByVal tr As SqlTransaction) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object
        Dim lngMax As Long
        Dim strNo As String
        Dim i As Integer

        For i = 1 To 3

            strSql = "SELECT MAX(SHIME_NO) FROM T_TH_SHIME"

            cm = New SqlCommand(strSql, cn, tr)
            objRet = cm.ExecuteScalar()

            If IsDBNull(objRet) Or objRet Is Nothing Then
                lngMax = 0
            Else
                lngMax = Val(CStr(objRet))
            End If

            strNo = Format(lngMax + 1, "000000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM T_TH_SHIME WHERE SHIME_NO = '" & strNo & "'"
            cm = New SqlCommand(strSql, cn, tr)

            If Val(CStr(cm.ExecuteScalar())) = 0 Then
                Return strNo
            End If

            System.Threading.Thread.Sleep(200)

        Next

        Return ""

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
