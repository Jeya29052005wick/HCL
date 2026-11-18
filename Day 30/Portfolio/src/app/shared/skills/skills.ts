import { Component } from '@angular/core';

@Component({
  selector: 'app-skills',
  imports: [],
  templateUrl: './skills.html',
  styleUrl: './skills.css',
})
export class Skills {

  skillCategories = [
    {
      title: 'Programming Languages',
      skills: ['Java', 'Python', 'JavaScript', 'Dart', 'SQL']
    },
    {
      title: 'Frontend Development',
      skills: ['HTML', 'CSS', 'Tailwind CSS', 'React.js', 'Flutter']
    },
    {
      title: 'Backend & Databases',
      skills: [
        'Node.js',
        'Express.js',
        'REST APIs',
        'MongoDB',
        'MySQL',
        'SQLite',
        'Firebase'
      ]
    },
    {
      title: 'AI & Machine Learning',
      skills: [
        'Machine Learning',
        'RAG',
        'LangChain',
        'FAISS',
        'Hugging Face'
      ]
    },
    {
      title: 'Tools',
      skills: [
        'Git',
        'GitHub',
        'Android Studio',
        'VS Code',
        'Postman',
        'Figma'
      ]
    }
  ];

}