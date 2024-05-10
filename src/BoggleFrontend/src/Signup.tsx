import { useState, useEffect } from 'react';
import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useDarkMode } from './DarkModeContext';
import './styles/Signup.css';

function Signup() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirmation, setPasswordConfirmation] = useState('');
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

    const handleSignup = async () => {
      if (password !== passwordConfirmation) {
        setError('Passwords do not match');
        return;
      }
      try {
        const response = await fetch('http://localhost:5189/api/WordInfo/isValidWord', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(password) 
        });
        if (response.ok) {
          const data = await response.json();
          localStorage.setItem('userId', data.userId);
          localStorage.setItem('username', username);
          navigate('/profile');
        } else {
          setError('Failed to create account. Please try again.');
        }
      } catch (error) {
        console.error('Signup failed:', error);
        setError('Signup failed. Please try again.');
      }
    };

    const handleKeyPress = (event) => {
      if (event.key === 'Enter') {
          handleSignup();
      }
    };
  
    return (
      <div className={darkMode ? 'dark-mode' : 'light-mode'}>
        <div className="header">
          <button id="color-scheme-switch" onClick={toggleTheme}>{darkMode ? "Light" : "Dark"}</button>
          <h2>Boggle</h2>
          <button id="game" onClick={() => navigate('/')}>Game</button>
        </div>
        <div className="signup-container">
            <h2>Sign Up</h2>
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
            <input
              className='password-confirmation'
              type='password'
              placeholder='Re-type password'
              value={passwordConfirmation}
              onChange={(e) => setPasswordConfirmation(e.target.value)}
              onKeyDown={handleKeyPress}
            />
            <button onClick={handleSignup}>Signup</button>
            <span>Already have an account? Log in <Link to='/login' className='login-link'>here</Link> </span>
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
  
  export default Signup;
