import { Injectable } from '@angular/core';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  private socket: WebSocket | null = null;

  constructor(private notificationService: NotificationService) {}

  connect(userId: string): void {
    const url = `ws://localhost:5142/api/websocket/connect?userID=${userId}`;
    this.socket = new WebSocket(url);
  
    this.socket.onopen = () => {
      console.log('WebSocket connection established');
    };
  
    this.socket.onmessage = (event) => {
      console.log('Message received from server:');
      this.notificationService.updateNotifications(event.data);
      this.notificationService.addNotificationsNumber(1);
    };
  
    this.socket.onclose = (event) => {
      console.log('WebSocket connection closed:', event.reason);
    };
  
    this.socket.onerror = (error) => {
      console.error('WebSocket error:', error);
    };
  }

  sendMessage(message: string): void {
    if (this.socket && this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(message);
    } else {
      console.error('WebSocket is not open. Cannot send message.');
    }
  }

  disconnect(): void {
    if (this.socket) {
      this.socket.close();
      this.socket = null;
    }
  }
}