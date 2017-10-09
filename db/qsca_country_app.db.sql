BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS `qscam_tbl` (
	`id`	TEXT,
	`full`	TEXT NOT NULL,
	`desc`	TEXT,
	PRIMARY KEY(`id`)
);
CREATE TABLE IF NOT EXISTS `qsca_unsd_sids` (
	`m49`	INTEGER NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS `qsca_unsd_lldc` (
	`m49`	INTEGER NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS `qsca_unsd_ldc` (
	`m49`	INTEGER NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS `qsca_unsd_dvlpng` (
	`m49`	INTEGER NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS `qsca_unsd_dvlpd` (
	`m49`	INTEGER NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS `qsca_svrgn_rgn` (
	`m49`	INTEGER NOT NULL UNIQUE,
	`nm`	TEXT NOT NULL UNIQUE,
	`pm49`	INTEGER,
	`flgtyp`	INTEGER NOT NULL,
	`iso31661a2`	TEXT NOT NULL UNIQUE,
	`iso31661a3`	TEXT NOT NULL UNIQUE,
	`iso31661a2styr`	NUMERIC,
	`cctld`	TEXT NOT NULL UNIQUE,
	`genc2a`	TEXT NOT NULL UNIQUE,
	`genc3a`	TEXT NOT NULL UNIQUE,
	`lngtd`	REAL NOT NULL UNIQUE,
	`lttd`	REAL NOT NULL UNIQUE,
	`hgptkm`	REAL,
	`hgptnm`	TEXT,
	`lwptkm`	REAL,
	`lwptnm`	TEXT,
	`prmrysubdivtyp`	TEXT,
	`prmrysubdivcnt`	INTEGER,
	`areakm2`	REAL,
	PRIMARY KEY(`m49`)
);
CREATE TABLE IF NOT EXISTS `qsca_prmry_subdivs` (
	`iso31661a2`	TEXT NOT NULL,
	`iso31662a3`	TEXT NOT NULL UNIQUE,
	`hasc`	TEXT NOT NULL UNIQUE,
	`pcfrmt`	TEXT,
	`areakm2`	REAL NOT NULL,
	`cptl`	TEXT
);
CREATE TABLE IF NOT EXISTS `qsca_pc` (
	`iso31661a2`	TEXT NOT NULL,
	`fmt`	TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS `qsca_nsvrgn_rgn` (
	`m49`	INTEGER NOT NULL UNIQUE,
	`nm`	TEXT NOT NULL UNIQUE,
	`pm49`	INTEGER,
	`flgtyp`	INTEGER NOT NULL,
	`iso31661a2`	TEXT NOT NULL UNIQUE,
	`iso31661a3`	TEXT NOT NULL UNIQUE,
	`iso31661a2styr`	NUMERIC,
	`cctld`	TEXT NOT NULL UNIQUE,
	`genc2a`	TEXT NOT NULL UNIQUE,
	`genc3a`	TEXT NOT NULL UNIQUE,
	`lngtd`	REAL NOT NULL UNIQUE,
	`lttd`	REAL NOT NULL UNIQUE,
	`hgptkm`	REAL,
	`hgptnm`	TEXT,
	`lwptkm`	REAL,
	`lwptnm`	TEXT,
	`prmrysubdivtyp`	TEXT,
	`prmrysubdivcnt`	INTEGER,
	`areakm2`	REAL,
	PRIMARY KEY(`m49`)
);
CREATE TABLE IF NOT EXISTS `qsca_lgsltr` (
	`iso31661a2`	TEXT NOT NULL,
	`prlmntnm`	TEXT,
	`unicameral`	INTEGER,
	`lwrhsnm`	TEXT NOT NULL,
	`lwrhscnt`	INTEGER NOT NULL,
	`lwrtrm`	INTEGER,
	`uprhsnm`	TEXT,
	`uprhscnt`	INTEGER,
	`uprtrm`	INTEGER
);
CREATE TABLE IF NOT EXISTS `qsca_curr_cntry` (
	`iso4217`	TEXT NOT NULL,
	`iso31661a2`	TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS `qsca_crncy` (
	`iso4217`	TEXT NOT NULL UNIQUE,
	`nm`	TEXT NOT NULL UNIQUE,
	`symbl`	TEXT,
	`mnruntnm`	TEXT,
	`cntrlbnk`	TEXT
);
CREATE TABLE IF NOT EXISTS `qsca_cptls` (
	`nm`	TEXT NOT NULL UNIQUE,
	`iso31661a2`	TEXT NOT NULL,
	`lngtd`	REAL NOT NULL,
	`lttd`	REAL NOT NULL,
	`typ`	INTEGER,
	PRIMARY KEY(`nm`)
);
CREATE TABLE IF NOT EXISTS `qsca_cptl_nm` (
	`id`	TEXT NOT NULL,
	`iso6391`	TEXT NOT NULL,
	`nm`	TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS `qsca_cntr_nm` (
	`iso31661a2`	TEXT NOT NULL,
	`iso6391`	TEXT NOT NULL,
	`nm`	TEXT NOT NULL
);
COMMIT;
