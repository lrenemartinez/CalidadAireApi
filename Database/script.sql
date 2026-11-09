CREATE DATABASE IF NOT EXISTS CalidadAireDb;
USE CalidadAireDb;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `SensoresCalidadAire` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Ubicacion` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `TipoGas` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Estado` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_SensoresCalidadAire` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Usuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Correo` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `LecturasAire` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SensorId` int NOT NULL,
    `PM2_5` decimal(10,2) NOT NULL,
    `PM10` decimal(10,2) NOT NULL,
    `CO2` decimal(10,2) NOT NULL,
    `FechaHora` datetime(6) NOT NULL,
    CONSTRAINT `PK_LecturasAire` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LecturasAire_SensoresCalidadAire_SensorId` FOREIGN KEY (`SensorId`) REFERENCES `SensoresCalidadAire` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `AlertasAire` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SensorId` int NOT NULL,
    `LecturaId` int NOT NULL,
    `Nivel` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Mensaje` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    `FechaHora` datetime(6) NOT NULL,
    CONSTRAINT `PK_AlertasAire` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AlertasAire_LecturasAire_LecturaId` FOREIGN KEY (`LecturaId`) REFERENCES `LecturasAire` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_AlertasAire_SensoresCalidadAire_SensorId` FOREIGN KEY (`SensorId`) REFERENCES `SensoresCalidadAire` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AlertasAire_LecturaId` ON `AlertasAire` (`LecturaId`);

CREATE INDEX `IX_AlertasAire_SensorId` ON `AlertasAire` (`SensorId`);

CREATE INDEX `IX_LecturasAire_SensorId_FechaHora` ON `LecturasAire` (`SensorId`, `FechaHora`);

CREATE UNIQUE INDEX `IX_Usuarios_Correo` ON `Usuarios` (`Correo`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261109171801_Inicial', '10.0.14');

COMMIT;

