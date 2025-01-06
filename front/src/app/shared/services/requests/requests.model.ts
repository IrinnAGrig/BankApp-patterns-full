export interface RequestsModel{
    id: string;
    name: string;
    receiverId: string;
    senderId: string;
    amount: number;
    valute: string;
    dueDate: string;
    phone: string;
    email: string;
    dateSent: string;
    closed: boolean;
    status: 'A' | 'D' | 'P';
    dateClosed: string;
    isRead: boolean;
}