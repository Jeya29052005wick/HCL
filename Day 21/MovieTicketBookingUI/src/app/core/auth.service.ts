import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { LoginResponse } from './models';
@Injectable({providedIn:'root'})
export class AuthService {
  private tokenKey='secret-tomatoes-token';
  token=signal<string|null>(localStorage.getItem(this.tokenKey));
  role=signal<string>(this.readRole(this.token()));
  name=signal<string>(this.readName(this.token()));
  constructor(private router:Router){}
  setSession(result:LoginResponse){localStorage.setItem(this.tokenKey,result.token);this.token.set(result.token);this.role.set(this.readRole(result.token));this.name.set(this.readName(result.token));}
  logout(){localStorage.removeItem(this.tokenKey);this.token.set(null);this.role.set('');this.name.set('');this.router.navigate(['/']);}
  get isLoggedIn(){return !!this.token();}
  hasRole(...roles:string[]){return roles.includes(this.role());}
  private decode(){const t=this.token();if(!t)return {};try{return JSON.parse(atob(t.split('.')[1].replace(/-/g,'+').replace(/_/g,'/')));}catch{return {};}}
  private readRole(token:string|null){if(!token)return '';try{const p=JSON.parse(atob(token.split('.')[1].replace(/-/g,'+').replace(/_/g,'/')));return p['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']||p.role||'';}catch{return '';}}
  private readName(token:string|null){if(!token)return '';try{const p=JSON.parse(atob(token.split('.')[1].replace(/-/g,'+').replace(/_/g,'/')));return p['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']||p.unique_name||'';}catch{return '';}}
}
