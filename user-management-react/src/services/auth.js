import { isTokenExpired } from '../utils/jwtUtils';
import { API_BACKEND_URL } from '../config.js';
import axios from 'axios';

export const apiFetch = async (url, options) => {
    let token = localStorage.getItem('token');

    if (token && isTokenExpired(token)) {
        token = await refreshAccessToken();
        if (!token) throw new Error('Unable to refresh token');
    }

    const headers = {
        'Authorization': `Bearer ${token}`,
        'Content-Type': options.data instanceof FormData ? 'multipart/form-data' : 'application/json',
        ...options.headers
    };

    try {
        const response = await axios({ url, headers, ...options });
        return response;
    } catch (error) {
        handleError(error);
        throw error;
    }
};

export const refreshAccessToken = async () => {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) return null;
    try {
        const response = await axios.post(`${API_BACKEND_URL}/token/refresh`, { refreshToken });
        const { accessToken } = response.data;
        localStorage.setItem('token', accessToken);
        return accessToken;
    } catch (error) {
        console.error('Error refreshing token:', error);
        return null;
    }
};

const handleError = (error) => {
    if (error.response.status == 401) {
        return;
    } 
    else if (error.response) {
        console.error('API Error:', error.response.data);
    } else {
        console.error('Network Error:', error);
    }
};

export const login = async ({ email, password }) => {
    try {
        const response = await axios.post(`${API_BACKEND_URL}/auth/login`, { email, password });
        return response; 
    } catch (error) {
        handleError(error);
        throw error; 
    }
};

export const register = async (data) => {
    try {
        const response = await axios.post(`${API_BACKEND_URL}/auth`, data);
        return response.data; 
    } catch (error) {
        handleError(error);
        throw error; 
    }
};