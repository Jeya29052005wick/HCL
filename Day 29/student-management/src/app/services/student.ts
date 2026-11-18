
import { Injectable } from '@angular/core';

export interface StudentRecord {
  name: string;
  age: number;
  email: string;
  course: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private students: StudentRecord[] = [];

  addStudent(student: StudentRecord): void {
    this.students.push(student);
  }

  getStudents(): StudentRecord[] {
    return this.students;
  }

}
