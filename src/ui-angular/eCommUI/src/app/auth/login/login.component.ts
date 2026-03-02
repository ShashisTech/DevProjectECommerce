import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, UserRole } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userName = '';
  password = '';
  role: UserRole = 'Buyer';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    if (!this.userName || !this.password) {
      this.errorMessage = 'User name and password are required';
      return;
    }

    // For now, simulate login locally. In real app, call backend.
    this.authService.login(this.userName, this.role);

    // Navigate based on role
    if (this.role === 'Admin') {
      this.router.navigate(['/admin']);
    } else if (this.role === 'Seller') {
      this.router.navigate(['/seller']);
    } else {
      this.router.navigate(['/']);
    }
  }
}
