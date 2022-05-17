--Tabla Jugadores
CREATE TABLE IF NOT EXISTS Jugadores(
		ID INTEGER PRIMARY KEY AUTOINCREMENT,
		NombreUsuario varchar(25) NOT NULL,
		Contrasenha varchar(20) NOT NULL,
		CorreoElectronico varchar(50)NOT NULL,
		Dinero money NOT NULL,
		Foto BLOB NULL
);

--Tabla Tipos
CREATE TABLE IF NOT EXISTS Tipos (
		ID INTEGER PRIMARY KEY AUTOINCREMENT,
		Nombre varchar(15) NOT NULL
);

--Tabla Movimientos
CREATE TABLE IF NOT EXISTS Movimientos (
		MT INTEGER PRIMARY KEY AUTOINCREMENT,
		Nombre varchar(20) NOT NULL,
		Danho smallint NOT NULL,
		Precision smallint NULL,
		PP smallint NOT NULL,
		Tipo INTEGER NOT NULL,

		--Foreing Key
		FOREIGN KEY (Tipo) REFERENCES Tipos(ID)
);

--Tabla PokemonsJugadores
CREATE TABLE IF NOT EXISTS PokemonsJugadores  (
		IDJugador INTEGER NOT NULL,
		IDPokemon INTEGER NOT NULL,
		NumeroPokemon smallint NOT NULL,
		Nombre varchar(30) NOT NULL,
		HP smallint NOT NULL,
		Nivel smallint NOT NULL,
		Ataque smallint NOT NULL,
		Defensa smallint NOT NULL,
		Velocidad smallint NOT NULL,	
		NumeroEquipado smallint DEFAULT 0 NOT NULL,	
		Experiencia int DEFAULT 0 NOT NULL,


		--Primary Key
		PRIMARY KEY(IDJugador,IDPokemon,NumeroPokemon),
		--Foreing Key
		FOREIGN KEY(IDJugador) REFERENCES Jugadores(ID)
);

--Tabla PokemonsEncontradosJugadores
CREATE TABLE IF NOT EXISTS PokemonsEncontradosJugadores(
		IDPokemon INTEGER NOT NULL,
		IDJugador INTEGER NOT NULL,
		NombrePokemon INTEGER NOT NULL,

		--Primary Key
		PRIMARY KEY(IDJugador,IDPokemon),
		--Foregin Key
		FOREIGN KEY(IDJugador) REFERENCES Jugadores(ID)
);

--Tabla PokemonsMovimientosJugadores
CREATE TABLE IF NOT EXISTS PokemonsJugadoresMovimientos (
		IDJugador INTEGER NOT NULL,
		IDPokemon INTEGER NOT NULL,
		NumeroPokemon smallint NOT NULL,
		IDMovimiento smallint NOT NULL,

		--Primary Key
		PRIMARY KEY(IDJugador,IDPokemon,NumeroPokemon,IDMovimiento),
		--Foreing Key
		FOREIGN KEY(IDJugador,IDPokemon,NumeroPokemon) REFERENCES PokemonsJugadores(IDJugador,IDPokemon,NumeroPokemon),
		FOREIGN KEY(IDMovimiento) REFERENCES Movimientos(MT)
);



--Tabla TiposPokemon
CREATE TABLE IF NOT EXISTS TiposPokemons(
		IDPokemon INTEGER NOT NULL,
		IDTipo INTEGER NOT NULL,

		--Primary Key
		PRIMARY KEY(IDPokemon,IDTipo),
		--Foreing Key
		FOREIGN KEY(IDTipo) REFERENCES Tipos(ID)
);


--Tabla TiposTiposDebiles
CREATE TABLE IF NOT EXISTS TiposTiposDebiles(
		IDTipo INTEGER NOT NULL,
		IDTipoDebil INTEGER NOT NULL,

		--Primary Key
		PRIMARY KEY(IDTipo,IDTipoDebil),
		--Foreing Key
		FOREIGN KEY(IDTipo) REFERENCES Tipos(ID),
		FOREIGN KEY(IDTipoDebil) REFERENCES Tipos(ID)
);

--Generalizacion Items que tiene dos subtipos Pociones y pokeballs como items tiene pocas columnas(ID y descripcion) y la relacion es exclusiva(O el item es una pokeball o una pocion) y total(Solo existen esos dos items), se creara una tabla para cada subtipo
--Tabla Items
CREATE TABLE IF NOT EXISTS Items (
		ID INTEGER PRIMARY KEY AUTOINCREMENT,
		Nombre varchar(20) NOT NULL,
		Descripcion varchar(255) NULL,
		IndiceExito smallint NULL,
		CuracionPS smallint NULL,
		Precio smallint,
		Tipo varchar(20) NOT NULL

);

