// registration.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
})
export class RegistrationComponent {
  registrationForm: FormGroup;

  isNameValid: boolean = true;
  isEmailValid: boolean = true;
  isPasswordValid: boolean = true;
  isConfirmPasswordValid: boolean = true;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.registrationForm = this.fb.group(
      {
        name: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
        sex: ['', [Validators.required]]
      },
      {
        validators: this.passwordsMatchValidator,
      }
    );

    authService.logout();
  }

  isInvalid(controlName: string): boolean {
    const control = this.registrationForm.get(controlName);
    let res = !!(control?.invalid && (control?.touched || control?.dirty));
    return res;
  }


  passwordsMatchValidator(form: FormGroup): null | { mismatch: true } {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  goToLogin() {
    this.router.navigate(['login']);
  }

  onSubmit() {
    this.isNameValid = true;
    this.isEmailValid = true;
    this.isPasswordValid = true;
    this.isConfirmPasswordValid = true;
    if (this.registrationForm.valid) {
      var data = {
        name: this.registrationForm.value.name,
        password: this.registrationForm.value.password,
        email: this.registrationForm.value.email,
        sex: this.registrationForm.value.sex
      }


      this.authService.register(data).subscribe({
        next: (response) => {
          this.authService.saveToken(response.token);
          localStorage.setItem('email', this.registrationForm.value.email);
          this.router.navigate(['characters']); // Перехід на сторінку входу після реєстрації
        },
        error: (error) => {
          this.isNameValid = false;
          this.isEmailValid = false;
          this.isPasswordValid = false;
          this.isConfirmPasswordValid = false;
          console.error(error);
        },
      });

      // Додати логіку для обробки форми (API виклик або збереження даних)
    } else {
      if (this.isInvalid('name')) {
        this.isNameValid = false;
      }
      if (this.isInvalid('email')) {
        this.isEmailValid = false;
      }
      if (this.isInvalid('password')) {
        this.isPasswordValid = false;
      }
      if (this.passwordsMatchValidator(this.registrationForm)) {
        this.isConfirmPasswordValid = false;
      }/*
      if (!(control?.invalid && (control?.touched || control?.dirty))) {
        this.isPasswordValid = false;
      }
      if (!(control?.invalid && (control?.touched || control?.dirty))) {
        this.isConfirmPasswordValid = false;
      }*/
    }
  }
}
