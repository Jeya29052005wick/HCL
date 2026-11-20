
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  EmployeeService,
  Employee,
  EmployeeRequest
} from '../../services/employee';

import {
  DepartmentService,
  Department
} from '../../services/department';

@Component({
  selector: 'app-employee',
  imports: [FormsModule,DatePipe],
  templateUrl: './employee.html',
  styleUrl: './employee.css'
})
export class EmployeeComponent implements OnInit {

  employees: Employee[] = [];
  departments: Department[] = [];

  employee: EmployeeRequest = {
    name: '',
    email: '',
    phone: '',
    address: '',
    departmentId: 0,
    joiningDate: ''
  };

  editingId: number | null = null;
  currentView: string = 'view';

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartments();
  }

  // Load employees
  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load employees.';
        this.cdr.detectChanges();
      }
    });
  }

  // Load departments for dropdown
  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load departments.';
        this.cdr.detectChanges();
      }
    });
  }

  // Open Add form
  openAdd(): void {
    this.resetForm();
    this.currentView = 'add';
  }

  // Open View page
  openView(): void {
    this.resetForm();
    this.currentView = 'view';
    this.loadEmployees();
  }

  // Add or Update employee
  saveEmployee(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.employee.name.trim() ||
        !this.employee.email.trim() ||
        !this.employee.phone.trim() ||
        !this.employee.joiningDate ||
        this.employee.departmentId <= 0) {

      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    if (this.editingId !== null) {

      this.employeeService.updateEmployee(
        this.editingId,
        this.employee
      ).subscribe({
        next: () => {
          this.successMessage = 'Employee updated successfully!';
          this.resetForm();
          this.currentView = 'view';
          this.loadEmployees();
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to update employee.';
          this.cdr.detectChanges();
        }
      });

    } else {

      this.employeeService.addEmployee(this.employee).subscribe({
        next: () => {
          this.successMessage = 'Employee added successfully!';
          this.resetForm();
          this.currentView = 'view';
          this.loadEmployees();
          this.cdr.detectChanges();
        },
        error: () => {
          this.errorMessage = 'Unable to add employee. Check the details.';
          this.cdr.detectChanges();
        }
      });
    }
  }

  // Edit employee
  editEmployee(item: Employee): void {

    this.editingId = item.id;

    this.employee = {
      name: item.name,
      email: item.email,
      phone: item.phone,
      address: item.address ?? '',
      departmentId: item.departmentId,
      joiningDate: item.joiningDate
        ? item.joiningDate.substring(0, 10)
        : ''
    };

    this.errorMessage = '';
    this.successMessage = '';
    this.currentView = 'add';
  }

  // Delete employee
  deleteEmployee(id: number): void {

    const confirmDelete = confirm(
      'Are you sure you want to delete this employee?'
    );

    if (!confirmDelete) {
      return;
    }

    this.employeeService.deleteEmployee(id).subscribe({
      next: () => {
        this.successMessage = 'Employee deleted successfully!';
        this.loadEmployees();
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to delete employee.';
        this.cdr.detectChanges();
      }
    });
  }

  // Reset form
  resetForm(): void {
    this.employee = {
      name: '',
      email: '',
      phone: '',
      address: '',
      departmentId: 0,
      joiningDate: ''
    };

    this.editingId = null;
    this.errorMessage = '';
  }
}
