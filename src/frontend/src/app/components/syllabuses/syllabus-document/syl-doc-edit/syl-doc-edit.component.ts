import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ListElement } from 'src/app/core/models/shared/list-element';
import { Subject } from 'src/app/core/models/subject/subject';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-syl-doc-edit',
  templateUrl: './syl-doc-edit.component.html',
  styleUrls: ['./syl-doc-edit.component.scss'],
})
export class SylDocEditComponent implements OnInit {
  @Input() disabled: boolean = false;
  _document: Syllabus = new Syllabus();
  @Input() set document(doc: Syllabus) {
    this._document = doc;
    this.semesters = this.createSemestersList(doc.description?.numOfSemesters);
  }

  subjects: Subject[] = [];
  semesters: ListElement[] = [];

  examControl = new FormControl("", Validators.required);
  internControl = new FormControl("", Validators.required);
  thesisControl = new FormControl("", Validators.required);

  updateSemesters() {
    this.semesters = this.createSemestersList(this._document.description?.numOfSemesters);
  }

  createSemestersList(semNum?: number): ListElement[] {
    const semestersList = [new ListElement(0, '-')];
    if (semNum) {
      for (let i = 1; i <= semNum; i++) {
        semestersList.push(new ListElement(i, i.toString()));
      }
    }
    return semestersList;
  }

  constructor() {}

  ngOnInit(): void {
    this.examControl.markAsTouched();
    this.internControl.markAsTouched();
    this.thesisControl.markAsTouched();
  }
}