--Tabla ItemsJugador
CREATE TABLE IF NOT EXISTS ItemsJugadores (
		IDItem INTEGER NOT NULL,
		IDJugador INTEGER NOT NULL,
		Cantidad smallint NOT NULL,

		--Primary Key
		PRIMARY KEY(IDItem,IDJugador),
		--Foreing Key
		FOREIGN KEY (IDItem) REFERENCES Items(ID),
		FOREIGN KEY (IDJugador) REFERENCES Jugadores(ID)
);

--Pociones
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('Pocion','Curacion basica que cura 20PS a un pokemon',NULL,20,100,'Pocion');
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('SuperPocion','Curacion normal que cura 50PS a un pokemon',NULL,50,150,'Pocion');
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('HiperPocion','Curacion avanzada que cura 200PS a un pokemon',NULL,200,350,'Pocion');
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('Refresco','Bebida refrescante que le encanta a los pokemons. Recupera 60 PS',NULL,60,200,'Pocion');
--Pokeballs
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('Poke Ball','Dispositivo capsular que sirve para atrapar pokemons salvajes',40,NULL,120,'Pokeball');
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('Super Ball','Poke Ball de alto rendimiento. Tiene un indice de exito superior al de la Poke Ball',55,NULL,250,'Pokeball');
INSERT INTO Items (Nombre,Descripcion,IndiceExito,CuracionPS,Precio,Tipo) Values ('Ultra Ball','Poke Ball de elite. Tiene un indice de exito superior al de la Super Ball',70,NULL,400,'Pokeball');

INSERT INTO Tipos (Nombre) VALUES ('Planta');
INSERT INTO Tipos (Nombre) VALUES ('Fuego');
INSERT INTO Tipos (Nombre) VALUES ('Agua');
INSERT INTO Tipos (Nombre) VALUES ('Normal');
INSERT INTO Tipos (Nombre) VALUES ('Electrico');
INSERT INTO Tipos (Nombre) VALUES ('Psiquico');
INSERT INTO Tipos (Nombre) VALUES ('Lucha');
INSERT INTO Tipos (Nombre) VALUES ('Roca');
INSERT INTO Tipos (Nombre) VALUES ('Tierra');
INSERT INTO Tipos (Nombre) VALUES ('Volador');
INSERT INTO Tipos (Nombre) VALUES ('Bicho');
INSERT INTO Tipos (Nombre) VALUES ('Veneno');
INSERT INTO Tipos (Nombre) VALUES ('Siniestro');
INSERT INTO Tipos (Nombre) VALUES ('Fantasma');
INSERT INTO Tipos (Nombre) VALUES ('Hielo');
INSERT INTO Tipos (Nombre) VALUES ('Acero');
INSERT INTO Tipos (Nombre) VALUES ('Dragon');
INSERT INTO Tipos (Nombre) VALUES ('Hada');

