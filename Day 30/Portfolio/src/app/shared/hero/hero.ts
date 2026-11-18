import { UpperCasePipe } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-hero',
  imports: [UpperCasePipe],
  templateUrl: './hero.html',
  styleUrl: './hero.css',
})
export class Hero {

  name: string = 'Jeyachandran';

  role: string = 'Aspiring Software Engineer';

  description: string =
    'Building scalable applications and AI-powered solutions.';

  introduction: string =
    'Passionate about software engineering, full-stack development and artificial intelligence.';
}