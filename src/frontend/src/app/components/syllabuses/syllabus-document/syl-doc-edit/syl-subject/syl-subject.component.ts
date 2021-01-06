import { Component, Input, OnInit } from '@angular/core';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-syl-subject',
  templateUrl: './syl-subject.component.html',
  styleUrls: ['./syl-subject.component.scss']
})
export class SylSubjectComponent implements OnInit {

  @Input() document: Syllabus = new Syllabus();
  
  constructor() { }

  ngOnInit(): void {
  }

}
