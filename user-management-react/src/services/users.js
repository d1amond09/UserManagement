import { API_BACKEND_URL } from '../config.js';
import { apiFetch } from './auth';

export const fetchUsers = async () => {
    try {
        const params = {};

        const response = await apiFetch(`${API_BACKEND_URL}/users`, {
            method: "GET",
            params: params,
        });
        console.log(response.data);
        return {
            users: response.data,
        };
    } catch (error) {
        console.error('Ошибка при получении пользователей:', error);
        return { users: [], totalPages: 0 };
    }
};

export const blockUser = async (userIds) => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users/block`, {
            method: "PUT",
            data: userIds,
        });
        return response.data;
    } catch (error) {
        console.error('Ошибка при блокировки пользователей:', error);
        throw error;
    }
};

export const deleteUser = async (id) => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users/${id}`, {
            method: "DELETE",
        });
        return response.status;
    } catch (error) {
        console.error('Ошибка при удалении пользователя:', error);
        throw error;
    }
};