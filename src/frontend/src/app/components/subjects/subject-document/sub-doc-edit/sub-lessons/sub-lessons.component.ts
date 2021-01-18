import { Component, Input, OnInit } from '@angular/core';
import { Lesson } from 'src/app/core/models/subject/lesson';
import { Subject } from 'src/app/core/models/subject/subject';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-sub-lessons',
  templateUrl: './sub-lessons.component.html',
  styleUrls: ['./sub-lessons.component.scss']
})
export class SubLessonsComponent implements OnInit {

  @Input() readOnly: boolean = true;
  @Input() document: Subject = new Subject();
  selected: Lesson | null = null;
  
  constructor(private alerts: AlertService) { }

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
    if (this.checkLessonIsUnique(les)) {
      this.delete();
      this.document.lessons.push(les);
    }
    else {
      this.alerts.showValidationFailMessage('Zajęcia tego typu już istnieją!');
    }
  }

  checkLessonIsUnique(le: Lesson): boolean {
    if(this.document.lessons.find(l => l.lessonType === le.lessonType) && this.selected?.lessonType !== le.lessonType) {
      return false;
    }
    return true;
  }
}
