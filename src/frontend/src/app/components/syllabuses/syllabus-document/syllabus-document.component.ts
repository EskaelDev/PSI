import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-syllabus-document',
  templateUrl: './syllabus-document.component.html',
  styleUrls: ['./syllabus-document.component.scss']
})
export class SyllabusDocumentComponent implements OnInit {
  title = 'programu studiów';

  constructor() { }

  ngOnInit(): void {
  }

}
