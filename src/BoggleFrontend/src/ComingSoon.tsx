import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/ComingSoon.css'; 

function ComingSoon() {
    const { darkMode, toggleTheme } = useDarkMode();
    const navigate = useNavigate();

    return (
        <div className={darkMode ? "dark-mode" : "light-mode"}>
            <div className="header">
                <button id="color-scheme-switch" onClick={toggleTheme}>
                    {darkMode ? "Light" : "Dark"}
                </button>
                <h2>Boggle</h2>
                <button id="home" onClick={() => navigate('/')}>Game</button>
            </div>
            <div className="coming-soon-container">
                <h1>Coming Soon!</h1>
                <p>Multiplayer is in progress. Stay tuned!</p>
            </div>
            <div className="footer">
                <a href="https://github.com/Kashish-Syed/Boggle" className="github-link">
                    <img
                        src="https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Octicons-mark-github.svg/1200px-Octicons-mark-github.svg.png"
                        alt="GitHub"
                        style={{ height: "25px", marginRight: "5px", verticalAlign: "middle" }}
                    />
                </a>
            </div>
        </div>
    );
}

export default ComingSoon;
