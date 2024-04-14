import { useState, useEffect } from 'react';
import './styles/App.css';

function App() {
  const [darkMode, setDarkMode] = useState(false);
  const [letters, setLetters] = useState([]);

  useEffect(() => {
    document.body.className = darkMode ? 'dark-mode' : 'light-mode';
  }, [darkMode]);

  useEffect(() => {
    resetLetters();
  }, []);

  const resetLetters = async () => {
    try {
      const response = await fetch('http://localhost:5189/api/Boggle/shuffle');
      if (!response.ok) throw new Error('Network response was not ok');
      const data = await response.json();
      setLetters(data);
    } catch (error) {
      console.error('Failed to fetch letters: ', error);
    }
  };

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
        <button id="login">Login</button>
      </div>
      <div id="boggle-container">
        <div id="board">
          {letters.map((letter, index) => (
            <div key={index} className="cell">{letter}</div>
          ))}
        </div>
        <div className="button-container"> 
          <button id="start-button">Start Game</button>
          <button id="reset-button" onClick={resetLetters}>Reset</button>
        </div>
        <div id="timer">00:00</div>
        <div id="word-list">
          <h2>Words Found</h2>
          <ul id="words-found">
            <li>Words</li>
            <li>Go</li>
            <li>Here</li>
          </ul>
        </div>
      </div>
      <div className="footer">
        <a href='https://github.com/Kashish-Syed/Boggle' className="github-link">
          <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png" alt="GitHub" style={{ height: '25px', marginRight: '5px', verticalAlign: 'middle' }} />
        </a>
      </div>
    </div>
  );
}

export default App;
