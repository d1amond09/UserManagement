import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const AuthDisplay = () => {
    const { user, logout } = useAuth();
    const color = "black";
    const name = user?.name || "User";

    return (
        <div className="d-flex justify-content-end position-absolute" style={{ top: '10px', right: '10px', zIndex: 1 }}>
            {user ? (
                <Link to="/login">
                    <button 
                        className={`btn btn-secondary ${color}`}
                        type="button" onClick={logout}>
                        Log out of {name}
                    </button>
                </Link>
            ) : (
                <Link to="/login">
                    <button className="btn btn-primary">Sign in</button>
                </Link>
            )}
        </div>
    );
}

export default AuthDisplay;