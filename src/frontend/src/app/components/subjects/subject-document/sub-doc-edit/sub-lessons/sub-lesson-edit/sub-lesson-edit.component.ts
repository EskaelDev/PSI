import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Lesson } from 'src/app/core/models/subject/lesson';

@Component({
  selector: 'app-sub-lesson-edit',
  templateUrl: './sub-lesson-edit.component.html',
  styleUrls: ['./sub-lesson-edit.component.scss']
})
export class SubLessonEditComponent implements OnInit {

  _elem: Lesson | null = null;
  editableElem: Lesson | null = null;

  @Input() set elem(les: Lesson) {
    this._elem = les;
    this.editableElem = Object.assign(new Lesson(), les);
    this.lesForm = this.fb.group({

    });
  }
  @Input() isNew: boolean = true;

  lesForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output() saved: EventEmitter<Lesson> = new EventEmitter();

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    this.saved.emit(Object.assign(this._elem, this.lesForm.value));
  }

  delete() {
    this.deleted.emit();
  }
}
