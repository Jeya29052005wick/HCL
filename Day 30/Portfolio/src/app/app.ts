
import { Component } from '@angular/core';

import { Navbar } from './shared/navbar/navbar';
import { Hero } from './shared/hero/hero';
import { About } from './shared/about/about';
import { Skills } from './shared/skills/skills';
import { Projects } from './shared/projects/projects';
import { Contact } from './shared/contact/contact';

@Component({
  selector: 'app-root',
  imports: [
    Navbar,
    Hero,
    About,
    Skills,
    Projects,
    Contact
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {

  aboutSummary: string =
    'Aspiring Software Engineer with practical experience in developing full-stack applications and AI-driven solutions. Experienced in transforming ideas into functional, user-focused products.';

}
