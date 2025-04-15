CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Obras` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` longtext CHARACTER SET utf8mb4 NULL,
    `Endereco` longtext CHARACTER SET utf8mb4 NULL,
    `Cliente` longtext CHARACTER SET utf8mb4 NULL,
    `NumeroContrato` longtext CHARACTER SET utf8mb4 NULL,
    `DataInicio` datetime(6) NOT NULL,
    `DataTerminoPrevista` datetime(6) NULL,
    `EngenheiroResponsavel` longtext CHARACTER SET utf8mb4 NULL,
    `Status` longtext CHARACTER SET utf8mb4 NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `DataAtualizacao` datetime(6) NULL,
    CONSTRAINT `PK_Obras` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `RegistroDiarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Data` datetime(6) NOT NULL,
    `ObraId` int NOT NULL,
    `Resumo` longtext CHARACTER SET utf8mb4 NULL,
    `CondicoesClimaticas` int NOT NULL,
    `TotalFuncionarios` int NOT NULL,
    `TotalTerceirizados` int NOT NULL,
    `AssinaturaResponsavel` longtext CHARACTER SET utf8mb4 NULL,
    `DataAssinatura` datetime(6) NULL,
    `Aprovado` tinyint(1) NOT NULL,
    `DataCriacao` datetime(6) NOT NULL,
    `CriadoPor` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RegistroDiarios` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RegistroDiarios_Obras_ObraId` FOREIGN KEY (`ObraId`) REFERENCES `Obras` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `FotoRegistros` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Descricao` longtext CHARACTER SET utf8mb4 NULL,
    `CaminhoArquivo` longtext CHARACTER SET utf8mb4 NULL,
    `DataUpload` datetime(6) NOT NULL,
    `RegistroDiarioId` int NOT NULL,
    `Tipo` int NOT NULL,
    `Localizacao` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_FotoRegistros` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_FotoRegistros_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_FotoRegistros_RegistroDiarioId` ON `FotoRegistros` (`RegistroDiarioId`);

CREATE INDEX `IX_RegistroDiarios_ObraId` ON `RegistroDiarios` (`ObraId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250401152434_MigracaoInicial', '8.0.0');

COMMIT;

START TRANSACTION;

UPDATE `RegistroDiarios` SET `Resumo` = ''
WHERE `Resumo` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `RegistroDiarios` MODIFY COLUMN `Resumo` varchar(300) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Obras` MODIFY COLUMN `NumeroContrato` varchar(30) CHARACTER SET utf8mb4 NULL;

UPDATE `Obras` SET `Nome` = ''
WHERE `Nome` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Obras` MODIFY COLUMN `Nome` varchar(80) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `Obras` SET `EngenheiroResponsavel` = ''
WHERE `EngenheiroResponsavel` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Obras` MODIFY COLUMN `EngenheiroResponsavel` varchar(80) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `Obras` SET `Endereco` = ''
WHERE `Endereco` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Obras` MODIFY COLUMN `Endereco` varchar(150) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `Obras` SET `Cliente` = ''
WHERE `Cliente` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `Obras` MODIFY COLUMN `Cliente` varchar(80) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `FotoRegistros` SET `Localizacao` = ''
WHERE `Localizacao` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `FotoRegistros` MODIFY COLUMN `Localizacao` varchar(50) CHARACTER SET utf8mb4 NOT NULL;

UPDATE `FotoRegistros` SET `Descricao` = ''
WHERE `Descricao` IS NULL;
SELECT ROW_COUNT();


ALTER TABLE `FotoRegistros` MODIFY COLUMN `Descricao` varchar(80) CHARACTER SET utf8mb4 NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250401153307_AjusteTabela', '8.0.0');

COMMIT;

START TRANSACTION;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250407133723_criaTabelasIdentity', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `AspNetUsers` ADD `RefreshToken` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `AspNetUsers` ADD `RefreshTokenExpiryTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250407170203_ajusteApplicationUser', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` DROP COLUMN `Aprovado`;

CREATE INDEX `IX_RegistroDiarios_Data` ON `RegistroDiarios` (`Data`);

CREATE INDEX `IX_RegistroDiarios_ObraId_Data` ON `RegistroDiarios` (`ObraId`, `Data`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409012610_AddIndicesToRegistroDiario', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `AspNetUsers` ADD `Nome` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409165728_CorrigirRelacionamentoEmpresa', '8.0.0');

COMMIT;

START TRANSACTION;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409183048_CreateEmpresaTable', '8.0.0');

COMMIT;

START TRANSACTION;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409184349_CreateEmpresaTableNew', '8.0.0');

COMMIT;

START TRANSACTION;

CREATE TABLE `Empresas` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CriadoEm` datetime(6) NOT NULL,
    CONSTRAINT `PK_Empresas` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409185704_CreateEmpresaTableNewTest', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `AspNetUsers` ADD `EmpresaId` int NOT NULL DEFAULT 0;

CREATE INDEX `IX_AspNetUsers_EmpresaId` ON `AspNetUsers` (`EmpresaId`);

ALTER TABLE `AspNetUsers` ADD CONSTRAINT `FK_AspNetUsers_Empresas_EmpresaId` FOREIGN KEY (`EmpresaId`) REFERENCES `Empresas` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409191941_CreateEmpresaUser', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `Obras` ADD `EmpresaId` int NOT NULL DEFAULT 0;

