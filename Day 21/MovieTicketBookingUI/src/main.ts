import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { App } from './app/app';
import { routes } from './app/app.routes';
import { authInterceptor } from './app/core/auth.interceptor';
bootstrapApplication(App, { providers: [provideRouter(routes, withInMemoryScrolling({scrollPositionRestoration:'top'})), provideHttpClient(withInterceptors([authInterceptor]))] }).catch(err => console.error(err));
