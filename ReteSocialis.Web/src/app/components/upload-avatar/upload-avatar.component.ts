import { Component, ElementRef, EventEmitter, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import Cropper from 'cropperjs'; // ✅ import correto (classe default)
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-upload-avatar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload-avatar.component.html',
  styleUrls: ['./upload-avatar.component.css']
})
export class UploadAvatarComponent {
  @Output() avatarChange = new EventEmitter<string>();

  @ViewChild('imagePreview', { static: false }) imagePreview!: ElementRef<HTMLImageElement>;

  previewUrl: string | null = null;
  message = '';
  cropper?: any;
  uploading = false;

  private readonly uploadUrl = 'http://localhost:5000/api/users/avatar';

  constructor(private http: HttpClient, private auth: AuthService) {}

  // Quando o usuário seleciona o arquivo
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    if (!file.type.startsWith('image/')) {
      this.message = 'Por favor, selecione uma imagem válida.';
      return;
    }

    const reader = new FileReader();
    reader.onload = e => {
      this.previewUrl = e.target?.result as string;
      setTimeout(() => this.initializeCropper(), 0);
    };
    reader.readAsDataURL(file);
  }

  // Inicializa o recorte da imagem
  initializeCropper() {
    if (this.cropper) this.cropper.destroy();

    const imageElement = this.imagePreview?.nativeElement;
    if (!imageElement) return;

    this.cropper = new Cropper(imageElement, {
      aspectRatio: 1,
      viewMode: 1,
      dragMode: 'move',
      background: false,
      responsive: true,
      autoCropArea: 1,
    });
  }

  // Recorta e envia o avatar
  cropAndUpload() {
    if (!this.cropper) return;

    this.uploading = true;
    this.message = '';

    this.cropper
      .getCroppedCanvas({
        width: 300,
        height: 300,
      })
      .toBlob((blob: Blob | null) => {
        if (!blob) {
          this.message = 'Erro ao gerar imagem.';
          this.uploading = false;
          return;
        }

        const formData = new FormData();
        formData.append('file', blob, 'avatar.png');

        const token = this.auth.getToken();
        const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

        this.http.post<{ avatarUrl: string }>(this.uploadUrl, formData, { headers }).subscribe({
          next: res => {
            this.message = 'Avatar atualizado com sucesso!';
            this.uploading = false;
            this.avatarChange.emit(res.avatarUrl);
          },
          error: err => {
            console.error('Erro no upload:', err);
            this.message = 'Erro ao enviar avatar.';
            this.uploading = false;
          },
        });
      }, 'image/png');
  }
}
