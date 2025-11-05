import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-edit-account',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-account.component.html',
  styleUrls: ['./edit-account.component.css']
})
export class EditAccountComponent implements OnInit {
  form!: FormGroup;
  loading = false;
  successMessage = '';
  errorMessage = '';

  constructor(private fb: FormBuilder, private accountService: AccountService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      currentPassword: [''],
      newPassword: ['']
    });

    this.loadProfile();
  }

  loadProfile() {
    this.loading = true;
    this.accountService.getProfile().subscribe({
      next: data => {
        this.form.patchValue({
          username: data.userName,
          email: data.email
        });
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Erro ao carregar dados do perfil.';
      }
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const { username, email, currentPassword, newPassword } = this.form.value;

    // Atualiza username e email
    this.accountService.updateAccount({ username, email }).subscribe({
      next: () => {
        this.form = this.fb.group({
          userName: ['', Validators.required],
          email: ['', [Validators.required, Validators.email]],
          newPassword: ['']
        });

        // Se senha foi preenchida, altera
        if (currentPassword && newPassword) {
          this.accountService.changePassword(currentPassword, newPassword).subscribe({
            next: () => {
              this.loading = false;
              this.successMessage = '✅ Dados e senha atualizados com sucesso!';
            },
            error: () => {
              this.loading = false;
              this.errorMessage = '❌ Erro ao alterar senha.';
            }
          });
        } else {
          this.loading = false;
          this.successMessage = '✅ Dados atualizados com sucesso!';
        }
      },
      error: () => {
        this.loading = false;
        this.errorMessage = '❌ Erro ao atualizar conta.';
      }
    });
  }
}