CREATE INDEX `IX_Obras_EmpresaId` ON `Obras` (`EmpresaId`);

ALTER TABLE `Obras` ADD CONSTRAINT `FK_Obras_Empresas_EmpresaId` FOREIGN KEY (`EmpresaId`) REFERENCES `Empresas` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250409192112_CreateEmpresaObraId', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` DROP COLUMN `CriadoPor`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250410005646_updateRegistroDiario', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `Obras` MODIFY COLUMN `Status` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250410152639_updateStatusEnum', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` ADD `AreaExecutada` decimal(65,30) NOT NULL DEFAULT 0.0;

ALTER TABLE `RegistroDiarios` ADD `ConsumoCimento` int NOT NULL DEFAULT 0;

ALTER TABLE `RegistroDiarios` ADD `Equipamentos` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RegistroDiarios` ADD `Etapa` int NOT NULL DEFAULT 0;

ALTER TABLE `RegistroDiarios` ADD `HorasTrabalhadas` int NOT NULL DEFAULT 0;

ALTER TABLE `RegistroDiarios` ADD `Ocorrencias` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RegistroDiarios` ADD `PercentualConcluido` int NOT NULL DEFAULT 0;

ALTER TABLE `RegistroDiarios` ADD `Precipitacao` decimal(65,30) NULL;

ALTER TABLE `RegistroDiarios` ADD `Temperatura` decimal(65,30) NULL;

CREATE TABLE `DocumentoRegistro` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `NomeArquivo` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Tipo` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CaminhoArquivo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `RegistroDiarioId` int NOT NULL,
    CONSTRAINT `PK_DocumentoRegistro` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_DocumentoRegistro_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `MaterialUtilizado` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `RegistroDiarioId` int NOT NULL,
    CONSTRAINT `PK_MaterialUtilizado` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_MaterialUtilizado_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_DocumentoRegistro_RegistroDiarioId` ON `DocumentoRegistro` (`RegistroDiarioId`);

CREATE INDEX `IX_MaterialUtilizado_RegistroDiarioId` ON `MaterialUtilizado` (`RegistroDiarioId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250410184044_AdicionarCamposRegistroDiario', '8.0.0');

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

ALTER TABLE `DocumentoRegistro` DROP FOREIGN KEY `FK_DocumentoRegistro_RegistroDiarios_RegistroDiarioId`;

ALTER TABLE `MaterialUtilizado` DROP FOREIGN KEY `FK_MaterialUtilizado_RegistroDiarios_RegistroDiarioId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'MaterialUtilizado');
ALTER TABLE `MaterialUtilizado` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'DocumentoRegistro');
ALTER TABLE `DocumentoRegistro` DROP PRIMARY KEY;

ALTER TABLE `MaterialUtilizado` RENAME `MaterialUtilizados`;

ALTER TABLE `DocumentoRegistro` RENAME `DocumentoRegistros`;

ALTER TABLE `MaterialUtilizados` RENAME INDEX `IX_MaterialUtilizado_RegistroDiarioId` TO `IX_MaterialUtilizados_RegistroDiarioId`;

ALTER TABLE `DocumentoRegistros` RENAME INDEX `IX_DocumentoRegistro_RegistroDiarioId` TO `IX_DocumentoRegistros_RegistroDiarioId`;

ALTER TABLE `MaterialUtilizados` ADD CONSTRAINT `PK_MaterialUtilizados` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'MaterialUtilizados', 'Id');

ALTER TABLE `DocumentoRegistros` ADD CONSTRAINT `PK_DocumentoRegistros` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'DocumentoRegistros', 'Id');

ALTER TABLE `DocumentoRegistros` ADD CONSTRAINT `FK_DocumentoRegistros_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE;

ALTER TABLE `MaterialUtilizados` ADD CONSTRAINT `FK_MaterialUtilizados_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250410184432_AdicionarCamposRegistroDiarioss', '8.0.0');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` ADD `Titulo` longtext CHARACTER SET utf8mb4 NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250411184953_addTitleRegistroDiario', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `MaterialUtilizados` ADD `Quantidade` decimal(65,30) NOT NULL DEFAULT 0.0;

ALTER TABLE `MaterialUtilizados` ADD `Unidade` varchar(20) CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413152009_AddQuantidadeUnidadeToMaterial', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `MaterialUtilizados` MODIFY COLUMN `Quantidade` decimal(18,2) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413153401_AddQuantidadeUnidadeToMaterialupdate', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` DROP COLUMN `TotalFuncionarios`;

ALTER TABLE `RegistroDiarios` DROP COLUMN `TotalTerceirizados`;

CREATE TABLE `MembroEquipe` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Cargo` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Observacao` varchar(200) CHARACTER SET utf8mb4 NULL,
    `Terceirizado` tinyint(1) NOT NULL,
    `RegistroDiarioId` int NOT NULL,
    CONSTRAINT `PK_MembroEquipe` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_MembroEquipe_RegistroDiarios_RegistroDiarioId` FOREIGN KEY (`RegistroDiarioId`) REFERENCES `RegistroDiarios` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_MembroEquipe_RegistroDiarioId` ON `MembroEquipe` (`RegistroDiarioId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413163820_AdicionarMembroEquipe', '8.0.0');

COMMIT;

START TRANSACTION;

ALTER TABLE `RegistroDiarios` MODIFY COLUMN `AreaExecutada` decimal(18,2) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413235521_updateAreaexecutada', '8.0.0');

COMMIT;

