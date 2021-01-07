import { Component, Input, OnInit } from '@angular/core';
import { Lesson } from 'src/app/core/models/subject/lesson';
import { Subject } from 'src/app/core/models/subject/subject';

@Component({
  selector: 'app-sub-lessons',
  templateUrl: './sub-lessons.component.html',
  styleUrls: ['./sub-lessons.component.scss']
})
export class SubLessonsComponent implements OnInit {

  @Input() document: Subject = new Subject();
  selected: Lesson | null = null;
  
  constructor() { }

  ngOnInit(): void {
  }

  select(les: Lesson) {
    this.selected = les;
  }

  add() {
    this.selected = new Lesson();
  }

  delete() {
    this.document.lessons = this.document.lessons.filter(l => l !== this.selected);
    this.selected = null;
  }

  save(les: Lesson) {
    this.delete();
    this.document.lessons.push(les);
  }

}
