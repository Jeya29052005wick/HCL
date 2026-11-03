import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './core/auth.service';
@Component({selector:'st-root',standalone:true,imports:[RouterOutlet,RouterLink,RouterLinkActive],template:`
<header class="topbar"><a class="brand" routerLink="/"><span class="brand-mark">✦</span><span>SECRET <b>TOMATOES</b><small>YOUR PRIVATE CINEMA CLUB</small></span></a><nav><a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact:true}">Discover</a><a routerLink="/movies" routerLinkActive="active">Movies</a><a routerLink="/movies">What's on</a>@if(auth.hasRole('Admin','TheatreManager')){<a routerLink="/manage">Studio</a>}</nav><div class="top-actions">@if(auth.isLoggedIn){<span class="welcome">Hi, {{auth.name() || 'Member'}}</span><button class="btn btn-quiet" (click)="auth.logout()">Sign out</button>}@else{<a class="signin" routerLink="/login">Sign in</a><a class="btn btn-primary btn-small" routerLink="/register">Join the club</a>}</div></header>
<main><router-outlet /></main><footer class="footer"><div class="footer-brand">SECRET <b>TOMATOES</b><p>Great stories. Better nights.</p></div><div class="footer-note">A more considered way to go to the movies.<br>© 2026 Secret Tomatoes</div></footer>
`}) export class App {constructor(public auth:AuthService){}}
