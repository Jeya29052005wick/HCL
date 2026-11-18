
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-about',
  imports: [],
  templateUrl: './about.html',
  styleUrl: './about.css',
})
export class About {

  @Input() summary: string = '';

  education = {
    degree: 'Bachelor of Technology in Information Technology',
    college: 'PSNA College of Engineering and Technology',
    duration: '2023 - 2027',
    cgpa: '8.2'
  };

  internship = {
    role: 'Data Analytics Intern',
    company: 'Sabudh Foundation',
    duration: 'January 2026 - June 2026'
  };

}
