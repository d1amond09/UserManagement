import { API_BACKEND_URL } from '../config.js';
import { apiFetch } from './authentication';

export const fetchUsers = async (filter) => {
    try {
        const params = {};
        if (filter.searchTerm) params.searchTerm = filter.searchTerm;

        if (filter.orderBy) {
            params.orderBy = filter.orderBy;
            if (filter.sortOrder) {
                params.orderBy += ` ${filter.sortOrder}`;
            }
        }

        const response = await apiFetch(`${API_BACKEND_URL}/users`, {
            method: "GET",
            params: params,
        });

        return {
            employees: response.data,
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