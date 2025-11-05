import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface ChatMessage {
  text: string;
  timestamp: Date;
  sender: string;
}

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent {
  message: string = '';
  userName: string = 'Você';
  messages: ChatMessage[] = [
    { text: 'Olá! Tudo bem?', timestamp: new Date(), sender: 'Amigo' },
    { text: 'Oi! Tudo ótimo 😄', timestamp: new Date(), sender: 'Você' }
  ];

  sendMessage() {
    if (!this.message.trim()) return;

    this.messages.push({
      text: this.message,
      timestamp: new Date(),
      sender: this.userName
    });

    this.message = '';
  }
}
