import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-syllabus-document',
  templateUrl: './syllabus-document.component.html',
  styleUrls: ['./syllabus-document.component.scss']
})
export class SyllabusDocumentComponent implements OnInit {
  title = 'programu studiów';

  constructor(private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.snapshot.paramMap.get('fosId');
    decodeURIComponent(this.route.snapshot.paramMap.get('year') ?? '');
  }

}
