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
