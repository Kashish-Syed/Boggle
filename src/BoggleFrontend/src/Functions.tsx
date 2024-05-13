import React, { useState, useEffect } from "react";

export function formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    const formattedMinutes = String(minutes).padStart(2, "0");
    const formattedSeconds = String(remainingSeconds).padStart(2, "0");
    return `${formattedMinutes}:${formattedSeconds}`;
}

export async function resetLetters(
    gameStarted: boolean,
    gameMode: string,
    placeholderLetters: string[],
    setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
    setCompletedWords: React.Dispatch<React.SetStateAction<string[]>>,
    setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
    setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>,
    setRemainingTime: React.Dispatch<React.SetStateAction<number>>,
    setLetters: React.Dispatch<React.SetStateAction<string[]>>,
    setGameStarted: React.Dispatch<React.SetStateAction<boolean>>
  ) {
    if (!gameStarted && gameMode === "timed") return;
    setClickedLetters("");
    setCompletedWords([]);
    setErrorMessage("");
    setClickedIndices([]);
    setRemainingTime(60);
    if(gameMode === "timed") {
        setLetters(placeholderLetters);
        setGameStarted(false);
    } else {
        const data = await getLetters();
        setLetters(data);
    }
  }
  
  export async function getLetters() {
    try {
        const createGameResponse = await fetch('http://localhost:5189/api/GameInfo/game/createGame', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (!createGameResponse.ok) throw new Error("Failed to create game");
        const gameCode = await createGameResponse.text();
        localStorage.setItem('gameCode', gameCode);

        const getBoardResponse = await fetch(`http://localhost:5189/api/GameInfo/game/${gameCode}/getBoard`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        if (!getBoardResponse.ok) throw new Error("Failed to get board");
        const board = await getBoardResponse.json();
        return board;
    } catch (error) {
        console.error("Failed to fetch letters: ", error);
    }
}
  
export async function handleClick(
    letter: string,
    index: number,
    gameMode: string,
    gameStarted: boolean,
    remainingTime: number,
    clickedIndices: number[],
    clickedLetters: string,
    completedWords: string[],
    letters: string[],
    setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
    setCompletedWords: React.Dispatch<React.SetStateAction<string[]>>,
    setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
    setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>,
    setClickedCells: React.Dispatch<React.SetStateAction<boolean[]>>,
    handleLetterSelection: (
      letter: string,
      index: number,
      clickedIndices: number[],
      setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>,
      clickedLetters: string,
      setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
      clickedCells: boolean[],
      setClickedCells: React.Dispatch<React.SetStateAction<boolean[]>>,
      errorMessage: string,
      setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
    ) => void,
    clickedCells: boolean[],
    errorMessage: string
  ) {
    if ((gameMode === "timed" && !gameStarted) || remainingTime <= 0) {
      return;
    }
  
    if (clickedIndices.includes(index)) {
      const word = clickedLetters;
      // API call to validate the word
      try {
        const response = await fetch('http://localhost:5189/api/WordInfo/isValidWord', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(word) // Send the word as a JSON string
        });
        if (!response.ok) {
          throw new Error(`Failed to validate word with status: ${response.status}`);
        }
        const isValid = await response.json(); 
  
        console.log("Validation result:", isValid);
        if (isValid) {
          if (completedWords.includes(word)) {
            setErrorMessage(`The word "${word}" has already been found!`);
          } else {
            setCompletedWords(prevCompletedWords => [
              ...prevCompletedWords,
              word,
            ]);
            setErrorMessage("");
          }
        }
      } catch (error) {
        console.error("Error validating word:", error);
      }
      // Reset after submitting the word
      setClickedLetters("");
      setClickedIndices([]);
      setClickedCells(new Array(letters.length).fill(false));
    } else {
      // Handling letter selection for adjacent or diagonal letters
      handleLetterSelection(
        letter,
        index,
        clickedIndices,
        setClickedIndices,
        clickedLetters,
        setClickedLetters,
        clickedCells,
        setClickedCells,
        errorMessage,
        setErrorMessage
      );
    }
  }
  
  
  

export async function handleLetterSelection(
    letter: string,
    index: number,
    clickedIndices: number[],
    setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>,
    clickedLetters: string,
    setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
    clickedCells: boolean[],
    setClickedCells: React.Dispatch<React.SetStateAction<boolean[]>>,
    errorMessage: string,
    setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
    ) {
    if (clickedIndices.length > 0) {
        const prevIndex = clickedIndices[clickedIndices.length - 1];
        const prevRowIndex = Math.floor(prevIndex / 4);
        const prevColIndex = prevIndex % 4;
        const currRowIndex = Math.floor(index / 4);
        const currColIndex = index % 4;
    
        const rowDifference = Math.abs(currRowIndex - prevRowIndex);
        const colDifference = Math.abs(currColIndex - prevColIndex);
    
        if (rowDifference > 1 || colDifference > 1) {
        setErrorMessage("Select a letter that is adjacent or diagonal to the previously clicked letter.");
        return;
        }
    }
    
    // Update clicked letters, indices, and cells
    setClickedLetters(clickedLetters + letter);
    setClickedIndices([...clickedIndices, index]);
    setClickedCells(clickedCells.map((val, idx) => idx === index ? true : val));
    setErrorMessage("");
    }
  
  export function handleLogin(userId: string | null, navigate: (route: string) => void) {
    if (userId) {
      navigate('/profile');
    } else {
      navigate('/login');
    }
  }
  
export async function handleStartGame(
    getLetters: () => Promise<any>,
    setLetters: React.Dispatch<React.SetStateAction<string[]>>,
    setClickedCells: React.Dispatch<React.SetStateAction<boolean[]>>,
    setGameStarted: React.Dispatch<React.SetStateAction<boolean>>,
    setRemainingTime: React.Dispatch<React.SetStateAction<number>>
  ) {
    const data = await getLetters();
    setLetters(data);
    setClickedCells(new Array(data.length).fill(false));
    setGameStarted(true);
    setRemainingTime(60);
}
  
export function toggleGameMode(
    gameMode: string,
    setGameMode: React.Dispatch<React.SetStateAction<string>>,
    setRemainingTime: React.Dispatch<React.SetStateAction<number>>,
    placeholderLetters: string[],
    setLetters: React.Dispatch<React.SetStateAction<string[]>>,
    setGameStarted: React.Dispatch<React.SetStateAction<boolean>>,
    resetLetters: (
      gameStarted: boolean,
      gameMode: string,
      placeholderLetters: string[],
      setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
      setCompletedWords: React.Dispatch<React.SetStateAction<string[]>>,
      setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
      setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>,
      setRemainingTime: React.Dispatch<React.SetStateAction<number>>, 
      setLetters: React.Dispatch<React.SetStateAction<string[]>>,
      setGameStarted: React.Dispatch<React.SetStateAction<boolean>>
    ) => Promise<void>,
    setClickedLetters: React.Dispatch<React.SetStateAction<string>>,
    setCompletedWords: React.Dispatch<React.SetStateAction<string[]>>,
    setErrorMessage: React.Dispatch<React.SetStateAction<string>>,
    setClickedIndices: React.Dispatch<React.SetStateAction<number[]>>
  ) {
    if (gameMode === "timed") {
      setGameMode("untimed");
      resetLetters(
        false,
        "untimed",
        [],
        setClickedLetters,
        setCompletedWords,
        setErrorMessage,
        setClickedIndices,
        setRemainingTime, 
        setLetters,
        setGameStarted
      );
    } else {
      setGameMode("timed");
      setRemainingTime(60);
      setLetters(placeholderLetters);
      setGameStarted(false);
      setCompletedWords([]);
      resetLetters(
        false,
        "timed",
        placeholderLetters,
        setClickedLetters,
        setCompletedWords,
        setErrorMessage,
        setClickedIndices,
        setRemainingTime,
        setLetters,
        setGameStarted
      );
    }
}
  
  