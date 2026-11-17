
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';

import { FormsModule } from '@angular/forms';

import {
  Department,
  CreateDepartment,
  UpdateDepartment
} from '../../models/department.model';

import { DepartmentService } from '../../services/department';

@Component({
  selector: 'app-department-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentForm implements OnChanges {

  @Input() department: Department | null = null;

  @Output() departmentSaved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  name: string = '';
  description: string = '';

  constructor(
    private departmentService: DepartmentService
  ) {}

  // Runs when the parent passes a different department
  ngOnChanges(changes: SimpleChanges): void {

    if (changes['department']) {

      if (this.department) {
        this.name = this.department.name;
        this.description = this.department.description ?? '';
      } else {
        this.resetForm();
      }
    }
  }

  // Add or update department
  saveDepartment(): void {

    if (!this.name.trim()) {
      alert('Department name is required');
      return;
    }

    if (this.department) {

      const data: UpdateDepartment = {
        name: this.name.trim(),
        description: this.description.trim() || null
      };

      this.departmentService.updateDepartment(
        this.department.id,
        data
      ).subscribe({
        next: () => {
          alert('Department updated successfully!');
          this.departmentSaved.emit();
        },
        error: (error) => {
          alert(
            error.error?.message ||
            'Failed to update department'
          );
          console.error(error);
        }
      });

    } else {

      const data: CreateDepartment = {
        name: this.name.trim(),
        description: this.description.trim() || null
      };

      this.departmentService.createDepartment(data).subscribe({
        next: () => {
          alert('Department added successfully!');
          this.departmentSaved.emit();
        },
        error: (error) => {
          alert(
            error.error?.message ||
            'Failed to add department'
          );
          console.error(error);
        }
      });
    }
  }

  resetForm(): void {
    this.name = '';
    this.description = '';
  }

  cancelForm(): void {
    this.resetForm();
    this.cancelled.emit();
  }
}
