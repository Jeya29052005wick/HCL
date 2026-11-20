
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const authGuard: CanActivateFn = (route) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  // Check whether the user is logged in
  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  // Get allowed roles from route configuration
  const allowedRoles = route.data['roles'] as string[] | undefined;

  // Get the logged-in user's role
  const userRole = authService.getRole();

  // Check whether the user's role is allowed
  if (allowedRoles && !allowedRoles.includes(userRole ?? '')) {

    if (userRole === 'HR') {
      router.navigate(['/employees']);
    } else {
      router.navigate(['/']);
    }

    return false;
  }

  return true;
};
