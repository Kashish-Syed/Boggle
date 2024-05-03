import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import "./styles/App.css";
import { useDarkMode } from "./DarkModeContext";
import validateInput from "./Component";

function formatTime(seconds: number): string {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = seconds % 60;
  const formattedMinutes = String(minutes).padStart(2, "0");
  const formattedSeconds = String(remainingSeconds).padStart(2, "0");
  return `${formattedMinutes}:${formattedSeconds}`;
}

function App() {
  const [letters, setLetters] = useState([]);
  const [clickedCells, setClickedCells] = useState<Array<boolean>>([]);
  const [clickedLetters, setClickedLetters] = useState<string>("");
  const [clickedIndices, setClickedIndices] = useState<number[]>([]);
  const [completedWords, setCompletedWords] = useState<string[]>([]);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [gameMode, setGameMode] = useState<string>("timed");
  const [remainingTime, setRemainingTime] = useState<number>(180);
  const [gameStarted, setGameStarted] = useState<boolean>(false);
  const { darkMode, toggleTheme } = useDarkMode();

  const navigate = useNavigate();

  useEffect(() => {
    resetLetters();
  }, []);

  useEffect(() => {
    let timer;
    if (gameStarted && remainingTime > 0) {
      timer = setInterval(() => {
        setRemainingTime((prevTime) => prevTime - 1);
      }, 1000);
    } else if (remainingTime <= 0) {
      setErrorMessage("Time's up! You can no longer submit words.");
    }
    return () => clearInterval(timer);
  }, [gameStarted, remainingTime]);

  const resetLetters = async () => {
    try {
      const response = await fetch("http://localhost:5189/api/Boggle/shuffle");
      if (!response.ok) throw new Error("Network response was not ok");
      const data = await response.json();
      setLetters(data);
      setClickedCells(new Array(data.length).fill(false));
      setClickedLetters("");
      setCompletedWords([]);
      setErrorMessage("");
      setClickedIndices([]);
      setRemainingTime(180);
      setGameStarted(false);
    } catch (error) {
      console.error("Failed to fetch letters: ", error);
    }
  };

  const handleClick = async (letter: string, index: number) => {
    if ((gameMode === "timed" && !gameStarted) || remainingTime <= 0) {
      return;
    }

    console.log("Clicked letter:", letter);
    if (clickedIndices.includes(index)) {
      const word = clickedLetters;
      const isValid = await validateInput(word);
      if (completedWords.includes(word)) {
        // Word found already
        setErrorMessage(`The word "${word}" has already been found!`);
      } else {
        // Confirm a new word
        console.log("Confirmed word:", word);
        setCompletedWords((prevCompletedWords) => [
          ...prevCompletedWords,
          word,
        ]);
        setErrorMessage("");
      }
      setClickedLetters("");
      setClickedIndices([]);
      setClickedCells(new Array(letters.length).fill(false));
    } else {
      // Make sure letter clicked is adjacent or diagonal
      if (clickedIndices.length > 0) {
        const [prevRowIndex, prevColIndex] = [
          Math.floor(clickedIndices[clickedIndices.length - 1] / 4),
          clickedIndices[clickedIndices.length - 1] % 4,
        ];
        const [currRowIndex, currColIndex] = [Math.floor(index / 4), index % 4];

        const rowDifference = Math.abs(currRowIndex - prevRowIndex);
        const colDifference = Math.abs(currColIndex - prevColIndex);

        if (
          (rowDifference <= 1 && colDifference <= 1) ||
          (rowDifference === 1 && colDifference === 1)
        ) {
          // Letters are adjacent or diagonal
          setClickedLetters((prevLetters) => prevLetters + letter);
          setClickedIndices((prevClickedIndices) => [
            ...prevClickedIndices,
            index,
          ]);
          setClickedCells((prevState) => {
            const newState = [...prevState];
            newState[index] = !prevState[index];
            return newState;
          });
          setErrorMessage("");
        } else {
          // Letters are not adjacent or diagonal
          setErrorMessage(
            "Select a letter that is adjacent or diagonal to the previously clicked letter."
          );
        }
      } else {
        // First clicked letter
        setClickedLetters((prevLetters) => prevLetters + letter);
        setClickedIndices((prevClickedIndices) => [
          ...prevClickedIndices,
          index,
        ]);
        setClickedCells((prevState) => {
          const newState = [...prevState];
          newState[index] = !prevState[index];
          return newState;
        });
      }
    }
  };

  const handleLoginClick = () => {
    navigate("/login");
  };

  const toggleGameMode = () => {
    resetLetters();
    setGameMode((prevMode) => (prevMode === "timed" ? "untimed" : "timed"));
  };

  const handleStartGame = () => {
    setGameStarted(true);
  };

  return (
    <div className={darkMode ? "dark-mode" : "light-mode"}>
      <div className="header">
        <button id="color-scheme-switch" onClick={toggleTheme}>
          {darkMode ? "Light" : "Dark"}
        </button>
        <h2>Boggle</h2>
        <button id="login" onClick={handleLoginClick}>
          Login
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
            {gameMode === "timed" ? "Timed" : "Untimed"}
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
