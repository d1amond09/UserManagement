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
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState('');
    const [orderBy, setOrderBy] = useState('name');
    const [sortOrder, setSortOrder] = useState('asc');

    useEffect(() => {
        const getUsers = async () => {
            const filter = { searchTerm, orderBy, sortOrder };
            const response = await fetchUsers(filter);
            setUsers(response.users);
        };
        getUsers();
    }, [navigate, searchTerm, orderBy, sortOrder, selectedUsers]);

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
            await blockUser(userIds);
            setStatusMessage('Users have been successfully blocked.');
            await refreshUsers();
            if (user && userIds.includes(user.id)) {
                logout();
                navigate("/login");
            }
        } catch (error) {
            setStatusMessage('Error blocking users.');
        }
    };

    const handleUnblockUsers = async () => {
        try {
            const userIds = Array.from(selectedUsers);
            await unblockUser(userIds);
            setStatusMessage('Users have been successfully unblocked.');
            await refreshUsers();
        } catch (error) {
            setStatusMessage('Error unblocking users.');
        }
    };

    const handleDeleteUsers = async () => {
        try {
            let deleteMySelf = false;
            for (const userId of selectedUsers) {
                if (user && user.id == userId) {
                    deleteMySelf = true;
                }
                await deleteUser(userId);
            }
            setStatusMessage('Users have been successfully deleted.');
            await refreshUsers();
            if (deleteMySelf) {
                logout();
                navigate("/login");
            }
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
        <div className="vh-100 vw-100 bg-dark">
            <h1 className="text-center text-light">User Management</h1>
            <br/>
            {statusMessage && <div className="position-absolute alert alert-info text-center" style={{ top: '65px', left: '50%', transform: 'translateX(-50%)', zIndex: 1 }}>{statusMessage}</div>}
            <div className="m-3 mt-5 d-flex align-items-center">
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
                    <button className="btn btn-danger me-2" onClick={handleDeleteUsers}>
                        <Icon.Trash />
                    </button>
                </OverlayTrigger>
                <select
                    value={orderBy}
                    onChange={(e) => setOrderBy(e.target.value)}
                    className="form-select me-2"
                    style={{ width: 'auto' }} 
                >
                    <option value="name">Name</option>
                    <option value="email">Email</option>
                    <option value="registrationTime">Registration Time</option>
                    <option value="lastLogin">Last Seen</option>
                    <option value="isBlocked">Status</option>
                </select>
                <select
                    value={sortOrder}
                    onChange={(e) => setSortOrder(e.target.value)}
                    className="form-select me-2"
                    style={{ width: 'auto' }}  
                >
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
                <input
                    type="text"
                    placeholder="Search by name..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="form-control"
                    style={{ width: 'auto' }} 
                />
            </div>

            <div className="table-responsive m-3">
                <table className="table table-dark table-striped table-hover">
                    <thead>
                        <tr>
                            <th scope="col">
                                <input
                                    className="form-check-input"
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
                        {users.map((user) => (
                            <tr key={user.id}>
                                <td>
                                    <input
                                        className="form-check-input"
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