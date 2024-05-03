import { useState, useEffect } from 'react';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Login.css';

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { darkMode, toggleTheme } = useDarkMode();

    const navigate = useNavigate();

    const handleGameClick = () => {
        navigate('/');
    };    
  
    const handleLogin = () => {
      if (username === 'admin' && password === 'password') {
        navigate('/profile');
      } else {
        setError('Invalid username or password');
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
          <button id="color-scheme-switch" onClick={toggleTheme}>{darkMode ? "Light" : "Dark"}</button>
          <h2>Boggle</h2>
          <button id="game" onClick={handleGameClick}>Game</button>
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