import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, UserRole } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model = {
    userName: '',
    email: '',
    password: '',
    role: 'Buyer' as UserRole
  };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    if (!this.model.userName || !this.model.email || !this.model.password) {
      return;
    }

    // In real app, call backend to create user, then log in or redirect.
    this.authService.login(this.model.userName, this.model.role);
    this.router.navigate(['/']);
  }
}
