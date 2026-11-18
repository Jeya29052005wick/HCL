
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-contact',
  imports: [FormsModule],
  templateUrl: './contact.html',
  styleUrl: './contact.css',
})
export class Contact implements OnInit {

  name: string = '';

  email: string = '';

  message: string = '';

  submitted: boolean = false;

  currentYear: number = 0;

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();

    console.log('Contact component initialized');
  }

  onSubmit(): void {

    this.submitted = true;

    console.log('Name:', this.name);
    console.log('Email:', this.email);
    console.log('Message:', this.message);

  }

}
