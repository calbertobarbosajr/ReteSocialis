import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hubConnection!: signalR.HubConnection;
  private messagesSource = new BehaviorSubject<any[]>([]);
  messages$ = this.messagesSource.asObservable();

  connect(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`/hubs/chat`, { accessTokenFactory: () => userId })
      .build();

    this.hubConnection.on('ReceiveMessage', (senderId, message, timestamp) => {
      const newMessage = { senderId, message, timestamp };
      const current = this.messagesSource.value;
      this.messagesSource.next([...current, newMessage]);
    });

    this.hubConnection.start().then(() => console.log('🟢 Conectado ao ChatHub'));
  }

  sendMessage(receiverId: string, message: string) {
    return this.hubConnection.invoke('SendMessage', receiverId, message);
  }
}
