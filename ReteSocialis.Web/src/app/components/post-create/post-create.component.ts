import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-post-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.css']
})
export class PostCreateComponent {
  content = '';
  saving = false;
  error = '';

  constructor(private api: ApiService) {}

  submit() {
    if (!this.content.trim()) return;
    this.saving = true;

    this.api.createPost(this.content).subscribe({
      next: () => {
        this.content = '';
        this.saving = false;
      },
      error: () => {
        this.error = 'Erro ao publicar';
        this.saving = false;
      }
    });
  }
}
