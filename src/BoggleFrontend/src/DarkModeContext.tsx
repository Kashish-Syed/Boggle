import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

interface DarkModeContextType {
    darkMode: boolean;
    toggleTheme: () => void;
}

const DarkModeContext = createContext<DarkModeContextType | undefined>(undefined);

export const useDarkMode = () => {
    const context = useContext(DarkModeContext);
    if (context === undefined) {
        throw new Error('undefined context');
    }
    return context;
}

export const DarkMode: React.FC<{ children: ReactNode }> = ({ children }) => {
    
    // locally stored so it can be maintained across sessions, pretty cool eh?
    const [darkMode, setDarkMode] = useState<boolean>(() => {
        const storedMode = localStorage.getItem('darkMode');
        return storedMode !== null ? storedMode === 'true' : false;
    });

    useEffect(() => {
        document.body.className = darkMode ? 'dark-mode' : 'light-mode';
        localStorage.setItem('darkMode', darkMode.toString());
    }, [darkMode]);

    const toggleTheme = () => {
        setDarkMode(storedMode => !storedMode);
    };

    return (
        <DarkModeContext.Provider value={{ darkMode, toggleTheme }}>
            {children}
        </DarkModeContext.Provider>
    );
};

