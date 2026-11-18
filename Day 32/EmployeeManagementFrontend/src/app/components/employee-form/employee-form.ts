
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges
} from '@angular/core';

import { FormsModule } from '@angular/forms';

import { Employee, CreateEmployee, UpdateEmployee } from '../../models/employee.model';
import { Department } from '../../models/department.model';

import { EmployeeService } from '../../services/employee';
import { DepartmentService } from '../../services/department';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css'
})
export class EmployeeForm implements OnChanges {

  @Input() employee: Employee | null = null;

  @Output() employeeSaved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  departments: Department[] = [];

  name = '';
  email = '';
  phone = '';
  address = '';
  departmentId: number | null = null;
  joiningDate = '';

  errorMessage = '';

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['employee']) {

      this.errorMessage = '';

      if (this.employee) {
        this.name = this.employee.name;
        this.email = this.employee.email;
        this.phone = this.employee.phone;
        this.address = this.employee.address ?? '';
        this.departmentId = this.employee.departmentId;
        this.joiningDate = this.employee.joiningDate.substring(0, 10);
      } else {
        this.resetForm();
      }
    }
  }

  loadDepartments(): void {

    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (error) => {
        this.errorMessage = 'Unable to load departments.';
        console.error(error);
      }
    });
  }

  saveEmployee(): void {

    if (!this.name.trim() || !this.email.trim() || !this.phone.trim()) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    if (this.departmentId === null) {
      this.errorMessage = 'Please select a department.';
      return;
    }

    if (!this.joiningDate) {
      this.errorMessage = 'Please select a joining date.';
      return;
    }

    this.errorMessage = '';

    const data = {
      name: this.name.trim(),
      email: this.email.trim(),
      phone: this.phone.trim(),
      address: this.address.trim() || null,
      departmentId: this.departmentId,
      joiningDate: this.joiningDate
    };

    if (this.employee) {

      const updateData: UpdateEmployee = data;

      this.employeeService.updateEmployee(
        this.employee.id,
        updateData
      ).subscribe({

        next: () => {
          this.employeeSaved.emit();
        },

        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Failed to update employee';

          console.error(error);
        }

      });

    } else {

      const createData: CreateEmployee = data;

      this.employeeService.createEmployee(createData).subscribe({

        next: () => {
          this.employeeSaved.emit();
        },

        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Failed to add employee';

          console.error(error);
        }

      });
    }
  }

  resetForm(): void {
    this.name = '';
    this.email = '';
    this.phone = '';
    this.address = '';
    this.departmentId = null;
    this.joiningDate = '';
    this.errorMessage = '';
  }

  cancelForm(): void {
    this.resetForm();
    this.cancelled.emit();
  }
}
