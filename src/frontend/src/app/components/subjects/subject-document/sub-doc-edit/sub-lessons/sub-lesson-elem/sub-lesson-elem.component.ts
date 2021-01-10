import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Lesson } from 'src/app/core/models/subject/lesson';

@Component({
  selector: 'app-sub-lesson-elem',
  templateUrl: './sub-lesson-elem.component.html',
  styleUrls: ['./sub-lesson-elem.component.scss']
})
export class SubLessonElemComponent implements OnInit {

  @Input() elem: Lesson = new Lesson();
  @Input() selected: Lesson | null = null;

  @Output() selectedElem: EventEmitter<Lesson> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedElem.emit(this.elem);
  }

}
