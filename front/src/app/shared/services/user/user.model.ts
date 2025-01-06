export interface Transactions {
    sum: number;
    valute: string;
    emblem: string;
    title: string;
    subtitle: string;
}

export interface LoginData {
    email: string;
    password: string;
}
export interface SignUpData {
    email: string;
    fullName: string;
    phone: string;
    password: string;
}
export interface UserDetails {
    id: string;
    email: string;
    image: string;
    phone: string;
    fullName: string;
    passwordHash: string;
    birthDate: string;
    role: string;
    language: string;
    spendingLimit: number;
    totalBalance: number;
    historyTransfers: RecentUserSimple[];
}
export interface User {
    id: string;
    email: string;
    image: string;
    phone: string;
    fullName: string;
    passwordHash: string;
    birthDate: string;
    role: string;
    language: string;
    spendingLimit: number;
    totalBalance: number;
    historyTransfers: RecentUserSimple[];
}

export interface ErrorInfo{
    hasError: boolean;
    error: string;
}
export interface RecentUser{
    id: string;
    name: string;
    image: string;
    cardNumber: string;
}
export interface RecentUserSimple {
    id: string;
    cardNumber: string;
}