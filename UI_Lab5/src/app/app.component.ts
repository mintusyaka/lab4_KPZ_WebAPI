import { Component } from '@angular/core';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'UI_Lab5';

  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    // Перевірка авторизації при завантаженні
    if (this.authService.getToken()) {
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }
  }
  
  goToAbout() {
    this.router.navigate(['about']);
  }

  exit() {
    this.authService.logout();
    this.router.navigate(['registration']);
  }

  goToCharacters() {
    this.router.navigate(['characters']);
  }
}
