import React, { useState, useEffect, useRef } from 'react';
import './styles/App.css';

function App() {
  const [darkMode, setDarkMode] = useState(false);
  const [letters, setLetters] = useState([]);
  const [clickedCells, setClickedCells] = useState<Array<boolean>>([]);
  const [clickedLetters, setClickedLetters] = useState<string>(""); 
  const [clickedIndices, setClickedIndices] = useState<number[]>([]);
  const [completedWords, setCompletedWords] = useState<string[]>([]);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const specificDivRefs = useRef<Array<React.RefObject<HTMLDivElement>>>([]);

  useEffect(() => {
    document.body.className = darkMode ? 'dark-mode' : 'light-mode';
  }, [darkMode]);

  const toggleTheme = () => {
    setDarkMode(!darkMode);
  };

  useEffect(() => {
    resetLetters();
  }, []);

  const resetLetters = async () => {
    try {
      const response = await fetch('http://localhost:5189/api/Boggle/shuffle');
      if (!response.ok) throw new Error('Network response was not ok');
      const data = await response.json();
      setLetters(data);
      setClickedCells(new Array(data.length).fill(false));
      specificDivRefs.current = new Array(data.length).fill(null).map(() => useRef<HTMLDivElement>(null));
      setClickedLetters("");
      setCompletedWords([]);
      setErrorMessage("");
    } catch (error) {
      console.error('Failed to fetch letters: ', error);
    }
  };
  



  const handleClick = (letter: string, index: number) => {
    console.log("Clicked letter:", letter);
    if (clickedIndices.includes(index)) {
      const word = clickedLetters;
      if (completedWords.includes(word)) {
        setErrorMessage(`The word "${word}" has already been found!`);
        setClickedLetters("");
        setClickedIndices([]);
        setClickedCells(new Array(letters.length).fill(false));
        setClickedCells(prevState => {
          const newState = [...prevState];
          newState[index] = !prevState[index];
          return newState;
        });
      } else {
        console.log("Confirmed word:", word);
        setCompletedWords(prevCompletedWords => [...prevCompletedWords, word]);
        setClickedLetters("");
        setClickedIndices([]);
        setClickedCells(new Array(letters.length).fill(false));
        setErrorMessage("");
        setClickedCells(prevState => {
          const newState = [...prevState];
          newState[index] = !prevState[index];
          return newState;
        });
      }
    } else {
      setClickedLetters(prevLetters => prevLetters + letter);
      setClickedIndices(prevClickedIndices => [...prevClickedIndices, index]);
    }
    setClickedCells(prevState => {
      const newState = [...prevState];
      newState[index] = !prevState[index];
      return newState;
    });
  };  

  const handleLoginClick = () => {
    window.location.href = 'http://localhost:5173/login';
  };

  return (
    <div className={darkMode ? 'dark-mode' : 'light-mode'}>
      <div className="header">
        <button id="color-scheme-switch" onClick={toggleTheme}>{darkMode ? "Light" : "Dark"}</button>
        <h2>Boggle</h2>
        <button id="login" onClick={handleLoginClick}>Login</button>
      </div>
      <div id="boggle-container">
        <div id="word-curr"><h2>Current word: {clickedLetters}</h2></div>
        <div id="board">
          {letters.map((letter, index) => (
            <div key={index} className="cell" id={clickedCells[index] ? 'two' : 'one'} ref={specificDivRefs.current[index]} onClick={() => handleClick(letter, index)}>{letter}</div>
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
            {completedWords.map((word, index) => (
              <li key={index}>{word}</li>
            ))}
          </ul>
          {errorMessage && <p className="error-message">{errorMessage}</p>}
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
