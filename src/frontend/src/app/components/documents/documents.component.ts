import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { SubjectCardsComponent } from './subject-cards/subject-cards.component';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss']
})
export class DocumentsComponent implements OnInit {

  syllabuses: Syllabus[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  criteriaChanged(criteria: any) {
    criteria.fos;
    criteria.year;
    criteria.spec;
  }

  downloadSyllabus(syllabus: Syllabus) {

  }

  downloadPlan(syllabus: Syllabus) {

  }

  downloadLearningOutcomes(syllabus: Syllabus) {

  }

  downloadSubjects(syllabus: Syllabus) {
    const sub = this.dialog.open(SubjectCardsComponent, {
      height: '500px',
      width: '400px',
      data: {
        subjects: syllabus.subjectDescriptions.map(sd => sd.subject),
      },
    });
  }
}
