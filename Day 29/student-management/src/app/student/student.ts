
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { StudentService, StudentRecord } from '../../services/student';

@Component({
  selector: 'app-student',
  imports: [FormsModule],
  templateUrl: './student.html',
  styleUrl: './student.css'
})
export class Student {

  studentName = '';
  studentAge = 0;
  studentEmail = '';
  studentCourse = '';

  constructor(
    private router: Router,
    private studentService: StudentService
  ) {}

  addStudent(): void {

    const student: StudentRecord = {
      name: this.studentName,
      age: this.studentAge,
      email: this.studentEmail,
      course: this.studentCourse
    };

    this.studentService.addStudent(student);

    this.router.navigate(['/student-list']);
  }
}
