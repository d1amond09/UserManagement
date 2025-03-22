import React, { useState } from 'react';
import { login } from '../services/auth';
import { useLocation, useNavigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const Login = () => {
    const [rememberMe, setRememberMe] = useState(false);
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const location = useLocation();
    const { signIn } = useAuth();

    const fromPage = location.state?.from.pathname || "/users";

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const response = await login({ email, password });
            if (response.status == 200) {
                const { accessToken, refreshToken } = await response.data;
                signIn(accessToken, refreshToken);
                navigate(fromPage, { replace: true });
            } else if (response.status === 401) {
                setError('ERROR Sign In. Please, check your credentials.');
            } else {
                const data = await response.data;
                setError(data.message || 'ERROR. Please, try later...');
            }
        } catch (err) {
            setError('ERROR. Please, try later...');
            console.error(err);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100 vw-100 bg-dark text-white">
            <div className="card p-4 w-50">
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
                    <div className="form-group mb-3 form-check">
                        <input
                            type="checkbox"
                            className="form-check-input"
                            id="rememberMe"
                            checked={rememberMe}
                            onChange={() => setRememberMe(!rememberMe)}
                        />
                        <label className="form-check-label" htmlFor="rememberMe">Remember me</label>
                    </div>
                    <button type="submit" className="btn btn-primary w-100">Sign in</button>

                    {error && <p color="red.500">{error}</p>}
                </form>
                <p className="mt-3 text-center">
                    Don't have an account? <a href="/register" className="text-info">Sign up</a>
                </p>
            </div>
        </div>
    );
};

export default Login;