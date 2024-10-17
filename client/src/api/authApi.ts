import axios from './axiosInstance';
import { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth.types';

const authApi = {
    register: (payload: RegisterRequest) => axios.post<AuthResponse>('/api/v1/auth/register', payload),
    login: (payload: LoginRequest) => axios.post<AuthResponse>('/api/v1/auth/login', payload),
    refresh: (payload: { accessToken: string; refreshToken: string }) => axios.post<AuthResponse>('/api/v1/auth/refresh', payload),
    revoke: () => axios.post('/api/v1/auth/revoke'),
    me: () => axios.get('/api/v1/auth/me'),
    updateProfile: (payload: Partial<any>) => axios.put('/api/v1/auth/me', payload),
    changePassword: (payload: { oldPassword: string; newPassword: string }) => axios.put('/api/v1/auth/me/password', payload),
};

export default authApi;
