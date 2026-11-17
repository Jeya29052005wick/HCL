
import { Component, OnInit } from '@angular/core';

import { Department } from '../../models/department.model';
import { DepartmentService } from '../../services/department';
import { DepartmentForm } from '../department-form/department-form';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [DepartmentForm],
  templateUrl: './department-list.html',
  styleUrl: './department-list.css'
})
export class DepartmentList implements OnInit {

  departments: Department[] = [];

  selectedDepartment: Department | null = null;

  showForm: boolean = false;
  isLoading: boolean = false;

  constructor(
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  // Get all departments
  loadDepartments(): void {

    this.isLoading = true;

    this.departmentService.getDepartments().subscribe({

      next: (data) => {
        this.departments = data;
        this.isLoading = false;
      },

      error: (error) => {
        console.error('Error loading departments:', error);
        this.isLoading = false;
      }

    });
  }

  // Open form for adding
  openAddForm(): void {
    this.selectedDepartment = null;
    this.showForm = true;
  }

  // Open form for editing
  editDepartment(department: Department): void {
    this.selectedDepartment = { ...department };
    this.showForm = true;
  }

  // Called after successful add or update
  onDepartmentSaved(): void {
    this.showForm = false;
    this.selectedDepartment = null;
    this.loadDepartments();
  }

  // Called when form is cancelled
  onFormCancelled(): void {
    this.showForm = false;
    this.selectedDepartment = null;
  }

  // Delete department
  deleteDepartment(department: Department): void {

    const confirmed = confirm(
      `Are you sure you want to delete ${department.name}?`
    );

    if (!confirmed) {
      return;
    }

    this.departmentService.deleteDepartment(
      department.id
    ).subscribe({

      next: () => {
        alert('Department deleted successfully!');
        this.loadDepartments();
      },

      error: (error) => {
        alert(
          error.error?.message ||
          'Failed to delete department'
        );

        console.error(error);
      }

    });
  }
}
