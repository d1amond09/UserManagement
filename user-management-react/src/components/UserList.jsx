import React, { useEffect, useState } from 'react';
import { fetchUsers, blockUser, unblockUser, deleteUser } from '../services/users';
import { format, parseISO } from 'date-fns';
import { useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';
import { Tooltip, OverlayTrigger } from 'react-bootstrap';
import * as Icon from 'react-bootstrap-icons';

const UserList = () => {
    const [users, setUsers] = useState([]);
    const [selectedUsers, setSelectedUsers] = useState(new Set());
    const [statusMessage, setStatusMessage] = useState('');
    const { user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        const getUsers = async () => {
            const response = await fetchUsers();
            setUsers(response.users);
        };
        getUsers();
    }, [user, navigate]);

    const handleToggleUserSelection = (userId) => {
        const updatedSelection = new Set(selectedUsers);
        if (updatedSelection.has(userId)) {
            updatedSelection.delete(userId);
        } else {
            updatedSelection.add(userId);
        }
        setSelectedUsers(updatedSelection);
    };

    const handleBlockUsers = async () => {
        try {
            const userIds = Array.from(selectedUsers);
            console.log(userIds);
            await blockUser(userIds);
            setStatusMessage('Users have been successfully blocked.');
            await refreshUsers();
        } catch (error) {
            setStatusMessage('Error blocking users.');
        }
    };

    const handleUnblockUsers = async () => {
        try {
            const userIds = Array.from(selectedUsers);
            console.log(userIds);
            await unblockUser(userIds);
            setStatusMessage('Users have been successfully unblocked.');
            await refreshUsers();
        } catch (error) {
            setStatusMessage('Error unblocking users.');
        }
    };

    const handleDeleteUsers = async () => {
        try {
            for (const userId of selectedUsers) {
                await deleteUser(userId);
            }
            setStatusMessage('Users have been successfully deleted.');
            await refreshUsers();
        } catch (error) {
            setStatusMessage('Error deleting users.');
        }
    };

    const refreshUsers = async () => {
        const response = await fetchUsers();
        setUsers(response.users);
        setSelectedUsers(new Set());
    };

    return (
        <div className="vh-100 vw-100">
            <h1 className="text-center mb-4">User Management</h1>
            {statusMessage && <div className="alert alert-info">{statusMessage}</div>}
            <div className="m-2">
                <button className="btn btn-warning me-2" onClick={handleBlockUsers}>
                    Block
                </button>
                <OverlayTrigger
                    placement="top"
                    overlay={<Tooltip id="unblock-tooltip">Unblock selected users</Tooltip>}
                >
                    <button className="btn btn-success me-2" onClick={handleUnblockUsers}>
                        <Icon.Unlock />
                    </button>
                </OverlayTrigger>
                <OverlayTrigger
                    placement="top"
                    overlay={<Tooltip id="delete-tooltip">Delete selected users</Tooltip>}
                >
                    <button className="btn btn-danger" onClick={handleDeleteUsers}>
                       <Icon.Trash/>
                    </button>
                </OverlayTrigger>
            </div>
            <div className="table-responsive m-1">
                <table className="table table-dark table-striped">
                    <thead>
                        <tr>
                            <th scope="col">
                                <input
                                    type="checkbox"
                                    onChange={(e) => {
                                        if (e.target.checked) {
                                            setSelectedUsers(new Set(users.map(user => user.id)));
                                        } else {
                                            setSelectedUsers(new Set());
                                        }
                                    }}
                                />
                            </th>
                            <th scope="col">Name</th>
                            <th scope="col">Email</th>
                            <th scope="col">Last Seen</th>
                            <th scope="col">Registration Time</th>
                            <th scope="col">Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => (
                            <tr key={user.id}>
                                <td>
                                    <input
                                        type="checkbox"
                                        checked={selectedUsers.has(user.id)}
                                        onChange={() => handleToggleUserSelection(user.id)}
                                    />
                                </td>
                                <td>{user.name}</td>
                                <td>{user.email}</td>
                                <td>{user.lastLogin ? format(parseISO(user.lastLogin), 'dd MMM yyyy HH:mm:ss') : "No activity yet"}</td>
                                <td>{format(parseISO(user.registrationTime), 'dd MMM yyyy HH:mm:ss')}</td>
                                <td>{user.isBlocked ? "Blocked" : "Active"}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default UserList;