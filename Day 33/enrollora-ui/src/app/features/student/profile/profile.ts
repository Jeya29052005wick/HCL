import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  StudentResponse,
  UpdateStudentRequest,
} from '../../admin/students/student.models';

import { StudentService } from '../../admin/students/student.service';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class StudentProfile implements OnInit {

  private service = inject(StudentService);

  profile = signal<StudentResponse | null>(null);

  isLoading = signal(true);
  isSaving = signal(false);
  isEditing = signal(false);

  error = signal('');
  success = signal('');

  form: UpdateStudentRequest = {
    name: '',
    email: '',
    department: '',
    age: 17,
    cgpa: 0,
  };

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading.set(true);
    this.error.set('');

    this.service.getMe().subscribe({
      next: (student) => {
        this.profile.set(student);
        this.copyToForm(student);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(this.getErrorMessage(error));
        this.isLoading.set(false);
      },
    });
  }

  beginEdit(): void {
    const student = this.profile();

    if (!student) return;

    this.copyToForm(student);
    this.isEditing.set(true);
    this.success.set('');
    this.error.set('');
  }

  cancelEdit(): void {
    const student = this.profile();

    if (student) {
      this.copyToForm(student);
    }

    this.isEditing.set(false);
    this.error.set('');
  }

  save(): void {
    this.error.set('');
    this.success.set('');
    this.isSaving.set(true);

    this.service.updateMe(this.form).subscribe({
      next: (student) => {
        this.profile.set(student);
        this.copyToForm(student);

        this.isEditing.set(false);
        this.isSaving.set(false);

        this.success.set('Profile updated successfully.');
      },
      error: (error: HttpErrorResponse) => {
        this.error.set(this.getErrorMessage(error));
        this.isSaving.set(false);
      },
    });
  }

  private copyToForm(student: StudentResponse): void {
    this.form = {
      name: student.name,
      email: student.email,
      department: student.department,
      age: student.age,
      cgpa: student.cgpa,
    };
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      return body;
    }

    return (
      body?.detail ||
      body?.message ||
      body?.title ||
      'Unable to process your profile request.'
    );
  }
}