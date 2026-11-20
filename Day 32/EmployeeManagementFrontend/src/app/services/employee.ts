
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Employee {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  departmentId: number;
  departmentName: string;
  joiningDate: string;
}

export interface EmployeeRequest {
  name: string;
  email: string;
  phone: string;
  address: string;
  departmentId: number;
  joiningDate: string;
}

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
  getEmployee(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  // Add employee
  addEmployee(data: EmployeeRequest): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, data);
  }

  // Update employee
  updateEmployee(id: number, data: EmployeeRequest): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, data);
  }

  // Delete employee
  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
