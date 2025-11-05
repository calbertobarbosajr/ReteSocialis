import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface Friend {
  id: string;
  userName: string;
  email: string;
  since?: Date;
}

@Component({
  selector: 'app-friends',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit, OnDestroy {
  friends: Friend[] = [];
  newFriendEmail = '';
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadFriends();
    this.startHub();
  }

  ngOnDestroy() {
    this.hubConnection?.stop();
  }

  loadFriends() {
    this.http.get<Friend[]>(`${environment.apiBaseUrl}/friends`)
      .subscribe({
        next: data => this.friends = data,
        error: err => console.error('Erro ao carregar amigos', err)
      });
  }

  sendRequest() {
    if (!this.newFriendEmail.trim()) return;

    this.http.post(`${environment.apiBaseUrl}/friends/invite`, {
      receiverEmail: this.newFriendEmail
    }).subscribe({
      next: () => {
        alert(`Convite enviado para ${this.newFriendEmail}`);
        this.newFriendEmail = '';
      },
      error: () => alert('Erro ao enviar convite.')
    });
  }

  startHub() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl.replace('/api', '')}/hubs/friends`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveFriendRequest', (fromUser: string) => {
      alert(`📬 Novo pedido de amizade de ${fromUser}!`);
      this.loadFriends();
    });

    this.hubConnection.start()
      .catch(err => console.error('Erro ao conectar ao hub:', err));
  }
}
