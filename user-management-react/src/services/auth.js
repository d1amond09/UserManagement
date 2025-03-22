import { isTokenExpired } from '../utils/jwtUtils';
import { API_BACKEND_URL } from '../config.js';
import axios from 'axios';

export const apiFetch = async (url, options) => {
    let token = localStorage.getItem('token');

    if (isTokenExpired(token)) {
        token = await refreshAccessToken();
        if (!token) throw new Error('Unable to refresh token');
    }

    const headers = {
        'Authorization': `Bearer ${token}`,
        'Content-Type': options.data instanceof FormData ? 'multipart/form-data' : 'application/json',
        ...options.headers
    };

    const response = await axios({
        url,
        headers,
        ...options,
    });

    return response;
};

export const refreshAccessToken = async () => {
    const refreshToken = localStorage.getItem('refreshToken');
    const token = localStorage.getItem('token');
    if (!refreshToken) return null;

    try {
        const response = await axios.post(`${API_BACKEND_URL}/token/refresh`, token, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            credentials: 'include'
        });

        const { accessToken } = response.data;
        localStorage.setItem('token', accessToken);
        return accessToken;
    } catch (error) {
        console.error('Ошибка при обновлении токена:', error);
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        return null;
    }
};

export const login = async ({ email, password }) => {
    const response = await axios.post(`${API_BACKEND_URL}/auth/login`, { email, password }, {
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        credentials: 'include'
    });
    return response;
}

export const register = async (data) => {
    const response = await axios.post(`${API_BACKEND_URL}/auth`, data, {
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        credentials: 'include'
    });
    console.log(data);
    return response;
}