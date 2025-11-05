import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostCardComponent } from '../post-card/post-card.component';

@Component({
  selector: 'app-feed',
  standalone: true,
  imports: [CommonModule, PostCardComponent], // ✅ necessário para *ngFor
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent {
  posts = [
    { author: 'Ana Paula', content: 'Primeiro post da rede! 🎉', createdAt: new Date() },
    { author: 'Carlos Silva', content: 'Explorando o novo feed 🚀', createdAt: new Date() }
  ];
}
