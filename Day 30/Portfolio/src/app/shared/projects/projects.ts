
import { Component } from '@angular/core';
import { Project } from '../../models/project.model';
import { ProjectCard } from '../project-card/project-card';

@Component({
  selector: 'app-projects',
  imports: [ProjectCard],
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class Projects {

  projects: Project[] = [
    {
      id: 1,
      title: 'AI-Integrated E-Learning Platform',
      description:
        'Full-stack AI-powered mobile learning platform with secure authentication, course management and payment integration.',
      technologies: [
        'Flutter',
        'Firebase',
        'Node.js',
        'Express.js',
        'MongoDB',
        'Razorpay'
      ],
      image: 'images/elearning.png',
      githubUrl: ''
    },
    {
      id: 2,
      title: 'Medical RAG System',
      description:
        'AI-powered Retrieval-Augmented Generation system for medical question answering using semantic search and document retrieval.',
      technologies: [
        'Python',
        'LangChain',
        'FAISS',
        'Hugging Face',
        'Streamlit'
      ],
      image: 'images/medical-rag.png',
      githubUrl: ''
    }
  ];

  selectedProject: Project | null = null;

  showProjectDetails(project: Project): void {
    this.selectedProject = project;
  }

  closeProjectDetails(): void {
    this.selectedProject = null;
  }

}
