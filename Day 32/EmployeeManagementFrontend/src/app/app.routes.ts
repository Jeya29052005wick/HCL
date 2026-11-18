
import { Routes } from '@angular/router';

import { Dashboard } from './components/dashboard/dashboard';
import { DepartmentList } from './components/department-list/department-list';
import { EmployeeList } from './components/employee-list/employee-list';


export const routes: Routes = [

  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },

  {
    path: 'dashboard',
    component: Dashboard
  },

  {
    path: 'departments',
    component: DepartmentList
  },

  {
    path: 'employees',
    component: EmployeeList
  },

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];
