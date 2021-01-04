import { Component, OnInit } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-syllabus-acceptance',
  templateUrl: './syllabus-acceptance.component.html',
  styleUrls: ['./syllabus-acceptance.component.scss']
})
export class SyllabusAcceptanceComponent implements OnInit {

  syllabuses: Syllabus[] = [];
  
  constructor() { }

  ngOnInit(): void {
  }

  criteriaChanged(criteria: any) {
    criteria.fos;
    criteria.year;
  }

  download(syllabus: Syllabus) {

  }

  reject(syllabus: Syllabus) {

  }

  accept(syllabus: Syllabus) {

  }
}
