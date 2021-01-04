import { Component, OnInit } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss']
})
export class DocumentsComponent implements OnInit {

  syllabuses: Syllabus[] = [];

  constructor() { }

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

  }
}
