DROP TABLE IF EXISTS GamePlayer;
DROP TABLE IF EXISTS GameWord;
DROP TABLE IF EXISTS Word;
DROP TABLE IF EXISTS Game;
DROP TABLE IF EXISTS Player;

CREATE TABLE Player (
    PlayerID INT NOT NULL PRIMARY KEY IDENTITY,
    Username VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL
);
go

CREATE TABLE Game (
    GameCode VARCHAR(6) NOT NULL PRIMARY KEY,
    Board VARCHAR(16) NOT NULL,
    CONSTRAINT CHK_GameCode_Alphanumeric CHECK (GameCode NOT LIKE '%[^0-9A-Za-z]%')
);
go

CREATE TABLE Word (
    WordID INT NOT NULL PRIMARY KEY IDENTITY,
    Word varchar(25) NOT NULL,
    Points INT NOT NULL, 
    CONSTRAINT CHK_Word_Alpha CHECK (Word NOT LIKE '%[^A-Za-z]%')
);
go

CREATE TABLE GameWord (
    GameWordID INT NOT NULL PRIMARY KEY IDENTITY,
    GameCode VARCHAR(6) NOT NULL,
    PlayerID INT NOT NULL,
    WordID INT NOT NULL,
    FOREIGN KEY (GameCode) REFERENCES Game(GameCode),
    FOREIGN KEY (WordID) REFERENCES Word(WordID),
    FOREIGN KEY (PlayerID) REFERENCES Player(PlayerID)
);
go

CREATE TABLE GamePlayer (
    GamePlayerID INT NOT NULL PRIMARY KEY IDENTITY,
    GameCode VARCHAR(6) NOT NULL,
    PlayerID INT NOT NULL,
    TotalScore INT NOT NULL,
    FOREIGN KEY (GameCode) REFERENCES Game(GameCode),
    FOREIGN KEY (PlayerID) REFERENCES Player(PlayerID)
);
go

INSERT INTO Player (Username, Password) VALUES ('Alice123', 'password1');
INSERT INTO Player (Username, Password) VALUES ('Bob234', 'password2');
INSERT INTO Player (Username, Password) VALUES ('Charlie345', 'password3');

INSERT INTO Game (GameCode, Board) VALUES ('ABC123', 'ABCDEFGHIJKLMNO');
INSERT INTO Game (GameCode, Board) VALUES ('XYZ789', 'PQRSTUVWXYZABC');

INSERT INTO Word (Word, Points) VALUES ('hello', 2);
INSERT INTO Word (Word, Points) VALUES ('world', 2);
INSERT INTO Word (Word, Points) VALUES ('candy', 2);
INSERT INTO Word (Word, Points) VALUES ('silly', 2);
INSERT INTO Word (Word, Points) VALUES ('quick', 2);
INSERT INTO Word (Word, Points) VALUES ('brown', 2);

INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('ABC123', 1, 1);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('ABC123', 2, 2);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('ABC123', 2, 3);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('ABC123', 2, 4);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('XYZ789', 2, 5);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('XYZ789', 3, 6);

INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('ABC123', 1, 5); 
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('ABC123', 2, 20);
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('XYZ789', 2, 10); 
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('XYZ789', 3, 8);
