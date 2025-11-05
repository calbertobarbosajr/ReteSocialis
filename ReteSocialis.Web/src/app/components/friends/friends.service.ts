import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class FriendsService {
  private hubConnection!: signalR.HubConnection;

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/friends', { withCredentials: true })
      .build();

    this.hubConnection.start()
      .then(() => console.log('✅ Conectado ao FriendsHub'))
      .catch(err => console.error('Erro SignalR:', err));
  }

  onFriendRequestReceived(callback: (username: string) => void) {
    this.hubConnection.on('ReceiveFriendRequest', callback);
  }

  onFriendAccepted(callback: (username: string) => void) {
    this.hubConnection.on('FriendRequestAccepted', callback);
  }

  onFriendRemoved(callback: (username: string) => void) {
    this.hubConnection.on('FriendRemoved', callback);
  }
}
