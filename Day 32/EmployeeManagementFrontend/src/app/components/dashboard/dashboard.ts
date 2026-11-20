
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  role: string | null = null;
  email: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.role = this.authService.getRole();
    this.email = localStorage.getItem('email');
  }

  openEmployees(): void {
    this.router.navigate(['/employees']);
  }

  openDepartments(): void {
    this.router.navigate(['/departments']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
