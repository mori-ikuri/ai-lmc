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
' 2009/02/10 山田 締めの期間を出す処理を追加
' 2009/03/23 山田 年度を返す処理を追加
' 2011/11/08 山田 タイの受注CSVの置き場所を追加
'============================================================
Module modCommon

    '接続文字列 ※サーバを変えたらここを直すこと
    Public Const CONN_STR As String = "Data Source=SVR01\SQLEXPRESS;Initial Catalog=HANBAI;User ID=sa;Password=hanbai2008;"

    '受注CSVの置き場所
    Public Const CSV_PATH As String = "\SVR01\share\seisan\"

    'タイの受注CSVの置き場所
    '2011/11/08 山田 中村さんが現地からもらってきたものはここに入れてもらっている
    Public Const TH_CSV_PATH As String = "\SVR01\share\thai\"

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
    '締め日を返す
    '2009/02/10 山田 締日は31で持っているが、2月は28日(29日)しかない。
    '                 その月の末日を超えるときは末日にする
    '------------------------------------------------------------
    Public Function ShimebiOf(ByVal intYear As Integer, ByVal intMonth As Integer, ByVal intShimebi As Integer) As Date

        Dim datMatsujitsu As Date

        '翌月の0日でその月の末日になる
        datMatsujitsu = DateSerial(intYear, intMonth + 1, 0)

        'System.Windows.Forms にも Day があってそのままでは通らない
        If intShimebi >= Microsoft.VisualBasic.Day(datMatsujitsu) Then
            Return datMatsujitsu
        End If

        Return DateSerial(intYear, intMonth, intShimebi)

    End Function

    '------------------------------------------------------------
    '締めの開始日を返す ※前月の締め日の翌日
    '------------------------------------------------------------
    Public Function KaishibiOf(ByVal intYear As Integer, ByVal intMonth As Integer, ByVal intShimebi As Integer) As Date

        Dim datZengetsu As Date

        datZengetsu = DateAdd("m", -1, DateSerial(intYear, intMonth, 1))

        Return DateAdd("d", 1, ShimebiOf(Year(datZengetsu), Month(datZengetsu), intShimebi))

    End Function

    '------------------------------------------------------------
    '年度を返す ※4月から翌年3月まで
    '2009/03/23 山田 請求書番号の頭に付ける年度用
    '------------------------------------------------------------
    Public Function NendoOf(ByVal datHi As Date) As Integer

        If Month(datHi) >= 4 Then
            Return Year(datHi)
        End If

        Return Year(datHi) - 1

    End Function

    '------------------------------------------------------------
    '得意先の丸めを掛ける
    '2008/08/18 山田 Math.Roundは .5 が偶数側へ丸まって経理の四捨五入と合わないため
    '                 Int(x + 0.5) で計算する
    '2009/02/16 山田 呼ぶ場所を納品1行ごとから請求の合計へ移した
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
