import React, { useState, useEffect } from "react";
import "./styles/App.css";
import { useDarkMode } from "./DarkModeContext";
import { placeholderLetters} from "./Constants";
import { useNavigate } from "react-router-dom";
import * as Functions from "./Functions";

function App() {
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
      Functions.handleStartGame(Functions.getLetters, setLetters, setClickedCells, setGameStarted, setRemainingTime);
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

  const handleLogin = () => {
    Functions.handleLogin(userId, navigate);
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
              onClick={() => Functions.handleClick(letter, index, gameMode, gameStarted, remainingTime, clickedIndices, clickedLetters, completedWords, letters, setClickedLetters, setCompletedWords, setErrorMessage, setClickedIndices, setClickedCells, Functions.handleLetterSelection, clickedCells, errorMessage)}
            >
              {letter}
            </div>
          ))}
        </div>
        <div className="button-container">
          <button id="gamemode-button" onClick={() => Functions.toggleGameMode(gameMode, setGameMode, setRemainingTime, placeholderLetters, setLetters, setGameStarted, Functions.resetLetters, setClickedLetters, setCompletedWords, setErrorMessage, setClickedIndices)}>
            {gameMode === "timed" ? "Untimed" : "Timed"}
          </button>
          {gameMode === "timed" && (
            <button id="start-button" onClick={() => Functions.handleStartGame(Functions.getLetters, setLetters, setClickedCells, setGameStarted, setRemainingTime)}>
              Start Game
            </button>
          )}
          <button id="reset-button" onClick={() => Functions.resetLetters(gameStarted, gameMode, placeholderLetters, setClickedLetters, setCompletedWords, setErrorMessage, setClickedIndices, setRemainingTime, setLetters, setGameStarted)}>
            Reset
          </button>
        </div>
        {gameMode === "timed" && (
          <div id="timer">{Functions.formatTime(remainingTime)}</div>
        )}
        <div id="word-list">
        <h2>Words Found</h2>
        <textarea
          id="words-found"
          readOnly
          value={completedWords.join(", ")} 
          rows={3} 
          style={{ resize: "none" }}
        />
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
