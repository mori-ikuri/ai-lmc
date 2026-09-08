Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 締め処理
' 作成者     : 山田
'
' 履歴
' 2009/02/12 山田 新規作成
' 2009/03/09 山田 締め取消を追加
'                 締めたあとに納品を直したいときは、いったん取り消して
'                 入れ直してから締め直してもらう
' 2009/03/16 山田 1回目の締めの前回請求額に得意先の開始残高を使うようにした
' 2009/04/24 山田 初回の締めが一覧で分かるようにした。
'                 初回だけ前回請求額が得意先の開始残高から来るので、
'                 締める前に田島さんにExcelと突き合わせてもらう
'============================================================
Public Class frmShime

    '選択中の請求先
    Private mstrSeikyusakiCd As String = ""

    '選択中の行の請求書番号(空なら未締め)
    Private mstrSeikyuNo As String = ""

    'ダブルクリック連打対策
    Private mblnShori As Boolean = False

    'Form_Load中は月を変えても出しなおさない
    Private mblnLoad As Boolean = False

#Region " 画面初期処理 "

    Private Sub frmShime_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer

        Me.Text = "締め処理  [" & gUserName & "]"

        mblnLoad = True

        cboTsuki.Items.Clear()
        For i = 1 To 12
            cboTsuki.Items.Add(CStr(i))
        Next

        '前の月を締めることが多いので前月を出しておく
        txtNen.Text = CStr(Year(DateAdd("m", -1, Now)))
        cboTsuki.Text = CStr(Month(DateAdd("m", -1, Now)))

        dtpHakkobi.Value = Now

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
    '------------------------------------------------------------
    Private Sub btnShime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShime.Click

        Dim cn As SqlConnection = Nothing
        Dim tr As SqlTransaction = Nothing
        Dim cm As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String
        Dim strYm As String
        Dim strMarumeKbn As String
        Dim strSeikyuNo As String
        Dim strJoken As String
        Dim intShimebi As Integer
        Dim datShimebi As Date
        Dim datKaishibi As Date
        Dim lngKensu As Long
        Dim blnShokai As Boolean
        Dim dblKaishiZan As Double
        Dim dblUriage As Double
        Dim dblZenkai As Double
        Dim dblNyukin As Double
        Dim dblKurikoshi As Double
        Dim dblZei As Double
        Dim dblSeikyu As Double

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If Check() = False Then
                mblnShori = False
                Exit Sub
            End If

            If mstrSeikyusakiCd = "" Then
                MsgBox("一覧から締める請求先を選択してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            If mstrSeikyuNo <> "" Then
                MsgBox("この請求先は請求書 " & mstrSeikyuNo & " で締め済みです。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

            cn = GetConnection()

            '締め日と丸めを取り直す
            strSql = ""
            strSql = strSql & "SELECT SHIMEBI, MARUME_KBN, ISNULL(KAISHI_ZAN, 0) AS KAISHI_ZAN "
            strSql = strSql & "FROM M_TOKUISAKI WHERE TOKUISAKI_CD = '" & EscQuote(mstrSeikyusakiCd) & "'"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "T")

            If ds.Tables("T").Rows.Count = 0 Then
                MsgBox("請求先が得意先に登録されていません。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            intShimebi = Val(ToStr(ds.Tables("T").Rows(0)("SHIMEBI")))
            strMarumeKbn = ToStr(ds.Tables("T").Rows(0)("MARUME_KBN"))
            dblKaishiZan = Val(ToStr(ds.Tables("T").Rows(0)("KAISHI_ZAN")))

            datShimebi = ShimebiOf(Val(txtNen.Text), Val(cboTsuki.Text), intShimebi)
            datKaishibi = KaishibiOf(Val(txtNen.Text), Val(cboTsuki.Text), intShimebi)

            '同じ月をもう一度締めていないか、ここでもう一度みる
            strSql = ""
            strSql = strSql & "SELECT COUNT(*) FROM T_SEIKYU "
            strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(mstrSeikyusakiCd) & "' "
            strSql = strSql & "AND SHIME_YM = '" & strYm & "' AND TORIKESHI_KBN = '0'"

            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) > 0 Then
                MsgBox("この請求先の " & strYm & " は既に締められています。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            '対象の納品 ※締め済みの分は請求書番号が入っているので拾わない
            strJoken = TaishoJoken(mstrSeikyusakiCd, datKaishibi, datShimebi)

            strSql = ""
            strSql = strSql & "SELECT COUNT(*) AS KENSU, ISNULL(SUM(N.KINGAKU), 0) AS GOKEI FROM T_NOHIN N "
            strSql = strSql & "INNER JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = N.TOKUISAKI_CD "
            strSql = strSql & strJoken

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "N")

            lngKensu = Val(ToStr(ds.Tables("N").Rows(0)("KENSU")))

            '2009/02/16 山田 丸めは1行ずつではなく、ここで合計に1回だけ掛ける
            dblUriage = Marume(Val(ToStr(ds.Tables("N").Rows(0)("GOKEI"))), strMarumeKbn)

            dblZenkai = ZenkaiSeikyuGaku(cn, mstrSeikyusakiCd, datShimebi, dblKaishiZan)
            blnShokai = Not ZenkaiAri(cn, mstrSeikyusakiCd, datShimebi)
            dblNyukin = NyukinGaku(cn, mstrSeikyusakiCd, datKaishibi, datShimebi)
            dblKurikoshi = dblZenkai - dblNyukin
            dblZei = Int(dblUriage * 0.05)
            dblSeikyu = dblKurikoshi + dblUriage + dblZei

            '2009/04/24 山田 初回は前回請求額が開始残高から来るので、先に見てもらう
            If blnShokai = True Then

                If MsgBox("この請求先は今回が初めての締めです。" & vbCrLf & _
                          "前回請求額 " & Format(dblZenkai, "#,##0") & " 円 は、得意先登録の開始残高から出しています。" & vbCrLf & _
                          "Excelの前回の請求額と合っているか確かめてください。" & vbCrLf & vbCrLf & _
                          "違っていたら、いったんやめて得意先登録で直してください。" & vbCrLf & _
                          "このまま進みますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            If lngKensu = 0 Then

                If MsgBox(Format(datKaishibi, "yyyy/MM/dd") & " ～ " & Format(datShimebi, "yyyy/MM/dd") & " に納品がありません。" & vbCrLf & _
                          "繰越だけで締めますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            Else

                If MsgBox(mstrSeikyusakiCd & " を締めます。" & vbCrLf & _
                          "期間 " & Format(datKaishibi, "yyyy/MM/dd") & " ～ " & Format(datShimebi, "yyyy/MM/dd") & vbCrLf & _
                          "納品 " & CStr(lngKensu) & " 件   今回請求額 " & Format(dblSeikyu, "#,##0") & " 円" & vbCrLf & vbCrLf & _
                          "締めてよろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            '請求を作るのと納品への書き込みは、片方だけ通ると二重請求になるのでまとめて行う
            tr = cn.BeginTransaction()

            strSeikyuNo = SeikyuSaiban(cn, tr, dtpHakkobi.Value)

            If strSeikyuNo = "" Then
                tr.Rollback()
                tr = Nothing
                MsgBox("請求書Noの採番に失敗しました。もう一度実行してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            strSql = ""
            strSql = strSql & "INSERT INTO T_SEIKYU "
            strSql = strSql & "(SEIKYU_NO, SEIKYUSAKI_CD, SHIME_YM, KAISHIBI, SHIMEBI, HAKKOBI, "
            strSql = strSql & "ZENKAI_GAKU, NYUKIN_GAKU, KURIKOSHI_GAKU, URIAGE_GAKU, SHOHIZEI, SEIKYU_GAKU, "
            strSql = strSql & "INSATSU_DT, TORIKESHI_KBN, KOSHIN_USER, KOSHIN_DT) "
            strSql = strSql & "VALUES ("
            strSql = strSql & "'" & strSeikyuNo & "',"
            strSql = strSql & "'" & EscQuote(mstrSeikyusakiCd) & "',"
            strSql = strSql & "'" & strYm & "',"
            strSql = strSql & "'" & Format(datKaishibi, "yyyy/MM/dd") & "',"
            strSql = strSql & "'" & Format(datShimebi, "yyyy/MM/dd") & "',"
            strSql = strSql & "'" & Format(dtpHakkobi.Value, "yyyy/MM/dd") & "',"
            strSql = strSql & CStr(dblZenkai) & ","
            strSql = strSql & CStr(dblNyukin) & ","
            strSql = strSql & CStr(dblKurikoshi) & ","
            strSql = strSql & CStr(dblUriage) & ","
            strSql = strSql & CStr(dblZei) & ","
            strSql = strSql & CStr(dblSeikyu) & ","
            strSql = strSql & "NULL,"
            strSql = strSql & "'0',"
            strSql = strSql & "'" & gUserName & "',"
            strSql = strSql & "GETDATE())"

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            '締めた納品に請求書番号を書く。これが入っている納品はもう拾わない
            strSql = ""
            strSql = strSql & "UPDATE N SET N.SEIKYU_NO = '" & strSeikyuNo & "',"
            strSql = strSql & "N.KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "N.KOSHIN_DT = GETDATE() "
            strSql = strSql & "FROM T_NOHIN N "
            strSql = strSql & "INNER JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = N.TOKUISAKI_CD "
            strSql = strSql & strJoken

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            tr.Commit()
            tr = Nothing

            MsgBox("締めました。請求書No = " & strSeikyuNo, MsgBoxStyle.Information)

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
    '2009/03/09 山田 締めたあとに納品を直したいときはこれで戻してもらう
    '------------------------------------------------------------
    Private Sub btnTorikeshi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTorikeshi.Click

        Dim cn As SqlConnection = Nothing
        Dim tr As SqlTransaction = Nothing
        Dim cm As SqlCommand
        Dim strSql As String
        Dim strInsatsu As String

        If mblnShori = True Then
            Exit Sub
        End If
        mblnShori = True

        Try

            If mstrSeikyuNo = "" Then
                MsgBox("一覧から締め済みの行を選択してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            cn = GetConnection()

            'これより後の締めが残っていると前回請求額がずれるので、新しいほうから取り消してもらう
            strSql = ""
            strSql = strSql & "SELECT COUNT(*) FROM T_SEIKYU "
            strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(mstrSeikyusakiCd) & "' "
            strSql = strSql & "AND TORIKESHI_KBN = '0' "
            strSql = strSql & "AND SHIMEBI > (SELECT SHIMEBI FROM T_SEIKYU WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "')"

            cm = New SqlCommand(strSql, cn)

            If Val(CStr(cm.ExecuteScalar())) > 0 Then
                MsgBox("この請求より後の締めが残っています。" & vbCrLf & _
                       "新しいほうから順に取り消してください。", MsgBoxStyle.Exclamation)
                mblnShori = False
                Exit Sub
            End If

            strSql = "SELECT ISNULL(CONVERT(varchar(10), INSATSU_DT, 111), '') FROM T_SEIKYU WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"
            cm = New SqlCommand(strSql, cn)
            strInsatsu = ToStr(cm.ExecuteScalar())

            If strInsatsu <> "" Then

                If MsgBox("請求書 " & mstrSeikyuNo & " は " & strInsatsu & " に印刷しています。" & vbCrLf & _
                          "先方に送っていないか確かめてから取り消してください。" & vbCrLf & vbCrLf & _
                          "取り消しますか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            Else

                If MsgBox("請求書 " & mstrSeikyuNo & " の締めを取り消します。" & vbCrLf & _
                          "この番号は欠番になります。よろしいですか?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) <> MsgBoxResult.Yes Then
                    mblnShori = False
                    Exit Sub
                End If

            End If

            tr = cn.BeginTransaction()

            '納品を未請求に戻す
            strSql = ""
            strSql = strSql & "UPDATE T_NOHIN SET SEIKYU_NO = NULL,"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            '請求は消さずに取消区分を立てる
            strSql = ""
            strSql = strSql & "UPDATE T_SEIKYU SET TORIKESHI_KBN = '1',"
            strSql = strSql & "KOSHIN_USER = '" & gUserName & "',"
            strSql = strSql & "KOSHIN_DT = GETDATE() "
            strSql = strSql & "WHERE SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "'"

            cm = New SqlCommand(strSql, cn, tr)
            cm.ExecuteNonQuery()

            tr.Commit()
            tr = Nothing

            MsgBox("締めを取り消しました。納品を直したら、もう一度締めてください。", MsgBoxStyle.Information)

            mstrSeikyuNo = ""

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
    '締めの対象を請求先ごとに出す
    '※締め日が請求先ごとに違うので、1件ずつ期間を出して集計する
    '------------------------------------------------------------
    Private Sub HyojiTaisho()

        Dim cn As SqlConnection = Nothing
        Dim cm As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim dsT As DataSet
        Dim dt As DataTable
        Dim dr As DataRow
        Dim strSql As String
        Dim strYm As String
        Dim strCd As String
        Dim strMarumeKbn As String
        Dim strSeikyuNo As String
        Dim strJoken As String
        Dim intShimebi As Integer
        Dim intFurui As Integer
        Dim datShimebi As Date
        Dim datKaishibi As Date
        Dim dblUriage As Double
        Dim dblZenkai As Double
        Dim dblNyukin As Double
        Dim dblKurikoshi As Double
        Dim dblZei As Double
        Dim dblSeikyu As Double
        Dim i As Integer

        mstrSeikyusakiCd = ""
        mstrSeikyuNo = ""
        dgvNohin.DataSource = Nothing
        lblChui.Text = ""

        If Check() = False Then
            Exit Sub
        End If

        strYm = Format(Val(txtNen.Text), "0000") & Format(Val(cboTsuki.Text), "00")

        Try

            cn = GetConnection()

            dt = New DataTable
            dt.Columns.Add("SEIKYUSAKI_CD")
            dt.Columns.Add("SEIKYUSAKI_NM")
            dt.Columns.Add("SHOKAI")
            dt.Columns.Add("KAISHIBI")
            dt.Columns.Add("SHIMEBI")
            dt.Columns.Add("KENSU", GetType(Integer))
            dt.Columns.Add("URIAGE", GetType(Double))
            dt.Columns.Add("ZENKAI", GetType(Double))
            dt.Columns.Add("NYUKIN", GetType(Double))
            dt.Columns.Add("KURIKOSHI", GetType(Double))
            dt.Columns.Add("ZEI", GetType(Double))
            dt.Columns.Add("SEIKYU", GetType(Double))
            dt.Columns.Add("SEIKYU_NO")

            '請求先 ※請求先が空の得意先が、そのまま自分あての請求先になる
            strSql = ""
            strSql = strSql & "SELECT TOKUISAKI_CD, TOKUISAKI_NM, SHIMEBI, MARUME_KBN, ISNULL(KAISHI_ZAN, 0) AS KAISHI_ZAN "
            strSql = strSql & "FROM M_TOKUISAKI "
            strSql = strSql & "WHERE SHIYO_KBN = '0' "
            strSql = strSql & "AND (SEIKYUSAKI_CD IS NULL OR SEIKYUSAKI_CD = '') "
            strSql = strSql & "ORDER BY TOKUISAKI_CD"

            dsT = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(dsT, "TOKUISAKI")

            intFurui = 0

            For i = 0 To dsT.Tables("TOKUISAKI").Rows.Count - 1

                strCd = ToStr(dsT.Tables("TOKUISAKI").Rows(i)("TOKUISAKI_CD"))
                intShimebi = Val(ToStr(dsT.Tables("TOKUISAKI").Rows(i)("SHIMEBI")))
                strMarumeKbn = ToStr(dsT.Tables("TOKUISAKI").Rows(i)("MARUME_KBN"))

                datShimebi = ShimebiOf(Val(txtNen.Text), Val(cboTsuki.Text), intShimebi)
                datKaishibi = KaishibiOf(Val(txtNen.Text), Val(cboTsuki.Text), intShimebi)

                dr = dt.NewRow()

                '前の締めが無ければ、前回請求額は得意先の開始残高から来る
                If ZenkaiAri(cn, strCd, datShimebi) = True Then
                    dr("SHOKAI") = ""
                Else
                    dr("SHOKAI") = "初回"
                End If

                '締め済みかどうか
                strSql = ""
                strSql = strSql & "SELECT ISNULL(MAX(SEIKYU_NO), '') FROM T_SEIKYU "
                strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(strCd) & "' "
                strSql = strSql & "AND SHIME_YM = '" & strYm & "' AND TORIKESHI_KBN = '0'"

                cm = New SqlCommand(strSql, cn)
                strSeikyuNo = ToStr(cm.ExecuteScalar())

                If strSeikyuNo <> "" Then

                    '締め済みは請求に入れた数字をそのまま出す
                    strSql = ""
                    strSql = strSql & "SELECT ZENKAI_GAKU, NYUKIN_GAKU, KURIKOSHI_GAKU, URIAGE_GAKU, SHOHIZEI, SEIKYU_GAKU, "
                    strSql = strSql & "(SELECT COUNT(*) FROM T_NOHIN WHERE SEIKYU_NO = T_SEIKYU.SEIKYU_NO) AS KENSU "
                    strSql = strSql & "FROM T_SEIKYU WHERE SEIKYU_NO = '" & EscQuote(strSeikyuNo) & "'"

                    ds = New DataSet
                    da = New SqlDataAdapter(strSql, cn)
                    da.Fill(ds, "S")

                    dblZenkai = Val(ToStr(ds.Tables("S").Rows(0)("ZENKAI_GAKU")))
                    dblNyukin = Val(ToStr(ds.Tables("S").Rows(0)("NYUKIN_GAKU")))
                    dblKurikoshi = Val(ToStr(ds.Tables("S").Rows(0)("KURIKOSHI_GAKU")))
                    dblUriage = Val(ToStr(ds.Tables("S").Rows(0)("URIAGE_GAKU")))
                    dblZei = Val(ToStr(ds.Tables("S").Rows(0)("SHOHIZEI")))
                    dblSeikyu = Val(ToStr(ds.Tables("S").Rows(0)("SEIKYU_GAKU")))

                    dr("KENSU") = Val(ToStr(ds.Tables("S").Rows(0)("KENSU")))

                Else

                    strJoken = TaishoJoken(strCd, datKaishibi, datShimebi)

                    strSql = ""
                    strSql = strSql & "SELECT COUNT(*) AS KENSU, ISNULL(SUM(N.KINGAKU), 0) AS GOKEI FROM T_NOHIN N "
                    strSql = strSql & "INNER JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = N.TOKUISAKI_CD "
                    strSql = strSql & strJoken

                    ds = New DataSet
                    da = New SqlDataAdapter(strSql, cn)
                    da.Fill(ds, "N")

                    '2009/02/16 山田 丸めは合計に1回だけ掛ける
                    dblUriage = Marume(Val(ToStr(ds.Tables("N").Rows(0)("GOKEI"))), strMarumeKbn)

                    dblZenkai = ZenkaiSeikyuGaku(cn, strCd, datShimebi, Val(ToStr(dsT.Tables("TOKUISAKI").Rows(i)("KAISHI_ZAN"))))
                    dblNyukin = NyukinGaku(cn, strCd, datKaishibi, datShimebi)
                    dblKurikoshi = dblZenkai - dblNyukin
                    dblZei = Int(dblUriage * 0.05)
                    dblSeikyu = dblKurikoshi + dblUriage + dblZei

                    dr("KENSU") = Val(ToStr(ds.Tables("N").Rows(0)("KENSU")))

                    '締めの期間より前に未請求の納品が残っていないかみる
                    strSql = ""
                    strSql = strSql & "SELECT COUNT(*) FROM T_NOHIN N "
                    strSql = strSql & "INNER JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = N.TOKUISAKI_CD "
                    strSql = strSql & "WHERE ISNULL(NULLIF(T.SEIKYUSAKI_CD, ''), T.TOKUISAKI_CD) = '" & EscQuote(strCd) & "' "
                    strSql = strSql & "AND N.TORIKESHI_KBN = '0' "
                    strSql = strSql & "AND N.SEIKYU_NO IS NULL "
                    strSql = strSql & "AND N.NOHINBI < '" & Format(datKaishibi, "yyyy/MM/dd") & "'"

                    cm = New SqlCommand(strSql, cn)
                    intFurui = intFurui + Val(CStr(cm.ExecuteScalar()))

                End If

                dr("SEIKYUSAKI_CD") = strCd
                dr("SEIKYUSAKI_NM") = ToStr(dsT.Tables("TOKUISAKI").Rows(i)("TOKUISAKI_NM"))
                dr("KAISHIBI") = Format(datKaishibi, "yyyy/MM/dd")
                dr("SHIMEBI") = Format(datShimebi, "yyyy/MM/dd")
                dr("URIAGE") = dblUriage
                dr("ZENKAI") = dblZenkai
                dr("NYUKIN") = dblNyukin
                dr("KURIKOSHI") = dblKurikoshi
                dr("ZEI") = dblZei
                dr("SEIKYU") = dblSeikyu
                dr("SEIKYU_NO") = strSeikyuNo

                dt.Rows.Add(dr)

            Next

            dgvTaisho.DataSource = dt

            dgvTaisho.Columns(0).HeaderText = "請求先"
            dgvTaisho.Columns(1).HeaderText = "請求先名"
            dgvTaisho.Columns(2).HeaderText = "初回"
            dgvTaisho.Columns(3).HeaderText = "開始日"
            dgvTaisho.Columns(4).HeaderText = "締め日"
            dgvTaisho.Columns(5).HeaderText = "件数"
            dgvTaisho.Columns(6).HeaderText = "今回売上額"
            dgvTaisho.Columns(7).HeaderText = "前回請求額"
            dgvTaisho.Columns(8).HeaderText = "入金額"
            dgvTaisho.Columns(9).HeaderText = "繰越額"
            dgvTaisho.Columns(10).HeaderText = "消費税"
            dgvTaisho.Columns(11).HeaderText = "今回請求額"
            dgvTaisho.Columns(12).HeaderText = "請求書No"

            dgvTaisho.Columns(0).Width = 55
            dgvTaisho.Columns(1).Width = 120
            dgvTaisho.Columns(2).Width = 40
            dgvTaisho.Columns(3).Width = 70
            dgvTaisho.Columns(4).Width = 70
            dgvTaisho.Columns(5).Width = 40
            dgvTaisho.Columns(6).Width = 80
            dgvTaisho.Columns(7).Width = 80
            dgvTaisho.Columns(8).Width = 75
            dgvTaisho.Columns(9).Width = 75
            dgvTaisho.Columns(10).Width = 65
            dgvTaisho.Columns(11).Width = 80
            dgvTaisho.Columns(12).Width = 65

            For i = 5 To 11
                dgvTaisho.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvTaisho.Columns(i).DefaultCellStyle.Format = "#,##0"
            Next

            lblKensu.Text = "請求先 " & CStr(dt.Rows.Count) & " 件"

            '2009/03/09 山田 期間より前の納品が残っていると請求漏れになるので出しておく
            If intFurui > 0 Then
                lblChui.Text = "※締めの期間より前に、まだ請求していない納品が " & CStr(intFurui) & " 件あります"
            End If

        Catch ex As Exception

            MsgBox("締め対象の取得でエラーが発生しました。" & vbCrLf & ex.Message, MsgBoxStyle.Critical)

        Finally

            If Not cn Is Nothing Then
                cn.Close()
            End If

        End Try

    End Sub

    '------------------------------------------------------------
    '一覧クリックで、その請求先の納品を下に出す
    '------------------------------------------------------------
    Private Sub dgvTaisho_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTaisho.CellClick

        Dim r As DataGridViewRow

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        r = dgvTaisho.Rows(e.RowIndex)

        mstrSeikyusakiCd = ToStr(r.Cells(0).Value)
        mstrSeikyuNo = ToStr(r.Cells(12).Value)

        Call HyojiNohin(ToStr(r.Cells(3).Value), ToStr(r.Cells(4).Value))

    End Sub

    '------------------------------------------------------------
    '選んだ請求先の対象納品
    '------------------------------------------------------------
    Private Sub HyojiNohin(ByVal strKaishibi As String, ByVal strShimebi As String)

        Dim cn As SqlConnection = Nothing
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strSql As String

        If mstrSeikyusakiCd = "" Then
            dgvNohin.DataSource = Nothing
            Exit Sub
        End If

        Try

            cn = GetConnection()

            strSql = ""
            strSql = strSql & "SELECT CONVERT(varchar(10), N.NOHINBI, 111) AS NOHINBI, "
            strSql = strSql & "N.NOHIN_NO, T.TOKUISAKI_NM, N.HINBAN, N.HINMEI, N.SURYO, N.TANKA, N.KINGAKU, "
            strSql = strSql & "CASE WHEN N.MUSHO_KBN = '1' THEN '無償' ELSE '' END AS MUSHO_NM "
            strSql = strSql & "FROM T_NOHIN N "
            strSql = strSql & "INNER JOIN M_TOKUISAKI T ON T.TOKUISAKI_CD = N.TOKUISAKI_CD "
            strSql = strSql & "WHERE ISNULL(NULLIF(T.SEIKYUSAKI_CD, ''), T.TOKUISAKI_CD) = '" & EscQuote(mstrSeikyusakiCd) & "' "
            strSql = strSql & "AND N.TORIKESHI_KBN = '0' "

            If mstrSeikyuNo = "" Then
                strSql = strSql & "AND N.SEIKYU_NO IS NULL "
                strSql = strSql & "AND N.NOHINBI BETWEEN '" & strKaishibi & "' AND '" & strShimebi & "' "
            Else
                strSql = strSql & "AND N.SEIKYU_NO = '" & EscQuote(mstrSeikyuNo) & "' "
            End If

            strSql = strSql & "ORDER BY N.NOHINBI, N.NOHIN_NO"

            ds = New DataSet
            da = New SqlDataAdapter(strSql, cn)
            da.Fill(ds, "NOHIN")

            dgvNohin.DataSource = ds.Tables("NOHIN")

            dgvNohin.Columns(0).HeaderText = "納品日"
            dgvNohin.Columns(1).HeaderText = "納品No"
            dgvNohin.Columns(2).HeaderText = "納品先"
            dgvNohin.Columns(3).HeaderText = "品番"
            dgvNohin.Columns(4).HeaderText = "品名"
            dgvNohin.Columns(5).HeaderText = "数量"
            dgvNohin.Columns(6).HeaderText = "単価"
            dgvNohin.Columns(7).HeaderText = "金額"
            dgvNohin.Columns(8).HeaderText = "無償"

            dgvNohin.Columns(0).Width = 90
            dgvNohin.Columns(1).Width = 70
            dgvNohin.Columns(2).Width = 150
            dgvNohin.Columns(3).Width = 110
            dgvNohin.Columns(4).Width = 180
            dgvNohin.Columns(5).Width = 70
            dgvNohin.Columns(6).Width = 80
            dgvNohin.Columns(7).Width = 90
            dgvNohin.Columns(8).Width = 50

            dgvNohin.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvNohin.Columns(6).DefaultCellStyle.Format = "#,##0.00"
            dgvNohin.Columns(7).DefaultCellStyle.Format = "#,##0.##"

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
    '※一覧を出すときと締めるときで条件が違うと二重請求になるので、ここにまとめてある
    '------------------------------------------------------------
    Private Function TaishoJoken(ByVal strSeikyusakiCd As String, ByVal datKaishibi As Date, ByVal datShimebi As Date) As String

        Dim strSql As String

        strSql = ""
        strSql = strSql & "WHERE ISNULL(NULLIF(T.SEIKYUSAKI_CD, ''), T.TOKUISAKI_CD) = '" & EscQuote(strSeikyusakiCd) & "' "
        strSql = strSql & "AND N.TORIKESHI_KBN = '0' "
        strSql = strSql & "AND N.SEIKYU_NO IS NULL "
        strSql = strSql & "AND N.NOHINBI BETWEEN '" & Format(datKaishibi, "yyyy/MM/dd") & "' AND '" & Format(datShimebi, "yyyy/MM/dd") & "'"

        Return strSql

    End Function

    '------------------------------------------------------------
    '前回請求額
    '※前の締めが無ければ得意先の開始残高を使う(Excelから移ってきた分)
    '------------------------------------------------------------
    Private Function ZenkaiSeikyuGaku(ByVal cn As SqlConnection, ByVal strSeikyusakiCd As String, ByVal datShimebi As Date, ByVal dblKaishiZan As Double) As Double

        Dim cm As SqlCommand
        Dim strSql As String
        Dim objRet As Object

        strSql = ""
        strSql = strSql & "SELECT TOP 1 SEIKYU_GAKU FROM T_SEIKYU "
        strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(strSeikyusakiCd) & "' "
        strSql = strSql & "AND TORIKESHI_KBN = '0' "
        strSql = strSql & "AND SHIMEBI < '" & Format(datShimebi, "yyyy/MM/dd") & "' "
        strSql = strSql & "ORDER BY SHIMEBI DESC"

        cm = New SqlCommand(strSql, cn)
        objRet = cm.ExecuteScalar()

        If IsDBNull(objRet) Or objRet Is Nothing Then
            Return dblKaishiZan
        End If

        Return Val(CStr(objRet))

    End Function

    '------------------------------------------------------------
    '前の締めがあるか
    '※無ければ前回請求額は得意先の開始残高から来る(初回)
    '------------------------------------------------------------
    Private Function ZenkaiAri(ByVal cn As SqlConnection, ByVal strSeikyusakiCd As String, ByVal datShimebi As Date) As Boolean

        Dim cm As SqlCommand
        Dim strSql As String

        strSql = ""
        strSql = strSql & "SELECT COUNT(*) FROM T_SEIKYU "
        strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(strSeikyusakiCd) & "' "
        strSql = strSql & "AND TORIKESHI_KBN = '0' "
        strSql = strSql & "AND SHIMEBI < '" & Format(datShimebi, "yyyy/MM/dd") & "'"

        cm = New SqlCommand(strSql, cn)

        If Val(CStr(cm.ExecuteScalar())) > 0 Then
            Return True
        End If

        Return False

    End Function

    '------------------------------------------------------------
    '締めの期間に入った入金
    '------------------------------------------------------------
    Private Function NyukinGaku(ByVal cn As SqlConnection, ByVal strSeikyusakiCd As String, ByVal datKaishibi As Date, ByVal datShimebi As Date) As Double

        Dim cm As SqlCommand
        Dim strSql As String

        strSql = ""
        strSql = strSql & "SELECT ISNULL(SUM(KINGAKU), 0) FROM T_NYUKIN "
        strSql = strSql & "WHERE SEIKYUSAKI_CD = '" & EscQuote(strSeikyusakiCd) & "' "
        strSql = strSql & "AND TORIKESHI_KBN = '0' "
        strSql = strSql & "AND NYUKINBI BETWEEN '" & Format(datKaishibi, "yyyy/MM/dd") & "' AND '" & Format(datShimebi, "yyyy/MM/dd") & "'"

        cm = New SqlCommand(strSql, cn)

        Return Val(CStr(cm.ExecuteScalar()))

    End Function

    '------------------------------------------------------------
    '請求書Noの採番
    '※年度2桁-連番4桁。4月になったら0001に戻す(経理 田島さん)
    '  取り消した請求も残してあるので、その番号は欠番になる
    '------------------------------------------------------------
    Private Function SeikyuSaiban(ByVal cn As SqlConnection, ByVal tr As SqlTransaction, ByVal datHakkobi As Date) As String

        Dim cm As SqlCommand
        Dim strSql As String
        Dim strNendo As String
        Dim strMax As String
        Dim strNo As String
        Dim lngRen As Long
        Dim i As Integer

        strNendo = Format(NendoOf(datHakkobi) Mod 100, "00")

        For i = 1 To 3

            strSql = "SELECT ISNULL(MAX(SEIKYU_NO), '') FROM T_SEIKYU WHERE SEIKYU_NO LIKE '" & strNendo & "-%'"

            cm = New SqlCommand(strSql, cn, tr)
            strMax = ToStr(cm.ExecuteScalar())

            If strMax = "" Then
                lngRen = 1
            Else
                lngRen = Val(Mid(strMax, 4, 4)) + 1
            End If

            strNo = strNendo & "-" & Format(lngRen, "0000")

            '重複していないか一応みる
            strSql = "SELECT COUNT(*) FROM T_SEIKYU WHERE SEIKYU_NO = '" & strNo & "'"
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
