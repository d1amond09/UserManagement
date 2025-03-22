import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { AuthProvider } from './context/AuthContext';
import UserList from "./components/UserList";
import Register from "./components/Register";
import Login from "./components/Login";
import Layout from "./components/Layout";
import React from "react";

const App = () => {
    return (
        <Router>
            <AuthProvider>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route index element={<UserList />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/users" element={<UserList />} />
                    </Route>
                </Routes>
            </AuthProvider>
        </Router>
    );
};

export default App;