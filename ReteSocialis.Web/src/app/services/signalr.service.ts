// src/app/services/signalr.service.ts
import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject, Observable } from 'rxjs';
import { Post } from '../models/post.model';
import { environment } from '../environments/environment';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection?: signalR.HubConnection;
  private newPostSubject = new Subject<Post>();

  constructor(private auth: AuthService, private ngZone: NgZone) {
    // quando fizer login no AuthService -> inicia conexao SignalR
    this.auth.login$.subscribe(() => this.startConnection());
    // quando logout -> para conexao
    this.auth.logout$.subscribe(() => this.stop());

    // se já houver token (reload de página), tenta conectar
    if (this.auth.getToken()) {
      this.startConnection();
    }
  }

  startConnection() {
    if (this.hubConnection) return;

    const token = this.auth.getToken() ?? '';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalRHubUrl, { accessTokenFactory: () => token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR conectado'))
      .catch(err => console.error('SignalR start error', err));

    this.hubConnection.on('NewPost', (post: Post) => {
      this.ngZone.run(() => this.newPostSubject.next(post));
    });
  }

  onNewPost(): Observable<Post> {
    return this.newPostSubject.asObservable();
  }

  stop() {
    this.hubConnection?.stop().catch(() => {});
    this.hubConnection = undefined;
  }
}
