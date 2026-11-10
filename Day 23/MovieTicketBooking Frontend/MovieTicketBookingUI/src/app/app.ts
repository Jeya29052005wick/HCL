
import { Component } from '@angular/core';
import {
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';
import { AuthService } from './core/auth.service';

@Component({
  selector: 'st-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  template: `
    <header class="topbar">

      <!-- BRAND -->
      <a class="brand" routerLink="/">
        <span class="brand-mark">🍅</span>

        <span class="brand-text">
          SECRET <b>TOMATOES</b>
          <small>YOUR CINEMA EXPERIENCE</small>
        </span>
      </a>

      <!-- NAVIGATION -->
      <nav class="main-nav">

        <a
          routerLink="/"
          routerLinkActive="active"
          [routerLinkActiveOptions]="{ exact: true }">
          Home
        </a>

        <a
          routerLink="/movies"
          routerLinkActive="active">
          Movies
        </a>

        <a routerLink="/movies">
          What's On
        </a>

        @if (auth.isLoggedIn && auth.hasRole('Customer')) {
          <a routerLink="/movies">
            Theatres
          </a>
        }

        @if (auth.hasRole('Admin', 'TheatreManager')) {
          <a
            routerLink="/manage"
            routerLinkActive="active">
            Studio
          </a>
        }

      </nav>

      <!-- RIGHT ACTIONS -->
      <div class="top-actions">

        @if (auth.isLoggedIn) {

          <div class="user-info">
            <span class="user-avatar">
              {{ (auth.name() || 'U').charAt(0).toUpperCase() }}
            </span>

            <div class="user-details">
              <span class="welcome">Welcome back</span>
              <strong>{{ auth.name() || 'Member' }}</strong>
            </div>
          </div>

          <button
            class="btn btn-quiet"
            (click)="auth.logout()">
            Sign out
          </button>

        } @else {

          <a class="signin" routerLink="/login">
            Login
          </a>

          <a class="btn btn-primary btn-small" routerLink="/register">
            Create Account
            <span>↗</span>
          </a>

        }

      </div>

    </header>

    <main>
      <router-outlet />
    </main>

    <footer class="footer">

      <div class="footer-brand">
        🍅 SECRET <b>TOMATOES</b>
        <p>Great stories. Better nights.</p>
      </div>

      <div class="footer-note">
        Your next favourite movie is waiting.
        <br>
        © 2026 Secret Tomatoes. All rights reserved.
      </div>

    </footer>
  `
})
export class App {
  constructor(public auth: AuthService) {}
}
