--============================================================
-- システム名 : 販売
-- 機能名     : テーブル作成
-- 作成者     : 山田
--
-- 履歴
-- 2008/04/21 山田 新規作成
-- 2008/06/09 山田 受注取込のテーブルを追加
-- 2008/07/14 山田 納品のテーブルを追加
-- 2008/08/05 山田 取込ログのテーブルを追加
-- 2008/09/16 山田 納品に取消区分を追加
-- 2009/02/10 山田 締めと請求のテーブルを追加
-- 2009/02/16 山田 納品の金額を丸める前の値で持つように変更
-- 2009/03/02 山田 入金のテーブルを追加
-- 2009/03/16 山田 得意先に開始残高を追加
-- 2011/11/08 山田 タイの得意先とタイの受注のテーブルを追加
--
-- ※SQL Server Management Studio Express で HANBAI に対して流すこと
--============================================================

--------------------------------------------------------------
-- 得意先マスタ
--------------------------------------------------------------
CREATE TABLE M_TOKUISAKI (
    TOKUISAKI_CD    varchar(10)     NOT NULL,   --得意先コード ※生産管理のCSVのコード
    TOKUISAKI_NM    varchar(40)     NOT NULL,   --得意先名
    SEIKYUSAKI_CD   varchar(10)     NULL,       --請求先の得意先コード ※空なら自分あて
    SHIMEBI         int             NOT NULL,   --締日 ※末日は31
    SHIHARAI        varchar(20)     NULL,       --支払条件
    MARUME_KBN      char(1)         NOT NULL,   --丸め 0:切り捨て 1:四捨五入
    SHIYO_KBN       char(1)         NOT NULL,   --0:使用中 1:使用しない
    BIKO            varchar(100)    NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_M_TOKUISAKI PRIMARY KEY (TOKUISAKI_CD)
)
GO

--------------------------------------------------------------
-- 単価マスタ
-- ※同じ品番でも得意先が違えば単価が違う(経理 田島さん)
--------------------------------------------------------------
CREATE TABLE M_TANKA (
    TOKUISAKI_CD    varchar(10)     NOT NULL,
    HINBAN          varchar(20)     NOT NULL,
    HINMEI          varchar(40)     NULL,       --請求書に出す品名
    TANKA           decimal(11,2)   NOT NULL,   --小数の単価があるので decimal
    SHIYO_KBN       char(1)         NOT NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_M_TANKA PRIMARY KEY (TOKUISAKI_CD, HINBAN)
)
GO

--------------------------------------------------------------
-- 受注 ※生産管理のCSVから取り込む
--------------------------------------------------------------
CREATE TABLE T_JUCHU (
    DENPYO_NO       varchar(10)     NOT NULL,   --伝票番号 CSVの9列目
    TOKUISAKI_CD    varchar(10)     NOT NULL,
    JUCHUBI         datetime        NOT NULL,
    NOKI            datetime        NULL,       --空で出てくることがある
    HINBAN          varchar(20)     NULL,
    HINMEI          varchar(40)     NULL,       --CSVの品名。品番がそのまま入っている行がある
    SURYO           int             NOT NULL,
    KBN             varchar(2)      NULL,       --CSVの7列目
    TORIKESHI_KBN   char(1)         NOT NULL,   --CSVの8列目 N:通常 C:取消
    TORIKOMI_FILE   varchar(40)     NULL,
    TORIKOMI_DT     datetime        NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_T_JUCHU PRIMARY KEY (DENPYO_NO)
)
GO

--------------------------------------------------------------
-- 納品
--------------------------------------------------------------
CREATE TABLE T_NOHIN (
    NOHIN_NO        varchar(10)     NOT NULL,
    NOHINBI         datetime        NOT NULL,
    DENPYO_NO       varchar(10)     NOT NULL,   --受注の伝票番号
    TOKUISAKI_CD    varchar(10)     NOT NULL,
    HINBAN          varchar(20)     NULL,
    HINMEI          varchar(40)     NULL,
    SURYO           int             NOT NULL,
    TANKA           decimal(11,2)   NOT NULL,
    KINGAKU         decimal(13,0)   NOT NULL,   --得意先の丸めを掛けた後の金額
    MUSHO_KBN       char(1)         NOT NULL,   --0:通常 1:無償支給
    TORIKESHI_KBN   char(1)         NOT NULL,   --0:通常 1:取消
    BIKO            varchar(100)    NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_T_NOHIN PRIMARY KEY (NOHIN_NO)
)
GO

--------------------------------------------------------------
-- 取込ログ ※同じファイルを二度選んだときに気付けるように
--------------------------------------------------------------
CREATE TABLE T_TORIKOMI_LOG (
    FILE_NM         varchar(40)     NOT NULL,
    TORIKOMI_DT     datetime        NULL,
    KENSU           int             NULL,
    TORIKOMI_USER   varchar(20)     NULL,
    CONSTRAINT PK_T_TORIKOMI_LOG PRIMARY KEY (FILE_NM)
)
GO

--------------------------------------------------------------
-- 2009/02/16 山田 納品の金額
-- 経理のExcelは1行ずつ丸めておらず、月の合計で丸まっている。
-- 東北機械のように単価に小数があると1行ずつ丸めた分だけずれるため、
-- 丸める前の金額で持ち、締めのときに合計へ1回だけ丸めを掛ける。
-- ※9月以降の納品はまだ請求に使っていないので入れ直してよい(経理 田島さんに確認済み)
--------------------------------------------------------------
ALTER TABLE T_NOHIN ALTER COLUMN KINGAKU decimal(13,2) NOT NULL
GO

