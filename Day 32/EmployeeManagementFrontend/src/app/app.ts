
import { Component } from '@angular/core';
import { DepartmentList } from './components/department-list/department-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DepartmentList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}
