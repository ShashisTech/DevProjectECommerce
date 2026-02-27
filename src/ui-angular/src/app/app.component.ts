import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
  <div class="container">
    <h2>DevProjectECommerce</h2>
    <nav>
      <a routerLink="/login">Login</a> | <a routerLink="/register">Register</a>
    </nav>
    <router-outlet></router-outlet>
  </div>
  `
})
export class AppComponent { }
