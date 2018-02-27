CREATE TABLE `Rooms` (
	`id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar NOT NULL,
	`Short Description` varchar NOT NULL,
	`Full Description` varchar NOT NULL,
	`Light` bool NOT NULL,
	`Commands` varchar NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE `Items` (
	`` bigint NOT NULL,
	`id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar NOT NULL AUTO_INCREMENT,
	`Type` varchar NOT NULL,
	`Special` varchar NOT NULL,
	`Magic` bool NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE `PCs` (
	`id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar NOT NULL,
	`Type` varchar NOT NULL,
	`HP` int NOT NULL AUTO_INCREMENT,
	`AC` int NOT NULL,
	`Damage` int NOT NULL,
	`LVL` int NOT NULL,
	`EXP` int NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE `NPCs` (
	`id` bigint NOT NULL AUTO_INCREMENT,
	`Name` varchar NOT NULL,
	`Type` varchar NOT NULL,
	`HP` int NOT NULL AUTO_INCREMENT,
	`AC` int NOT NULL,
	`Damage` int NOT NULL,
	`LVL` int NOT NULL,
	PRIMARY KEY (`id`)
);

CREATE TABLE `Inventory` (
	`Items` bigint NOT NULL,
	`PCs` bigint NOT NULL
);

CREATE TABLE `Loot` (
	`Items` bigint NOT NULL,
	`NPCs` bigint NOT NULL
);

CREATE TABLE `Contents` (
	`RoomId` bigint NOT NULL,
	`NPCs` bigint NOT NULL,
	`PCs` bigint NOT NULL,
	`Items` bigint NOT NULL
);

ALTER TABLE `Rooms` ADD CONSTRAINT `Rooms_fk0` FOREIGN KEY (`Commands`) REFERENCES `Contents`(``);

ALTER TABLE `Inventory` ADD CONSTRAINT `Inventory_fk0` FOREIGN KEY (`Items`) REFERENCES `Items`(``);

ALTER TABLE `Inventory` ADD CONSTRAINT `Inventory_fk1` FOREIGN KEY (`PCs`) REFERENCES `PCs`(`id`);

ALTER TABLE `Loot` ADD CONSTRAINT `Loot_fk0` FOREIGN KEY (`Items`) REFERENCES `Items`(`id`);

ALTER TABLE `Loot` ADD CONSTRAINT `Loot_fk1` FOREIGN KEY (`NPCs`) REFERENCES `NPCs`(`id`);

ALTER TABLE `Contents` ADD CONSTRAINT `Contents_fk0` FOREIGN KEY (`RoomId`) REFERENCES `Rooms`(`id`);

ALTER TABLE `Contents` ADD CONSTRAINT `Contents_fk1` FOREIGN KEY (`NPCs`) REFERENCES `NPCs`(`id`);

ALTER TABLE `Contents` ADD CONSTRAINT `Contents_fk2` FOREIGN KEY (`PCs`) REFERENCES `PCs`(`id`);

ALTER TABLE `Contents` ADD CONSTRAINT `Contents_fk3` FOREIGN KEY (`Items`) REFERENCES `Items`(``);
