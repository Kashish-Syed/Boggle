import { useState, useEffect } from 'react';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Profile.css';

interface Game {
  GameCode: string;
  TotalScore: number;
}

function Profile(){
  const { darkMode, toggleTheme } = useDarkMode();
  const [gameLength, setGameLength] = useState('');
  const [userId] = useState(localStorage.getItem('userId'));
  const username = localStorage.getItem('username'); 
  const [games, setGames] = useState<Game[]>([]);

  useEffect(() => {
    const getGamesPlayed = async () => {
      if (username) {
        try {
          const response = await fetch(`http://localhost:5189/api/PlayerInfo/player/${username}/games`);
          if (response.ok) {
            const data: Game[] = await response.json();
            setGames(data); 
          } else {
            throw new Error('Failed to get games');
          }
        } catch (error) {
          console.error('Error getting games:', error);
        }
      }
    };
  
    getGamesPlayed();
  }, [username]);


  const navigate = useNavigate();

  const handleKeyPress = (event) => {
    
  };

  const handleLogout = () => {
    if (userId) {
      localStorage.removeItem('userId');
      localStorage.removeItem('username');
      navigate('/login')
    }
  };

  return (
    <div className={darkMode ? 'dark-mode' : 'light-mode'}>
      <div className="header">
          <button id="multiplayer" onClick={() => navigate('/multiplayer')}>Multiplayer</button>
          <h2>Boggle</h2>
          <button id="game" onClick={() => navigate('/')}>Game</button>
        </div>
      <div className="information-container">
        <h1 className="usernameView">{username || 'Username'}</h1>
          <button id="color-scheme-switch" onClick={toggleTheme}>{darkMode ? "Light" : "Dark"}</button>
          <div className="games-list">
          <h2>Games Played</h2>
          {games.length > 0 ? (
            <ul className="games">
              {games.map((game, index) => (
                <li key={index}>
                  {game.GameCode} -  Score: {game.TotalScore}
                </li>
              ))}
            </ul>
          ) : (
            <p>No games played yet.</p>
          )}
        </div>
        <button id="logout" onClick={handleLogout}>Logout</button>
      </div>
      <div className="footer">
          <a href='https://github.com/Kashish-Syed/Boggle' className="github-link">
            <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png" alt="GitHub" style={{ height: '25px', marginRight: '5px', verticalAlign: 'middle' }} />
          </a>
        </div>
    </div>
  );
}

export default Profile;