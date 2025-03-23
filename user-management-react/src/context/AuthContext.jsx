import React, { createContext, useState, useEffect } from 'react';
import decodeJwt, { isTokenExpired } from '../utils/JwtUtils';
import { refreshAccessToken } from '../services/auth';
import { useNavigate } from 'react-router-dom';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(localStorage.getItem('token') || "");
    const navigate = useNavigate();

    useEffect(() => {
        checkToken();
    },[]);

    const checkToken = async () => {
        if (token) { 
            if (isTokenExpired(token)) { 
                const newToken = await refreshAccessToken(); 
                if (newToken) {
                    localStorage.setItem('token', newToken); 
                    setToken(newToken); 
                    setUser(decodeUser(newToken)); 
                } else {
                    logout(); 
                }
            } else {
                setUser(decodeUser(token)); 
            }
        } else {
            setUser(null); 
        }
    };

    const decodeUser = (token) => {
        const decoded = decodeJwt(token);
        if (!decoded) return null;
        return {
            name: decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || "",
            email: decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || "",
            id: decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || "",
        };
    };

    const signIn = (accessToken, newRefreshToken) => {
        localStorage.setItem('token', accessToken);
        localStorage.setItem('refreshToken', newRefreshToken);
        setToken(accessToken);
        setUser(decodeUser(accessToken));
        return decodeUser(accessToken);
    };

    const logout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        setToken(null);
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, signIn, logout }}>
            {children}
        </AuthContext.Provider>
    );
};