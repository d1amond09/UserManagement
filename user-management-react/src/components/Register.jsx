import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../services/auth';

const Register = () => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            await register({ name, email, password });
            navigate('/login');
        } catch (err) {
            setError(err.response.data.message);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100 vw-100 bg-dark text-white">
            {error && <div className="position-absolute alert alert-danger text-center" style={{ top: '75px', left: '50%', transform: 'translateX(-50%)', zIndex: 1 }}>{error}</div>}
            <div className="card p-4 w-50 rounded-5">
                <h1 className="text-center mb-4">Sign up</h1>
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label htmlFor="name" className="form-label">Name:</label>
                        <input
                            type="text"
                            className="form-control"
                            id="name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                        />
                    </div>
                    <div className="mb-3">
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
                    <div className="mb-3">
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
                    <button type="submit" className="btn btn-primary w-100">Sign up</button>
                </form>
                <p className="mt-3 text-center">
                    Already have an account? <a href="/login" className="text-info">Sign in</a>
                </p>
            </div>
        </div >
    );
};

export default Register;