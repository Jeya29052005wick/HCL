
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Employee,
  CreateEmployee,
  UpdateEmployee
} from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'http://localhost:5112/api/Employee';

  constructor(private http: HttpClient) {}

  // Get all employees
  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  // Get employee by ID
  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  // Create employee
  createEmployee(data: CreateEmployee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, data);
  }

  // Update employee
  updateEmployee(
    id: number,
    data: UpdateEmployee
  ): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, data);
  }

  // Delete employee
  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
