import { API_BACKEND_URL } from '../config.js';
import { apiFetch } from './auth';

export const fetchUsers = async () => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users`, { method: "GET" });
        return { users: response.data };
    } catch (error) {
        console.error('Error fetching users:', error);
        return { users: [], totalPages: 0 };
    }
};

export const blockUser = async (userIds) => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users/block`, {
            method: "PUT",
            data: { userIds },
        });
        return response.data;
    } catch (error) {
        console.error('Error blocking users:', error);
        throw error;
    }
};

export const unblockUser = async (userIds) => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users/unblock`, {
            method: "PUT",
            data: { userIds },
        });
        return response.data;
    } catch (error) {
        console.error('Error blocking users:', error);
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
        console.error('Error deleting user:', error);
        throw error;
    }
};

export const getUser = async (id) => {
    try {
        const response = await apiFetch(`${API_BACKEND_URL}/users/${id}`, {
            method: "GET",
        });
        return {
            user: response.data,
        };
    } catch (error) {
        console.error('Error getting user:', error);
        throw error;
    }
};