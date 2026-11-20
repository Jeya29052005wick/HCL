
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  DepartmentService,
  Department,
  DepartmentRequest
} from '../../services/department';

@Component({
  selector: 'app-department',
  imports: [FormsModule],
  templateUrl: './department.html',
  styleUrl: './department.css'
})
export class DepartmentComponent implements OnInit {

  departments: Department[] = [];

  department: DepartmentRequest = {
    name: '',
    description: ''
  };

  editingId: number | null = null;

  currentView: string = 'view';

  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  // Load all departments
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
    this.loadDepartments();
  }

  // Add or Update department
  saveDepartment(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.department.name.trim()) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    if (this.editingId !== null) {

      // Update department
      this.departmentService.updateDepartment(
        this.editingId,
        this.department
      ).subscribe({

        next: () => {
          this.successMessage = 'Department updated successfully!';

          this.resetForm();
          this.currentView = 'view';

          this.loadDepartments();
          this.cdr.detectChanges();
        },

        error: () => {
          this.errorMessage = 'Unable to update department.';
          this.cdr.detectChanges();
        }

      });

    } else {

      // Add department
      this.departmentService.addDepartment(
        this.department
      ).subscribe({

        next: () => {
          this.successMessage = 'Department added successfully!';

          this.resetForm();
          this.currentView = 'view';

          this.loadDepartments();
          this.cdr.detectChanges();
        },

        error: () => {
          this.errorMessage = 'Unable to add department.';
          this.cdr.detectChanges();
        }

      });

    }
  }

  // Edit department
  editDepartment(item: Department): void {

    this.editingId = item.id;

    this.department = {
      name: item.name,
      description: item.description ?? ''
    };

    this.errorMessage = '';
    this.successMessage = '';

    this.currentView = 'add';
  }

  // Delete department
  deleteDepartment(id: number): void {

    const confirmDelete = confirm(
      'Are you sure you want to delete this department?'
    );

    if (!confirmDelete) {
      return;
    }

    this.departmentService.deleteDepartment(id).subscribe({

      next: () => {
        this.successMessage = 'Department deleted successfully!';

        this.loadDepartments();
        this.cdr.detectChanges();
      },

      error: () => {
        this.errorMessage =
          'Unable to delete department. Employees may be assigned to it.';

        this.cdr.detectChanges();
      }

    });
  }

  // Reset form
  resetForm(): void {

    this.department = {
      name: '',
      description: ''
    };

    this.editingId = null;
    this.errorMessage = '';
  }

}
