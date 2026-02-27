import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  template: `
  <div class="container mt-5">
    <h3>Register</h3>
    <form [formGroup]="form" (ngSubmit)="submit()">
      <div class="mb-3">
        <input class="form-control" formControlName="username" placeholder="Username" />
      </div>
      <div class="mb-3">
        <input class="form-control" type="password" formControlName="password" placeholder="Password" />
      </div>
      <div class="mb-3">
        <select class="form-control" formControlName="role">
          <option value="Buyer">Buyer</option>
          <option value="Seller">Seller</option>
          <option value="Admin">Admin</option>
        </select>
      </div>
      <button class="btn btn-primary">Register</button>
    </form>
  </div>
  `
})
export class RegisterComponent {
  form: FormGroup;
  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.form = this.fb.group({ username: [''], password: [''], role: ['Buyer'] });
  }

  submit() {
    const val = this.form.value;
    this.auth.register(val).subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}
