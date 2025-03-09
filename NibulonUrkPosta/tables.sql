CREATE TABLE OBL (
                     OBL SMALLINT IDENTITY(1,1) PRIMARY KEY,
                     NOBL NVARCHAR(200) NOT NULL
);

CREATE TABLE RAJ (
                     KRAJ INT IDENTITY(1,1) PRIMARY KEY,
                     RAJ NVARCHAR(200) NOT NULL
);

CREATE TABLE CITY (
                      CITY_KOD INT IDENTITY(1,1) PRIMARY KEY,
                      CITY NVARCHAR(50) NOT NULL,
                      KRAJ INT NULL,
                      OBL SMALLINT NULL,
                      FOREIGN KEY (OBL) REFERENCES OBL(OBL) ON DELETE NO ACTION,
                      FOREIGN KEY (KRAJ) REFERENCES RAJ(KRAJ) ON DELETE NO ACTION
);

CREATE TABLE AUP (
                     ID INT IDENTITY(1,1) PRIMARY KEY,
                     INDEX_A NVARCHAR(6) NOT NULL,
                     CITY INT NOT NULL,
                     NCITY NVARCHAR(200) NOT NULL,
                     OBL SMALLINT NULL,
                     NOBL NVARCHAR(200) NOT NULL,
                     RAJ INT NULL,
                     NRAJ NVARCHAR(255) NOT NULL,
                     FOREIGN KEY (CITY) REFERENCES CITY(CITY_KOD) ON DELETE CASCADE,
                     FOREIGN KEY (OBL) REFERENCES OBL(OBL) ON DELETE SET NULL
);
