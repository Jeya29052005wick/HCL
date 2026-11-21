import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CourseService } from '../course.service';
import { CourseResponse, SaveCourseRequest } from '../course.models';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {
  private courseService = inject(CourseService);

  courses = signal<CourseResponse[]>([]);

  isLoading = signal(false);
  errorMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);

  totalRecords = signal(0);
  totalPages = signal(0);

  // Add course modal state
  showAddModal = signal(false);
  isSubmitting = signal(false);
  addCourseError = signal('');
  addCourseSuccess = signal('');

  courseName = signal('');
  duration = signal('');
  fees = signal<number | null>(null);
  capacity = signal<number | null>(null);
  isActive = signal(true);

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService.getCourses(this.pageNumber(), this.pageSize()).subscribe({
      next: (response) => {
        this.courses.set(response.items);
        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading courses:', error);
        this.errorMessage.set('Unable to load courses.');
        this.isLoading.set(false);
      },
    });
  }

  openAddModal(): void {
    this.resetForm();
    this.showAddModal.set(true);
  }

  closeAddModal(): void {
    if (this.isSubmitting()) return;

    this.showAddModal.set(false);
    this.resetForm();
  }

  resetForm(): void {
    this.courseName.set('');
    this.duration.set('');
    this.fees.set(null);
    this.capacity.set(null);
    this.isActive.set(true);
    this.addCourseError.set('');
  }

  submitCourse(): void {
    const name = this.courseName().trim();
    const courseDuration = this.duration().trim();
    const courseFees = this.fees();
    const courseCapacity = this.capacity();

    this.addCourseError.set('');

    if (!name || !courseDuration) {
      this.addCourseError.set('Course name and duration are required.');
      return;
    }

    if (courseFees === null || courseFees < 0 || !Number.isFinite(courseFees)) {
      this.addCourseError.set('Enter valid course fees.');
      return;
    }

    if (
      courseCapacity === null ||
      !Number.isInteger(courseCapacity) ||
      courseCapacity <= 0
    ) {
      this.addCourseError.set('Capacity must be a positive whole number.');
      return;
    }

    const request: SaveCourseRequest = {
      courseName: name,
      duration: courseDuration,
      fees: courseFees,
      capacity: courseCapacity,
      isActive: this.isActive(),
    };

    this.isSubmitting.set(true);

    this.courseService.createCourse(request).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.showAddModal.set(false);
        this.addCourseSuccess.set('Course created successfully.');

        this.pageNumber.set(1);
        this.loadCourses();

        this.resetForm();
      },
      error: (error) => {
        console.error('Create course error:', error);

        const body = error.error;

        this.addCourseError.set(
          body?.detail ||
            body?.message ||
            body?.title ||
            'Unable to create course. Please try again.',
        );

        this.isSubmitting.set(false);
      },
    });
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((page) => page + 1);
      this.loadCourses();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((page) => page - 1);
      this.loadCourses();
    }
  }
}