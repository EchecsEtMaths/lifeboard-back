CREATE DATABASE finances;
USE finances;

CREATE TABLE Banque (
	id int DEFAULT NULL,
	nom varchar(255) DEFAULT NULL
);

CREATE TABLE Categorie (
id int DEFAULT NULL,
nom varchar(255) DEFAULT NULL
);


CREATE TABLE Compte (
id int DEFAULT NULL,
nom varchar(255) DEFAULT NULL,
solde_base decimal(10,2) DEFAULT NULL,
plafond decimal(10,2) DEFAULT NULL,
banque_id int DEFAULT NULL
);


CREATE TABLE Transactions (
id int DEFAULT NULL,
nom varchar(255) DEFAULT NULL,
date_transac date DEFAULT NULL,
montant decimal(10,2) DEFAULT NULL,
categorie_id int DEFAULT NULL,
compte_id int DEFAULT NULL
);
