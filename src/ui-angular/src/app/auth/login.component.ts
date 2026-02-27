import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  template: `
  <div class="container mt-5">
    <h3>Login</h3>
    <form [formGroup]="form" (ngSubmit)="submit()">
      <div class="mb-3">
        <input class="form-control" formControlName="username" placeholder="Username" />
      </div>
      <div class="mb-3">
        <input class="form-control" type="password" formControlName="password" placeholder="Password" />
      </div>
      <button class="btn btn-primary">Login</button>
    </form>
  </div>
  `
})
export class LoginComponent {
  form: FormGroup;
  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.group({ username: [''], password: [''] });
  }

  submit() {
    const val = this.form.value;
    this.auth.login(val).subscribe((res: any) => {
      localStorage.setItem('token', res.token);
      this.router.navigate(['/']);
    });
  }
}
