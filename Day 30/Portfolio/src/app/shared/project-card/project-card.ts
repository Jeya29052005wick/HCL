import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-project-card',
  imports: [],
  templateUrl: './project-card.html',
  styleUrl: './project-card.css',
})
export class ProjectCard {

  @Input() project!: Project;

  @Output() viewProject = new EventEmitter<Project>();

}