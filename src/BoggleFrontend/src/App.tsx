import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./styles/App.css";
import { useDarkMode } from "./DarkModeContext";

function formatTime(seconds: number): string {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  const formattedMinutes = String(minutes).padStart(2, "0");
  const formattedSeconds = String(remainingSeconds).padStart(2, "0");
  return `${formattedMinutes}:${formattedSeconds}`;
}

function App() {
  const placeholderLetters = Array(16).fill("X"); // Assuming a 4x4 board
  const [letters, setLetters] = useState(placeholderLetters);
  const [clickedCells, setClickedCells] = useState<Array<boolean>>([]);
  const [clickedLetters, setClickedLetters] = useState<string>("");
  const [clickedIndices, setClickedIndices] = useState<number[]>([]);
  const [completedWords, setCompletedWords] = useState<string[]>([]);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [gameMode, setGameMode] = useState<string>("timed");
  const [remainingTime, setRemainingTime] = useState<number>(60);
  const [gameStarted, setGameStarted] = useState<boolean>(false);
  const { darkMode } = useDarkMode();
  const [userId] = useState(localStorage.getItem('userId'));

  const navigate = useNavigate();
  
  useEffect(() => {
    if (gameMode !== "timed") {
      handleStartGame();
    }
  }, [gameMode]);

  useEffect(() => {
    let timer;
    if (gameStarted && gameMode === "timed" && remainingTime > 0) {
      timer = setInterval(() => {
        setRemainingTime((prevTime) => prevTime - 1);
      }, 1000);
    } else if (remainingTime <= 0 && gameMode === "timed") {
      setErrorMessage("Time's up! You can no longer submit words.");
    }
  
    return () => clearInterval(timer);
  }, [gameStarted, remainingTime, gameMode]);

  const resetLetters = async () => {
    if (!gameStarted && gameMode === "timed") return;
    setClickedLetters("");
    setCompletedWords([]);
    setErrorMessage("");
    setClickedIndices([]);
    setRemainingTime(60);
    setLetters(placeholderLetters);
    setGameStarted(false);
  };

  const getLetters = async () => {
    try {
      const shuffleResponse = await fetch("http://localhost:5189/api/Boggle/shuffle", {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });
      if (!shuffleResponse.ok) throw new Error("Failed to get board");
      const data = await shuffleResponse.json();
      return data;
    } catch (error) {
      console.error("Failed to fetch letters: ", error);
    }
  }

  
  

  const handleClick = async (letter: string, index: number) => {
    if ((gameMode === "timed" && !gameStarted) || remainingTime <= 0) {
      return;
    }

    if (clickedIndices.includes(index)) {
      const word = clickedLetters;
      // API call to validate the word
      try {
        const response = await fetch('http://localhost:5189/api/WordInfo/isValidWord', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(word) // Send the word as a JSON string
        });
        if (!response.ok) {
          throw new Error(`Failed to validate word with status: ${response.status}`);
        }
        const isValid = await response.json(); // The response is expected to be a simple true or false

        console.log("Validation result:", isValid);
        if (isValid) {
          if (completedWords.includes(word)) {
            setErrorMessage(`The word "${word}" has already been found!`);
          } else {
            setCompletedWords(prevCompletedWords => [
              ...prevCompletedWords,
              word,
            ]);
            setErrorMessage("");
          }
        }
      } catch (error) {
        console.error("Error validating word:", error);
      }
      // Reset after submitting the word
      setClickedLetters("");
      setClickedIndices([]);
      setClickedCells(new Array(letters.length).fill(false));
    } else {
      // Handling letter selection for adjacent or diagonal letters
      handleLetterSelection(letter, index);
    }
  };

  const handleLetterSelection = (letter, index) => {
    // Ensure letter clicked is adjacent or diagonal
    if (clickedIndices.length > 0) {
      const prevIndex = clickedIndices[clickedIndices.length - 1];
      const prevRowIndex = Math.floor(prevIndex / 4);
      const prevColIndex = prevIndex % 4;
      const currRowIndex = Math.floor(index / 4);
      const currColIndex = index % 4;

      const rowDifference = Math.abs(currRowIndex - prevRowIndex);
      const colDifference = Math.abs(currColIndex - prevColIndex);

    if (rowDifference > 1 || colDifference > 1) {
      setErrorMessage("Select a letter that is adjacent or diagonal to the previously clicked letter.");
      return;
    }
  }

  setClickedLetters(clickedLetters + letter);
  setClickedIndices([...clickedIndices, index]);
  setClickedCells(clickedCells.map((val, idx) => idx === index ? true : val));
  setErrorMessage("");
};

  const handleLogin = () => {
    if (userId) {
      navigate('/profile');
    } else {
      navigate('/login');
    }
  };

  const handleStartGame = async () => {
    const data = await getLetters();
    setLetters(data);
    setClickedCells(new Array(data.length).fill(false));
    setGameStarted(true);
    setRemainingTime(60);
  };

  const toggleGameMode = async () => {
    if (gameMode === "timed") {
      setGameMode("untimed");
      resetLetters();
    } else {
      setGameMode("timed");
      setRemainingTime(60);
      setLetters(placeholderLetters);
      setGameStarted(false);
      resetLetters();
    }
  };

  return (
    <div className={darkMode ? "dark-mode" : "light-mode"}>
      <div className="header">
        <button id="multiplayer" onClick={() => navigate('/multiplayer')}>Multiplayer</button>
        <h2>Boggle</h2>
        <button id="login" onClick={handleLogin}>
          {userId ? 'Profile' : 'Login'}
        </button>
      </div>
      <div id="boggle-container">
        <div id="word-curr">
          <h2>Current word: {clickedLetters}</h2>
        </div>
        <div id="board">
          {letters.map((letter, index) => (
            <div
              key={index}
              className="cell"
              id={clickedCells[index] ? "two" : "one"}
              onClick={() => handleClick(letter, index)}
            >
              {letter}
            </div>
          ))}
        </div>
        <div className="button-container">
          <button id="gamemode-button" onClick={toggleGameMode}>
            {gameMode === "timed" ? "Untimed" : "Timed"}
          </button>
          {gameMode === "timed" && (
            <button id="start-button" onClick={handleStartGame}>
              Start Game
            </button>
          )}
          <button id="reset-button" onClick={resetLetters}>
            Reset
          </button>
        </div>
        {gameMode === "timed" && (
          <div id="timer">{formatTime(remainingTime)}</div>
        )}
        <div id="word-list">
          <h2>Words Found</h2>
          <ul id="words-found">
            {completedWords.map((word, index) => (
              <li key={index}>{word}</li>
            ))}
          </ul>
          {errorMessage && <h3 className="error-message">{errorMessage}</h3>}
        </div>
      </div>
      <div className="footer">
        <a
          href="https://github.com/Kashish-Syed/Boggle"
          className="github-link"
        >
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png"
            alt="GitHub"
            style={{
              height: "25px",
              marginRight: "5px",
              verticalAlign: "middle",
            }}
          />
        </a>
      </div>
    </div>
  );
}
export default App;
