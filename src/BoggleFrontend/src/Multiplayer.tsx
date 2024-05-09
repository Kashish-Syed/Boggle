import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Multiplayer.css';

function Multiplayer() {
  const { darkMode } = useDarkMode();
  const navigate = useNavigate();
  const [userId] = useState(localStorage.getItem('userId'));
  const [joinGameCode, setJoinGameCode] = useState('');
  const [hostGameCode, setHostGameCode] = useState('');

  const handleJoinGame = (event) => {
    event.preventDefault();
    console.log(`Joining game with code: ${joinGameCode}`);
    if (joinGameCode.trim() !== '') {
      navigate(`/multiplayer-lobby`);
    }
  };

  const handleHostGame = () => {
    const code = Math.random().toString(36).substr(2, 6).toUpperCase();
    setHostGameCode(code);
    console.log(`Hosting game with code: ${code}`);
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
        <button id="multiplayer" onClick={() => navigate('/multiplayer')}>
          Multiplayer
        </button>
        <h2>Boggle</h2>
        <button id="login" onClick={handleLogin}>
          {userId ? 'Profile' : 'Login'}
        </button>
      </div>
      <div id="multiplayer-container">
        <div id="join_game">
          <h2>Join a Game</h2>
          <form onSubmit={handleJoinGame}>
            <label>Host IP: </label>
            <input
              type="text"
              placeholder="Enter Host's IP"
              value={joinGameCode}
              onChange={(e) => setJoinGameCode(e.target.value)}
            />
            <br />
            <label>Host Port: </label>
            <input
              type="text"
              placeholder="Enter Host's port number"
              value={joinGameCode}
              onChange={(e) => setJoinGameCode(e.target.value)}
            />
            <br />
            <button type="submit">Submit</button>
          </form>
        </div>
        <div id="host_game">
          <h2>Host a Game</h2>
          <button onClick={handleHostGame}>Click to Host Game</button>
        </div>
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
