drop table if exists Board;
drop table if exists Game;
drop table if exists Player;
drop table if exists Word;
drop table if exists Score;

CREATE TABLE Player (
    PlayerID INT NOT NULL PRIMARY KEY IDENTITY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL
);

CREATE TABLE Game (
    GameID INT NOT NULL PRIMARY KEY IDENTITY,
    PlayerID INT NOT NULL,
    FOREIGN KEY (PlayerID) REFERENCES Player(PlayerID),
    StartTime DATETIME NOT NULL DEFAULT GETDATE(),
    EndTime DATETIME,
    GameDuration TIME
);

CREATE TABLE Board (
    BoardID INT PRIMARY KEY IDENTITY,
    GameID INT NOT NULL,
    FOREIGN KEY (GameID) REFERENCES Game(GameID),
    Letters VARCHAR(16) NOT NULL
);

CREATE TABLE Word (
    WordID INT PRIMARY KEY IDENTITY,
    GameID INT NOT NULL,
    FOREIGN KEY (GameID) REFERENCES Game(GameID),
    WordText VARCHAR(50),
    WordLength INT NOT NULL
);

CREATE TABLE Score (
    ScoreID INT PRIMARY KEY IDENTITY,
    GameID INT NOT NULL,
    FOREIGN KEY (GameID) REFERENCES Game(GameID),
    PlayerID INT NOT NULL,
    FOREIGN KEY (PlayerID) REFERENCES Player(PlayerID),
    Score INT NOT NULL
);

INSERT INTO Player (FirstName, LastName, Username, Email, Password)
VALUES ('John', 'Doe', 'johndoe', 'john@example.com', 'hashed_password1'),
       ('Jane', 'Smith', 'janesmith', 'jane@example.com', 'hashed_password2');

INSERT INTO Game (PlayerID, StartTime, EndTime, GameDuration)
VALUES (1, '2024-04-10 18:00:00', '2024-04-10 18:15:00', '00:15:00'),
       (2, '2024-04-11 10:30:00', NULL, NULL);

INSERT INTO Board (GameID, Letters)
VALUES (1, 'TESTTESTTESTTEST'),
       (2, 'TESTTESTTESTTEST');

INSERT INTO Word (GameID, WordText, WordLength)
VALUES (1, 'CAT', 3),
       (1, 'DOG', 3),
       (2, 'TEST', 4);

INSERT INTO Score (GameID, PlayerID, Score)
VALUES (1, 1, 50),
       (1, 2, 40),
       (2, 1, 60);