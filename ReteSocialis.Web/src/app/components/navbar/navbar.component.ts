import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})

export class NavbarComponent {
  isLogged$: Observable<boolean>;

  constructor(private auth: AuthService, private router: Router) {
    this.isLogged$ = this.auth.isLogged$;
  }

  goHome() {
  this.router.navigate(['/home']);
}

  logout() {
    this.auth.logout();
  }
}
