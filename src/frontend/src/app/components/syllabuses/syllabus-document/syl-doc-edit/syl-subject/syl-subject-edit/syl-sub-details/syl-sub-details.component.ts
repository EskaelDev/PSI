import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'src/app/core/models/subject/subject';

@Component({
  selector: 'app-syl-sub-details',
  templateUrl: './syl-sub-details.component.html',
  styleUrls: ['./syl-sub-details.component.scss']
})
export class SylSubDetailsComponent implements OnInit {

  @Input() subject?: Subject;

  constructor() { }

  ngOnInit(): void {
  }

}
