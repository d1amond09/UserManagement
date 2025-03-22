import { jwtDecode } from 'jwt-decode';

export default function decodeJwt(token) {
    try {
        return jwtDecode(token);
    } catch (error) {
        console.error('Invalid token', error);
        return null;
    }
};

export function isTokenExpired(token) {
    const decoded = decodeJwt(token);
    if (!decoded) return true;

    const currentTime = Date.now() / 1000;
    return decoded.exp < currentTime;
}