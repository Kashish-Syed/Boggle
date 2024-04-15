import { useState, useEffect, useRef } from 'react';
import React from 'react';
import './styles/Login.css';

function Login() {
    const [darkMode, setDarkMode] = useState(false);
    const [letters, setLetters] = useState([]);
    const [clickedCells, setClickedCells] = useState<Array<boolean>>([]);
    const specificDivRefs = useRef<Array<React.RefObject<HTMLDivElement>>>([]);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleGameClick = () => {
        window.location.href = 'http://localhost:5173';
    };    
  
    const handleLogin = () => {
      if (username === 'admin' && password === 'password') {
        console.log('Login successful!');
      } else {
        setError('Invalid username or password');
      }
    };

    useEffect(() => {
      document.body.className = darkMode ? 'dark-mode' : 'light-mode';
    }, [darkMode]);

    const toggleTheme = () => {
      setDarkMode(!darkMode);
    };
  
    useEffect(() => {
      document.body.className = darkMode ? 'dark-mode' : 'light-mode';
    }, [darkMode]);
  
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
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
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