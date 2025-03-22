import { Outlet } from "react-router-dom";
import AuthDisplay from './AuthDisplay';

const Layout = () => {
    return (
        <>
            <header>
                <AuthDisplay />
            </header>
            <main>
                <Outlet />
            </main>
            <footer>
            </footer>
        </>
    );
}

export default Layout