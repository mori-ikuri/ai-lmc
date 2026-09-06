Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 備品管理
' 機能名     : 共通モジュール
' 作成者     : 山田
'
' 履歴
' 2006/05/22 山田 新規作成
' 2007/02/08 山田 サーバ入替に伴い接続先変更
'============================================================
Module modCommon

    '接続文字列 ※サーバを変えたらここを直すこと
    'Public Const CONN_STR As String = "Data Source=SVR-OLD\SQLEXPRESS;Initial Catalog=SOUMU;User ID=sa;Password=soumu2006;"
    Public Const CONN_STR As String = "Data Source=SVR01\SQLEXPRESS;Initial Catalog=SOUMU;User ID=sa;Password=soumu2006;"

    'ログインユーザ名 (Form_Loadでセット)
    Public gUserName As String = ""

    '------------------------------------------------------------
    '接続を開いて返す
    '------------------------------------------------------------
    Public Function GetConnection() As SqlConnection

        Dim cn As SqlConnection

        cn = New SqlConnection(CONN_STR)
        cn.Open()

        Return cn

    End Function

    '------------------------------------------------------------
    '空文字ならNULLを返す
    '------------------------------------------------------------
    Public Function NullIfEmpty(ByVal strVal As String) As Object

        If Trim(strVal) = "" Then
            Return DBNull.Value
        End If

        Return Trim(strVal)

    End Function

    '------------------------------------------------------------
    'SQL用に'をエスケープする
    '2006/08/30 山田 品名に'が入っていてエラーになったため追加
    '------------------------------------------------------------
    Public Function EscQuote(ByVal strVal As String) As String

        Return Replace(strVal, "'", "''")

    End Function

    '------------------------------------------------------------
    'DBの値を文字列にする(NULL対応)
    '------------------------------------------------------------
    Public Function ToStr(ByVal objVal As Object) As String

        If IsDBNull(objVal) Then
            Return ""
        End If
        If objVal Is Nothing Then
            Return ""
        End If

        Return CStr(objVal)

    End Function

End Module
