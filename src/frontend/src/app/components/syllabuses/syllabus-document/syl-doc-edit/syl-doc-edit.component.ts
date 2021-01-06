import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'src/app/core/models/subject/subject';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-syl-doc-edit',
  templateUrl: './syl-doc-edit.component.html',
  styleUrls: ['./syl-doc-edit.component.scss']
})
export class SylDocEditComponent implements OnInit {

  @Input() disabled: boolean = false;
  _document: Syllabus = new Syllabus();
  @Input() set document(doc: Syllabus) {
    this._document = doc;
  }

  subjects: Subject[] = [];

  constructor() { }

  ngOnInit(): void {
  }
}
