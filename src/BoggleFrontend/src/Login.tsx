import { useState, useEffect } from 'react';
import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Login.css';

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { darkMode, toggleTheme } = useDarkMode();

    const navigate = useNavigate();

    useEffect(() => {
      const userId = localStorage.getItem('userId');
      if (userId) {
        console.log('User already logged in, user ID:', userId);
        navigate('/profile');
      }
    }, [navigate]);

    const handleLogin = async () => {
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(password)
      };
    
      try {
        const response = await fetch(`http://localhost:5189/api/PlayerInfo/player/${username}/authenticate`, requestOptions);
        if (response.ok) {
          const data = await response.json();
          localStorage.setItem('userId', data.userId);
          localStorage.setItem('username', username);
          navigate('/profile');
        } else if (response.status === 404) {
          setError('Invalid username or password');
        }
      } catch (error) {
        console.error('Login failed:', error);
        setError('Login failed. Please try again.');
      }
    };

    const handleKeyPress = (event) => {
      if (event.key === 'Enter') {
          handleLogin();
      }
    };
  
    return (
      <div className={darkMode ? 'dark-mode' : 'light-mode'}>
        <div className="header">
        <button id="multiplayer" onClick={() => navigate('/multiplayer')}>
          Multiplayer
        </button>
          <h2>Boggle</h2>
          <button id="game" onClick={() => navigate('/')}>Game</button>
        </div>
        <div className="login-container">
            <h2>Login</h2>
            <input
                className='username'
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                onKeyDown={handleKeyPress}
            />
            <input
                className='password'
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                onKeyDown={handleKeyPress}
            />
            <button onClick={handleLogin}>Login</button>
            <span>Don't have an account? Sign up <Link to='/signup' className='signup-link'>here</Link> </span>
            {error && <div className="error-message">{error}</div>}
        </div>
        <div className="footer">
          <a href='https://github.com/Kashish-Syed/Boggle' className="github-link">
            <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png" alt="GitHub" style={{ height: '25px', marginRight: '5px', verticalAlign: 'middle' }} />
          </a>
        </div>
      </div>
    );
  }
  
  export default Login;
