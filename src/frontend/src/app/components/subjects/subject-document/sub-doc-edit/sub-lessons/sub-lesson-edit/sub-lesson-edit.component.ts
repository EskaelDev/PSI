import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormOfCrediting } from 'src/app/core/enums/subject/form-of-crediting.enum';
import { LessonType } from 'src/app/core/enums/subject/lesson-type.enum';
import { ClassForm } from 'src/app/core/models/subject/class-form';
import { Lesson } from 'src/app/core/models/subject/lesson';

@Component({
  selector: 'app-sub-lesson-edit',
  templateUrl: './sub-lesson-edit.component.html',
  styleUrls: ['./sub-lesson-edit.component.scss']
})
export class SubLessonEditComponent implements OnInit {

  _elem: Lesson | null = null;
  editableElem: Lesson | null = null;
  editableClassForms: ClassForm[] = [];

  @Input() set elem(les: Lesson) {
    this._elem = les;
    this.editableElem = Object.assign(new Lesson(), les);
    this.lesForm = this.fb.group({
      lessonType: [les.lessonType, Validators.required],
      hoursAtUniversity: [les.hoursAtUniversity, Validators.required],
      studentWorkloadHours: [les.hoursAtUniversity, Validators.required],
      formOfCrediting: [les.hoursAtUniversity, Validators.required],
      ects: [les.hoursAtUniversity, Validators.required],
      ectsinclPracticalClasses: [les.hoursAtUniversity, Validators.required],
      ectsinclDirectTeacherStudentContactClasses: [les.hoursAtUniversity, Validators.required],
      isFinal: [les.hoursAtUniversity, Validators.required],
      isScientific: [les.hoursAtUniversity, Validators.required],
      isGroup: [les.hoursAtUniversity, Validators.required],
    });
    this.editableClassForms = Object.assign([], les.classForms);
  }
  @Input() isNew: boolean = true;

  lessons = Object.values(LessonType);
  creditings = Object.values(FormOfCrediting);

  lesForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output() saved: EventEmitter<Lesson> = new EventEmitter();

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    const result = Object.assign(this._elem, this.lesForm.value);
    result.classForms = Object.assign([], this.editableClassForms);
    this.saved.emit(result);
  }

  delete() {
    this.deleted.emit();
  }

  remove(entry: ClassForm) {
    this.editableClassForms = this.editableClassForms.filter(e => e !== entry);
  }

  add() {
    this.editableClassForms.push(new ClassForm());
  }

  checkClassForm(editableClassForms: ClassForm[]): boolean {
    if (editableClassForms.find(cf => !cf.description || cf.description.trim().length == 0)) {
      return false;
    }
    return true;
  }
}