--Planta
INSERT INTO TiposTiposDebiles VALUES (1,2);
INSERT INTO TiposTiposDebiles VALUES (1,10);
INSERT INTO TiposTiposDebiles VALUES (1,11);
INSERT INTO TiposTiposDebiles VALUES (1,12);
INSERT INTO TiposTiposDebiles VALUES (1,15);
--Fuego
INSERT INTO TiposTiposDebiles VALUES (2,3);
INSERT INTO TiposTiposDebiles VALUES (2,8);
INSERT INTO TiposTiposDebiles VALUES (2,9);
--Agua
INSERT INTO TiposTiposDebiles VALUES (3,1);
INSERT INTO TiposTiposDebiles VALUES (3,5);
--Normal
INSERT INTO TiposTiposDebiles VALUES (4,7);
--Electrico
INSERT INTO TiposTiposDebiles VALUES (5,9);
--Psiquico
INSERT INTO TiposTiposDebiles VALUES (6,11);
INSERT INTO TiposTiposDebiles VALUES (6,13);
INSERT INTO TiposTiposDebiles VALUES (6,14);
--Lucha
INSERT INTO TiposTiposDebiles VALUES (7,6);
INSERT INTO TiposTiposDebiles VALUES (7,10);
INSERT INTO TiposTiposDebiles VALUES (7,18);
--Roca
INSERT INTO TiposTiposDebiles VALUES (8,1);
INSERT INTO TiposTiposDebiles VALUES (8,3);
INSERT INTO TiposTiposDebiles VALUES (8,7);
INSERT INTO TiposTiposDebiles VALUES (8,9);
INSERT INTO TiposTiposDebiles VALUES (8,16);
--Tierra
INSERT INTO TiposTiposDebiles VALUES (9,1);
INSERT INTO TiposTiposDebiles VALUES (9,3);
INSERT INTO TiposTiposDebiles VALUES (9,15);
--Volador
INSERT INTO TiposTiposDebiles VALUES (10,5);
INSERT INTO TiposTiposDebiles VALUES (10,8);
INSERT INTO TiposTiposDebiles VALUES (10,15);
--Bicho
INSERT INTO TiposTiposDebiles VALUES (11,2);
INSERT INTO TiposTiposDebiles VALUES (11,8);
INSERT INTO TiposTiposDebiles VALUES (11,10);
--Veneno
INSERT INTO TiposTiposDebiles VALUES (12,6);
INSERT INTO TiposTiposDebiles VALUES (12,9);
--Siniestro
INSERT INTO TiposTiposDebiles VALUES (13,7);
INSERT INTO TiposTiposDebiles VALUES (13,11);
INSERT INTO TiposTiposDebiles VALUES (13,18);
--Fantasma
INSERT INTO TiposTiposDebiles VALUES (14,13);
INSERT INTO TiposTiposDebiles VALUES (14,14);
--Hielo
INSERT INTO TiposTiposDebiles VALUES (15,2);
INSERT INTO TiposTiposDebiles VALUES (15,7);
INSERT INTO TiposTiposDebiles VALUES (15,8);
INSERT INTO TiposTiposDebiles VALUES (15,16);
--Acero
INSERT INTO TiposTiposDebiles VALUES (16,3);
INSERT INTO TiposTiposDebiles VALUES (16,7);
INSERT INTO TiposTiposDebiles VALUES (16,9);
--Dragon
INSERT INTO TiposTiposDebiles VALUES (17,15);
INSERT INTO TiposTiposDebiles VALUES (17,17);
INSERT INTO TiposTiposDebiles VALUES (17,18);
--Hada
INSERT INTO TiposTiposDebiles VALUES (18,13);
INSERT INTO TiposTiposDebiles VALUES (18,16);

--INSERT DE PRUEBA ELIMINAR
INSERT INTO Jugadores (NombreUsuario,Contrasenha,CorreoElectronico,Dinero,Foto) VALUES('a','a','correo',0,NULL);

INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(1,1,'Bulbasaur');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(152,1,'Chikorita');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(252,1,'Treecko');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(405,1,'Luxray');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(495,1,'Snivy');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(650,1,'Chespin');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(722,1,'Rowlet');
INSERT INTO PokemonsEncontradosJugadores (IDPokemon,IDJugador,NombrePokemon) VALUES(810,1,'Grokey');


INSERT INTO PokemonsJugadores VALUES(1,337,1,'PokemonPrueba',1,2,1,4,5,1,1);
INSERT INTO PokemonsJugadores VALUES(1,211,2,'PokemonPrueba2',1,2,2,4,5,2,1);
INSERT INTO PokemonsJugadores VALUES(1,428,3,'PokemonPrueba3',1,2,3,4,5,3,1);
INSERT INTO PokemonsJugadores VALUES(1,211,4,'PokemonPrueba2',1,2,2,4,5,4,1);
INSERT INTO PokemonsJugadores VALUES(1,337,5,'PokemonPrueba',1,2,1,4,5,0,1);

INSERT INTO Movimientos (Nombre,Danho,Precision,PP,Tipo) VALUES('MovimientoPrueba',1,2,3,8);
INSERT INTO Movimientos (Nombre,Danho,Precision,PP,Tipo) VALUES('MovimientoPrueba2',1,2,3,1);
INSERT INTO Movimientos (Nombre,Danho,Precision,PP,Tipo) VALUES('MovimientoPrueba3',1,2,3,14);
INSERT INTO Movimientos (Nombre,Danho,Precision,PP,Tipo) VALUES('MovimientoPrueba4',1,2,3,3);

INSERT INTO PokemonsJugadoresMovimientos VALUES(1,337,1,1);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,337,1,2);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,337,1,3);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,337,1,4);

INSERT INTO PokemonsJugadoresMovimientos VALUES(1,211,2,1);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,211,2,2);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,211,2,3);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,211,2,4);

INSERT INTO PokemonsJugadoresMovimientos VALUES(1,428,3,1);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,428,3,2);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,428,3,3);
INSERT INTO PokemonsJugadoresMovimientos VALUES(1,428,3,4);

INSERT INTO ItemsJugadores VALUES(1,1,20);
INSERT INTO ItemsJugadores VALUES(5,1,20);

INSERT INTO TiposPokemons VALUES (337,5);
INSERT INTO TiposPokemons VALUES (211,8);
INSERT INTO TiposPokemons VALUES (428,10);
