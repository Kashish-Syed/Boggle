import React, { useState, useEffect } from 'react';
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

    const usernameRegex = /^[a-zA-Z0-9_.'\s]{1,40}$/;
    const passwordRegex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{5,30}$/;

    useEffect(() => {
      const userId = localStorage.getItem('userId');
      if (userId) {
        navigate('/profile');
      }
    }, [navigate]);

    const handleSignup = async () => {
      if (!usernameRegex.test(username)) {
        setError('Username is invalid.');
        return;
      }
      if (password !== passwordConfirmation) {
        setError('Passwords do not match');
        return;
      }
      if (!passwordRegex.test(password)) {
        setError('Password does not meet requirements');
        return;
      }
    
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(password)
      };
    
      try {
        const response = await fetch(`http://localhost:5189/api/PlayerInfo/player/${username}/add`, requestOptions);
        if (response.ok) {
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
          navigate('/profile');
        } else {
          const errData = await response.json();
          setError(errData.message || 'Registration failed due to bad input.');
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
            <div className="password-requirements">
                Password requirements:
                <ul>
                    <li>Must be between 5 and 30 characters</li>
                    <li>Must contain at least one number</li>
                    <li>Must contain at least one lowercase letter</li>
                    <li>Must contain at least one uppercase letter</li>
                </ul>
            </div>
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
