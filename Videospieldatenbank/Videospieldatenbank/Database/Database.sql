﻿CREATE TABLE games(
	PRIMARY KEY (igdb_url),
	igdb_url VARCHAR(512),
	cover_url VARCHAR(512),
	name VARCHAR(512),
	developer VARCHAR(512) ,
	plattforms VARCHAR(512),
	genres VARCHAR(512),
	rating INTEGER
);

CREATE TABLE user(
	PRIMARY KEY (id),
	id INTEGER AUTO_INCREMENT,
	name VARCHAR(128),
	password VARCHAR(128),
	online BOOLEAN,
	picture MEDIUMBLOB
);

CREATE TABLE friends(
	PRIMARY KEY (user_ID, friend_ID),
	user_ID INTEGER,
	friend_ID INTEGER
);

CREATE TABLE gameinfo(
	PRIMARY KEY (user_ID, igdb_url),
	user_ID INTEGER,
	igdb_url VARCHAR(512),
	exec_path VARCHAR(512),
	playtime INTEGER 
);
