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
    '終了
    '------------------------------------------------------------
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

        Me.Close()

    End Sub

End Class
