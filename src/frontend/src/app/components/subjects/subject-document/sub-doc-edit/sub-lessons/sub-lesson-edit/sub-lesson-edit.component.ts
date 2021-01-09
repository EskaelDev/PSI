import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormOfCrediting } from 'src/app/core/enums/subject/form-of-crediting.enum';
import { LessonType } from 'src/app/core/enums/subject/lesson-type.enum';
import { ClassForm } from 'src/app/core/models/subject/class-form';
import { Lesson } from 'src/app/core/models/subject/lesson';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-sub-lesson-edit',
  templateUrl: './sub-lesson-edit.component.html',
  styleUrls: ['./sub-lesson-edit.component.scss']
})
export class SubLessonEditComponent implements OnInit {

  @Input() readOnly: boolean = true;
  _elem: Lesson | null = null;
  editableElem: Lesson | null = null;
  editableClassForms: ClassForm[] = [];

  @Input() set elem(les: Lesson) {
    this._elem = les;
    this.editableElem = Object.assign(new Lesson(), les);
    this.lesForm = this.fb.group({
      lessonType: [les.lessonType, Validators.required],
      hoursAtUniversity: [les.hoursAtUniversity, [Validators.required, Validators.min(1), Validators.max(200)]],
      studentWorkloadHours: [les.studentWorkloadHours, [Validators.required, Validators.min(1), Validators.max(200)]],
      formOfCrediting: [les.formOfCrediting, Validators.required],
      ects: [les.ects, [Validators.required, Validators.min(1), Validators.max(30)]],
      ectsinclPracticalClasses: [les.ectsinclPracticalClasses, Validators.required],
      ectsinclDirectTeacherStudentContactClasses: [les.ectsinclDirectTeacherStudentContactClasses, Validators.required],
      isFinal: [les.isFinal, Validators.required],
      isScientific: [les.isScientific, Validators.required],
      isGroup: [les.isGroup, Validators.required],
    });
    this.editableClassForms = Object.assign([], les.classForms);
  }
  @Input() isNew: boolean = true;

  lessons = Object.values(LessonType);
  creditings = Object.values(FormOfCrediting);

  lesForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output() saved: EventEmitter<Lesson> = new EventEmitter();

  constructor(private readonly fb: FormBuilder,
    private alerts: AlertService) {}

  ngOnInit(): void {}

  save() {
    if (this.checkClassFormZZUCompliance()) {
      const result = Object.assign(new Lesson(), this.lesForm.value);
      result.classForms = Object.assign([], this.editableClassForms);
      this.saved.emit(result);
    }
    else {
      this.alerts.showCustomWarningMessage('Niezgodna liczba ZZU z sumą godzin treści programowych');
      this.alerts.showCustomErrorMessage('Zajęcia nie zapisane!');
    }
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
    if (editableClassForms.find(cf => cf.hours < 1 || cf.hours > 200 || !cf.description || cf.description.trim().length == 0)) {
      return false;
    }
    return true;
  }

  checkClassFormZZUCompliance(): boolean {
    return this.editableClassForms.reduce((acc, val) => acc += val.hours, 0) === this.lesForm.get('hoursAtUniversity')?.value;
  }
}