UPDATE T_NOHIN SET KINGAKU = SURYO * TANKA
GO

--------------------------------------------------------------
-- 2009/02/10 山田 納品に請求書番号を持たせる
-- ※締めたときに入れる。空なら未請求。二重に請求しないための目印
--------------------------------------------------------------
ALTER TABLE T_NOHIN ADD SEIKYU_NO varchar(10) NULL
GO

--------------------------------------------------------------
-- 2009/03/16 山田 得意先に開始残高を追加
-- ※Excelから移ってくるときの前回請求額。1回目の締めのときだけ使う
--------------------------------------------------------------
ALTER TABLE M_TOKUISAKI ADD KAISHI_ZAN decimal(13,0) NULL
GO

--------------------------------------------------------------
-- 請求 ※締めると1件できる。請求書1枚がこの1件にあたる
--------------------------------------------------------------
CREATE TABLE T_SEIKYU (
    SEIKYU_NO       varchar(10)     NOT NULL,   --請求書番号 09-0001 の形
    SEIKYUSAKI_CD   varchar(10)     NOT NULL,   --請求先の得意先コード
    SHIME_YM        varchar(6)      NOT NULL,   --締めた年月 YYYYMM
    KAISHIBI        datetime        NOT NULL,   --締めの開始日
    SHIMEBI         datetime        NOT NULL,   --締め日 ※末日締めは実際の月末日が入る
    HAKKOBI         datetime        NOT NULL,   --請求書の発行日
    ZENKAI_GAKU     decimal(13,0)   NOT NULL,   --前回請求額
    NYUKIN_GAKU     decimal(13,0)   NOT NULL,   --入金額
    KURIKOSHI_GAKU  decimal(13,0)   NOT NULL,   --繰越額
    URIAGE_GAKU     decimal(13,0)   NOT NULL,   --今回売上額 ※合計に丸めを掛けた後
    SHOHIZEI        decimal(13,0)   NOT NULL,   --消費税
    SEIKYU_GAKU     decimal(13,0)   NOT NULL,   --今回請求額
    INSATSU_DT      datetime        NULL,       --請求書を印刷した日時
    TORIKESHI_KBN   char(1)         NOT NULL,   --0:通常 1:締め取消
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_T_SEIKYU PRIMARY KEY (SEIKYU_NO)
)
GO

--------------------------------------------------------------
-- 入金
-- ※どの納品に充てるかは経理でも見ていないとのこと(田島さん)。
--   請求先ごとに入った金額を記録するだけにする
--------------------------------------------------------------
CREATE TABLE T_NYUKIN (
    NYUKIN_NO       varchar(10)     NOT NULL,
    NYUKINBI        datetime        NOT NULL,
    SEIKYUSAKI_CD   varchar(10)     NOT NULL,   --請求先の得意先コード
    KINGAKU         decimal(13,0)   NOT NULL,
    HOHO            varchar(10)     NULL,       --現金 振込 手形 相殺 その他
    BIKO            varchar(100)    NULL,
    TORIKESHI_KBN   char(1)         NOT NULL,   --0:通常 1:取消
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_T_NYUKIN PRIMARY KEY (NYUKIN_NO)
)
GO

--------------------------------------------------------------
-- 2011/11/08 山田 タイの得意先マスタ
-- ※現地の得意先コードはCSVに入っているが、会社名はどこにも無い。
--   一覧を出してもらうと年明けになるとのことなので(営業 中村さん)、
--   コードと名前はこちらで登録する
--------------------------------------------------------------
CREATE TABLE M_TH_TOKUISAKI (
    TOKUISAKI_CD    varchar(10)     NOT NULL,   --現地の得意先コード TH-0001 の形
    TOKUISAKI_NM    varchar(40)     NOT NULL,   --こちらで付けた名前
    SHIYO_KBN       char(1)         NOT NULL,   --0:使用中 1:使用しない
    BIKO            varchar(100)    NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_M_TH_TOKUISAKI PRIMARY KEY (TOKUISAKI_CD)
)
GO

--------------------------------------------------------------
-- 2011/11/08 山田 タイの受注 ※現地の生産管理が出すCSVから取り込む
-- 日本の受注(T_JUCHU)とは別に持つ。
-- 単価がCSVに入っていて通貨がバーツのため、日本の納品・締め・請求と
-- 同じ表に混ぜると金額が混ざる。締めをどうするかはまだ決まっていない。
--------------------------------------------------------------
CREATE TABLE T_TH_JUCHU (
    DENPYO_NO       varchar(20)     NOT NULL,   --伝票番号 CSVの11列目 TH00004512 の形
    TOKUISAKI_CD    varchar(10)     NOT NULL,   --CSVの1列目
    JUCHUBI         datetime        NOT NULL,   --CSVの2列目 dd/MM/yyyy
    NOKI            datetime        NULL,       --CSVの3列目
    HINBAN          varchar(20)     NULL,
    HINMEI          varchar(40)     NULL,
    SURYO           int             NOT NULL,
    TANKA           decimal(11,2)   NOT NULL,   --CSVの7列目 ※日本のCSVには無いがこちらには入っている
    TSUKA           varchar(3)      NULL,       --CSVの8列目 THB
    KBN             varchar(2)      NULL,       --CSVの9列目
    JOTAI_KBN       varchar(2)      NULL,       --CSVの10列目
    TORIKOMI_FILE   varchar(40)     NULL,
    TORIKOMI_DT     datetime        NULL,
    KOSHIN_USER     varchar(20)     NULL,
    KOSHIN_DT       datetime        NULL,
    CONSTRAINT PK_T_TH_JUCHU PRIMARY KEY (DENPYO_NO)
)
GO
