
import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Dashboard } from './components/dashboard/dashboard';
import { authGuard } from './guards/auth.guard';
import { DepartmentComponent } from './components/department/department';
import { EmployeeComponent } from './components/employee/employee';

export const routes: Routes = [

  {
    path: 'login',
    component: Login
  },

  {
    path: '',
    component: Dashboard,
    canActivate: [authGuard],
    data: { roles: ['Manager'] }
  },

  {
  path: 'employees',
  component: EmployeeComponent,
  canActivate: [authGuard],
  data: { roles: ['Manager', 'HR'] }
},

  {
  path: 'departments',
  component: DepartmentComponent,
  canActivate: [authGuard],
  data: { roles: ['Manager'] }
},

  {
    path: '**',
    redirectTo: 'login'
  }

];
