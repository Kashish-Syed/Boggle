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




INSERT INTO Player (Username, Password) VALUES ('admin', 'password');

INSERT INTO Game (GameCode, Board) VALUES ('GAME1', 'ABCDEFGHIJKLMNOP');
INSERT INTO Game (GameCode, Board) VALUES ('GAME2', 'QRSTUVWXYZABCDEF');
INSERT INTO Game (GameCode, Board) VALUES ('GAME3', 'GHIJKLMNOPQRSTUV');
INSERT INTO Game (GameCode, Board) VALUES ('GAME4', 'WXYZABCDEFGHIJK');
INSERT INTO Game (GameCode, Board) VALUES ('GAME5', 'LMNOPQRSTUVWXYZ');

INSERT INTO Word (Word, Points) VALUES ('apple', 2);
INSERT INTO Word (Word, Points) VALUES ('banana', 2);
INSERT INTO Word (Word, Points) VALUES ('cherry', 2);
INSERT INTO Word (Word, Points) VALUES ('date', 2);
INSERT INTO Word (Word, Points) VALUES ('elderberry', 2);
INSERT INTO Word (Word, Points) VALUES ('fig', 2);
INSERT INTO Word (Word, Points) VALUES ('grape', 2);
INSERT INTO Word (Word, Points) VALUES ('honeydew', 2);
INSERT INTO Word (Word, Points) VALUES ('kiwi', 2);
INSERT INTO Word (Word, Points) VALUES ('lemon', 2);
INSERT INTO Word (Word, Points) VALUES ('mango', 2);
INSERT INTO Word (Word, Points) VALUES ('nectarine', 2);
INSERT INTO Word (Word, Points) VALUES ('orange', 2);
INSERT INTO Word (Word, Points) VALUES ('papaya', 2);
INSERT INTO Word (Word, Points) VALUES ('quince', 2);

INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME1', 4, 7);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME1', 4, 8);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME1', 4, 9);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME2', 4, 10);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME2', 4, 11);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME2', 4, 12);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME3', 4, 13);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME3', 4, 14);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME3', 4, 15);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME4', 4, 16);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME4', 4, 17);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME4', 4, 18);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME5', 4, 19);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME5', 4, 20);
INSERT INTO GameWord (GameCode, PlayerID, WordID) VALUES ('GAME5', 4, 21);

INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('GAME1', 4, 6); 
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('GAME2', 4, 6);
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('GAME3', 4, 6);
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('GAME4', 4, 6);
INSERT INTO GamePlayer (GameCode, PlayerID, TotalScore) VALUES ('GAME5', 4, 6);
