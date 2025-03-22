import React, { useEffect, useState } from 'react';
import axios from 'axios';

const UserList = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
            const response = await axios.get('/api/users');
            setUsers(response.data);
        };
        fetchUsers();
    }, []);

    const handleBlockUser = async (userId) => {
        try {
            await axios.post(`/api/users/block/${userId}`);
            setUsers(users.filter(user => user.id !== userId));
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center">User Management</h1>
            <table className="table table-dark table-striped mt-3">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Last Seen</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <tr key={user.id}>
                            <td>{user.name}</td>
                            <td>{user.email}</td>
                            <td>{user.lastSeen}</td>
                            <td>
                                <button className="btn btn-danger" onClick={() => handleBlockUser(user.id)}>Block</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default UserList;