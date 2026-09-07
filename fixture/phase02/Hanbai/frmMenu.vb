Option Strict Off
Option Explicit On

'============================================================
' システム名 : 販売
' 機能名     : メニュー
' 作成者     : 山田
'
' 履歴
' 2008/04/21 山田 新規作成
' 2008/07/14 山田 納品入力を追加
' 2009/02/12 山田 締め処理を追加
' 2009/03/02 山田 入金入力を追加
' 2009/03/23 山田 請求書発行を追加
'============================================================
Public Class frmMenu

    Private Sub frmMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gUserName = Environment.UserName

        Me.Text = "販売  [" & gUserName & "]"

    End Sub

    '------------------------------------------------------------
    '得意先登録
    '------------------------------------------------------------
    Private Sub btnTokuisaki_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTokuisaki.Click

        Dim f As frmTokuisaki

        f = New frmTokuisaki
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '単価登録
    '------------------------------------------------------------
    Private Sub btnTanka_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTanka.Click

        Dim f As frmTanka

        f = New frmTanka
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '受注取込
    '------------------------------------------------------------
    Private Sub btnJuchu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJuchu.Click

        Dim f As frmJuchuTorikomi

        f = New frmJuchuTorikomi
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '納品入力
    '------------------------------------------------------------
    Private Sub btnNohin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNohin.Click

        Dim f As frmNohin

        f = New frmNohin
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '締め処理
    '------------------------------------------------------------
    Private Sub btnShime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShime.Click

        Dim f As frmShime

        f = New frmShime
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '請求書発行
    '------------------------------------------------------------
    Private Sub btnSeikyusho_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeikyusho.Click

        Dim f As frmSeikyusho

        f = New frmSeikyusho
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '入金入力
    '------------------------------------------------------------
    Private Sub btnNyukin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNyukin.Click

        Dim f As frmNyukin

        f = New frmNyukin
        f.ShowDialog()
        f.Dispose()

    End Sub

    '------------------------------------------------------------
    '終了
    '------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Me.Close()

    End Sub

End Class
