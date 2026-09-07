Option Strict Off
Option Explicit On

Imports System.Data.SqlClient

'============================================================
' システム名 : 販売
' 機能名     : 共通モジュール
' 作成者     : 山田
'
' 履歴
' 2008/04/21 山田 新規作成
' 2008/08/18 山田 丸めの処理を追加
'============================================================
Module modCommon

    '接続文字列 ※サーバを変えたらここを直すこと
    Public Const CONN_STR As String = "Data Source=SVR01\SQLEXPRESS;Initial Catalog=HANBAI;User ID=sa;Password=hanbai2008;"

    '受注CSVの置き場所
    Public Const CSV_PATH As String = "\SVR01\share\seisan\"

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

    '------------------------------------------------------------
    'SQLの日付にする 空ならNULL
    '------------------------------------------------------------
    Public Function ToSqlDate(ByVal strVal As String) As String

        If Trim(strVal) = "" Then
            Return "NULL"
        End If

        Return "'" & Format(CDate(strVal), "yyyy/MM/dd") & "'"

    End Function

    '------------------------------------------------------------
    '得意先の丸めを掛ける
    '2008/08/18 山田 Math.Roundは .5 が偶数側へ丸まって経理の四捨五入と合わないため
    '                 Int(x + 0.5) で計算する
    '------------------------------------------------------------
    Public Function Marume(ByVal dblVal As Double, ByVal strMarumeKbn As String) As Double

        If strMarumeKbn = "1" Then
            '四捨五入
            Return Int(dblVal + 0.5)
        End If

        '切り捨て
        Return Int(dblVal)

    End Function

End Module
