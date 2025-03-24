import React, { useState } from 'react';
import { login } from '../services/auth';
import { useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const Login = () => {
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const { signIn, logout } = useAuth();
    const navigate = useNavigate();
    const [statusMessage, setStatusMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatusMessage('');

        try {
            logout();
            const response = await login({ email, password });
            if (response.status == 200) {
                const { accessToken, refreshToken } = response.data;
                signIn(accessToken, refreshToken);
                navigate("/users", { replace: true });
            }
        } catch (err) {
            setStatusMessage(err.response.data.message);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100 vw-100 bg-dark text-white">
            {statusMessage && <div className="position-absolute alert alert-danger text-center" style={{ top: '75px', left: '50%', transform: 'translateX(-50%)', zIndex: 1 }}>{statusMessage}</div>}
            <div className="card p-4 w-75 rounded-5">
                <h1 className="text-center mb-4">Sign in</h1>
                <form onSubmit={handleSubmit}>
                    <div className="form-group mb-3">
                        <label htmlFor="email" className="form-label">Email:</label>
                        <input
                            type="email"
                            className="form-control"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group mb-3">
                        <label htmlFor="password" className="form-label">Password:</label>
                        <input
                            type="password"
                            className="form-control"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" className="btn btn-primary w-100">Sign in</button>
                </form>
                <p className="mt-3 text-center">
                    Don't have an account? <a href="/register" className="text-info">Sign up</a>
                </p>
            </div>
        </div>
    );
};

export default Login;