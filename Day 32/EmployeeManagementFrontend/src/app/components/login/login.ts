
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  email = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.errorMessage = '';

    const loginData = {
      email: this.email,
      password: this.password
    };

    this.authService.login(loginData).subscribe({

      next: (response) => {

        if (response.role === 'Manager') {
          this.router.navigate(['/']);
        }
        else if (response.role === 'HR') {
          this.router.navigate(['/employees']);
        }
        else {
          this.authService.logout();
          this.errorMessage = 'Invalid user role.';
        }

      },

      error: () => {
        this.errorMessage = 'Invalid email or password.';
      }

    });
  }
}
