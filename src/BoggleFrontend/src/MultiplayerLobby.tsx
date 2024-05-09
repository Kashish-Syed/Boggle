import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/MultiplayerLobby.css';

type Player = {
  name: string;
  ready: boolean;
};

function Multiplayer() {
  const { darkMode, toggleTheme } = useDarkMode();
  const [players, setPlayers] = useState<Player[]>([]); 
  const [playerName, setPlayerName] = useState(''); 
  const [gameStarted, setGameStarted] = useState(false); 
  const navigate = useNavigate();
  const [userId] = useState(localStorage.getItem('userId'));

  useEffect(() => {
    const allReady = players.length > 0 && players.every((player) => player.ready);
    if (allReady && !gameStarted && players.length >= 2) {
      startGame();
    }
  }, [players, gameStarted]);

  const startGame = () => {
    setGameStarted(true);
    navigate('/');
  };

  const handlePlayerJoin = () => {
    if (playerName.trim() !== '') {
      setPlayers([...players, { name: playerName, ready: false }]);
      setPlayerName('');
    }
  };

  const togglePlayerReady = (index: number) => {
    setPlayers((prevPlayers) =>
      prevPlayers.map((player, i) =>
        i === index ? { ...player, ready: !player.ready } : player
      )
    );
  };

  const handleLogin = () => {
    if (userId) {
      navigate('/profile');
    } else {
      navigate('/login');
    }
  };

  return (
    <div className={darkMode ? 'dark-mode' : 'light-mode'}>
      <div className="header">
        <button id="multiplayer" onClick={() => navigate('/multiplayer')}>Multiplayer</button>
        <h2>Boggle</h2>
        <button id="login" onClick={handleLogin}>
          {userId ? 'Profile' : 'Login'}
        </button>
      </div>
      <div id="multiplayer-container">
        <div id="ready">
          <h3>Game ID:</h3>
          <h3>Players:</h3>
          <ul>
            {players.map((player, index) => (
              <li key={index}>
                {player.name}
                <input
                  type="checkbox"
                  checked={player.ready}
                  onChange={() => togglePlayerReady(index)}
                />
                Ready
              </li>
            ))}
          </ul>
          <hr />
        </div>
        <div id="ready">
          <h3>Join as Player:</h3>
          <input
            type="text"
            placeholder="Enter your name"
            value={playerName}
            onChange={(e) => setPlayerName(e.target.value)}
          />
          <button onClick={handlePlayerJoin}>Join</button>
        </div>
        {players.length > 1 && (
          <div id="start-game">
            <button onClick={startGame} disabled={gameStarted}>
              Start Game
            </button>
          </div>
        )}
      </div>
      <div className="footer">
        <a href="https://github.com/Kashish-Syed/Boggle" className="github-link">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png"
            alt="GitHub"
            style={{ height: '25px', marginRight: '5px', verticalAlign: 'middle' }}
          />
        </a>
      </div>
    </div>
  );
}

export default Multiplayer;
