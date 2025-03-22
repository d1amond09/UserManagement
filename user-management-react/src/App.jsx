import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { AuthProvider } from './context/AuthContext';
import UserList from "./components/UserList";
import Register from "./components/Register";
import Login from "./components/Login";
import React from "react";

const App = () => {
    return (
        <Router>
            <AuthProvider>
                <Routes>
                    <Route path="/" element={<Login />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/users" element={<UserList />} />
                </Routes>
            </AuthProvider>
        </Router>
    );
};

export default App;