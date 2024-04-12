drop table if exists Player;
drop table if exists Game;
drop table if exists Board;
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
    -- Assuming a 4x4 Board
    Letter1 VARCHAR(1) NOT NULL,
    Letter2 VARCHAR(1) NOT NULL,
    Letter3 VARCHAR(1) NOT NULL,
    Letter4 VARCHAR(1) NOT NULL,
    Letter5 VARCHAR(1) NOT NULL,
    Letter6 VARCHAR(1) NOT NULL,
    Letter7 VARCHAR(1) NOT NULL,
    Letter8 VARCHAR(1) NOT NULL,
    Letter9 VARCHAR(1) NOT NULL,
    Letter10 VARCHAR(1) NOT NULL,
    Letter11 VARCHAR(1) NOT NULL,
    Letter12 VARCHAR(1) NOT NULL,
    Letter13 VARCHAR(1) NOT NULL,
    Letter14 VARCHAR(1) NOT NULL,
    Letter15 VARCHAR(1) NOT NULL,
    Letter16 VARCHAR(1) NOT NULL
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

-- Insert test data into Player table
INSERT INTO Player (FirstName, LastName, Username, Email, Password)
VALUES ('John', 'Doe', 'johndoe', 'john@example.com', 'hashed_password1'),
       ('Jane', 'Smith', 'janesmith', 'jane@example.com', 'hashed_password2');

-- Insert test data into Game table
INSERT INTO Game (PlayerID, StartTime, EndTime, GameDuration)
VALUES (1, '2024-04-10 18:00:00', '2024-04-10 18:15:00', '00:15:00'),
       (2, '2024-04-11 10:30:00', NULL, NULL);

-- Insert test data into Board table
INSERT INTO Board (GameID, Letter1, Letter2, Letter3, Letter4, Letter5, Letter6, Letter7, Letter8, Letter9, Letter10, Letter11, Letter12, Letter13, Letter14, Letter15, Letter16)
VALUES (1, 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P'),
       (2, 'T', 'E', 'S', 'T', 'T', 'E', 'S', 'T', 'T', 'E', 'S', 'T', 'T', 'E', 'S', 'T');

-- Insert test data into Word table
INSERT INTO Word (GameID, WordText, WordLength)
VALUES (1, 'CAT', 3),
       (1, 'DOG', 3),
       (2, 'TEST', 4);

-- Insert test data into Score table
INSERT INTO Score (GameID, PlayerID, Score)
VALUES (1, 1, 50),
       (1, 2, 40),
       (2, 1, 60);
