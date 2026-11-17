
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Department,
  CreateDepartment,
  UpdateDepartment
} from '../models/department.model';

@Injectable({
  providedIn: 'root'
})

export class DepartmentService {

  private apiUrl = 'http://localhost:5112/api/Department';

  constructor(private http: HttpClient) {}

  // Get all departments
  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(this.apiUrl);
  }

  // Get department by ID
  getDepartmentById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.apiUrl}/${id}`);
  }

  // Create department
  createDepartment(data: CreateDepartment): Observable<Department> {
    return this.http.post<Department>(this.apiUrl, data);
  }

  // Update department
  updateDepartment(
    id: number,
    data: UpdateDepartment
  ): Observable<Department> {
    return this.http.put<Department>(`${this.apiUrl}/${id}`, data);
  }

  // Delete department
  deleteDepartment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
export type { Department };

