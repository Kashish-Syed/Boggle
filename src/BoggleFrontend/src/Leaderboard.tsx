import { useState, useEffect } from 'react';
import React from 'react';
import { useNavigate } from "react-router-dom";
import "./styles/Leaderboard.css";
import { useDarkMode } from "./DarkModeContext";

type Player = {
  name: string;
  score: number;
};

function Leaderboard() {
  const [completedWords, setCompletedWords] = useState<string[]>([]);
  const [players, setPlayers] = useState<Player[]>([]); 
  const [playerName, setPlayerName] = useState('');
  const [errorMessage, setErrorMessage] = useState<string>("");
  const { darkMode, toggleTheme } = useDarkMode();
  const [userId] = useState(localStorage.getItem('userId'));

  const navigate = useNavigate();
  

  const handleLogin = () => {
    if (userId) {
      navigate('/profile');
    } else {
      navigate('/login');
    }
  };

  const newGame = () => {
    navigate('/');
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
      <div id='stats-container'>
        <h2>Game Over!</h2>
        <h4>Your score: </h4>
        <hr />
        <h4>Leaderboard:</h4>
        <ul>
            {players.map((player, index) => (
              <li key={index}>
                {player.name}
                {player.score}
              </li>
            ))}
          </ul>
        {errorMessage && <h3 className="error-message">{errorMessage}</h3>}
       
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
export default Leaderboard;
