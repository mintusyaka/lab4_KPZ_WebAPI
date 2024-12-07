import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { HttpClientModule } from '@angular/common/http';

import { AuthService } from '../auth.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule],
})
export class LoginComponent {
  loginForm: FormGroup;

  isEmailValid: boolean = true;
  isPasswordValid: boolean = true;

  characters: any[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService 
  ) {
    this.loginForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]]
      }
    );
  }

  isInvalid(controlName: string): boolean {
    const control = this.loginForm.get(controlName);
    let res = !!(control?.invalid && (control?.touched || control?.dirty));
    return res;
  }

  onSubmit() {
    this.isEmailValid = true;
    this.isPasswordValid = true;
    if (this.loginForm.valid) {
      var data = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      };

      this.authService.login(data).subscribe({
        next: (response) => {
          this.authService.saveToken(response.token);
          localStorage.setItem('email', this.loginForm.value.email);
          this.router.navigate(['characters']);
        },
        error: (error) => {
          this.isEmailValid = false;
          this.isPasswordValid = false;
          console.error(error);
        },
      });
      
    } else {
      if (this.isInvalid('email')) {
        this.isEmailValid = false;
      }
      if (this.isInvalid('password')) {
        this.isPasswordValid = false;
      }
    }
  }

  goToRegistration() {
    /*this.apiService.getData('Characters').subscribe({
      next: (data) => {
        this.characters = data;
        console.log('Characters loaded: ', data[0]);
      },
      error: (error) => {
        console.error('Error loading characters: ', error);
      }
    });*/
    this.router.navigate(['registration']);
  }
}
