
import { Component, OnInit } from '@angular/core';

import { Employee } from '../../models/employee.model';
import { EmployeeService } from '../../services/employee';

import { EmployeeForm } from '../employee-form/employee-form';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [EmployeeForm, DatePipe],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css'
})
export class EmployeeList implements OnInit {

  employees: Employee[] = [];

  selectedEmployee: Employee | null = null;

  showForm = false;
  isLoading = false;

  constructor(
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {

    this.isLoading = true;

    this.employeeService.getEmployees().subscribe({

      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },

      error: (error) => {
        console.error('Error loading employees:', error);
        this.isLoading = false;
      }

    });
  }

  openAddForm(): void {
    this.selectedEmployee = null;
    this.showForm = true;
  }

  editEmployee(employee: Employee): void {
    this.selectedEmployee = { ...employee };
    this.showForm = true;
  }

  onEmployeeSaved(): void {
    this.showForm = false;
    this.selectedEmployee = null;
    this.loadEmployees();
  }

  onFormCancelled(): void {
    this.showForm = false;
    this.selectedEmployee = null;
  }

  deleteEmployee(employee: Employee): void {

    const confirmed = confirm(
      `Are you sure you want to delete ${employee.name}?`
    );

    if (!confirmed) {
      return;
    }

    this.employeeService.deleteEmployee(employee.id).subscribe({

      next: () => {
        this.loadEmployees();
      },

      error: (error) => {
        alert(
          error.error?.message || 'Failed to delete employee'
        );

        console.error(error);
      }

    });
  }
}
