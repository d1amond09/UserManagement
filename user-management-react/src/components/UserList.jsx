import React, { useEffect, useState } from 'react';
import { fetchUsers } from '../services/users';
import { format, parseISO } from 'date-fns';

const UserList = () => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const getUsers = async () => {
            const response = await fetchUsers();
            setUsers(response.users);
        };
        getUsers();
    }, []);

    const handleBlockUser = async (userId) => {
        try {
            await blockUser(userId);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="vh-100 vw-100 mt-2">
            <h1 className="text-center mb-4">User Management</h1>
            <div className="table-responsive">
                <table className="table table-dark table-striped">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Name</th>
                            <th scope="col">Email</th>
                            <th scope="col">Last Seen</th>
                            <th scope="col">Registration Time</th>
                            <th scope="col">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => (
                            <tr key={user.id}>
                                <th scope="row">{index + 1}</th>
                                <td>{user.name}</td>
                                <td>{user.email}</td>
                                <td>{format(parseISO(user.lastLogin), 'dd MMM yyyy HH:mm:ss')}</td>
                                <td>{format(parseISO(user.registrationTime), 'dd MMM yyyy HH:mm:ss')}</td>
                                <td>
                                    <button
                                        className="btn btn-danger"
                                        onClick={() => handleBlockUser(user.id)}
                                    >
                                        Block
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default UserList;