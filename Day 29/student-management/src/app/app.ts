import { Component } from '@angular/core';
import { Student } from './student/student';

@Component({
  selector: 'app-root',
  imports: [Student],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'Student Management System';
  message = 'Welcome to Angular!';
}
