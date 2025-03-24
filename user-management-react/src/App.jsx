import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { AuthProvider } from './context/AuthContext';
import PrivateRoute from "./components/PrivateRoute";
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
                        <Route index element={<Login />} />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/users" element={<PrivateRoute> <UserList /> </PrivateRoute> } />
                    </Route>
                </Routes>
            </AuthProvider>
        </Router>
    );
};

export default App;