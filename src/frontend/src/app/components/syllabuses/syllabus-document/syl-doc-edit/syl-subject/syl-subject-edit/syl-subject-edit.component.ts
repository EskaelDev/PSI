import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ListElement } from 'src/app/core/models/shared/list-element';
import { Subject } from 'src/app/core/models/subject/subject';
import { SubjectInSyllabusDescription } from 'src/app/core/models/syllabus/subject-in-syllabus-description';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-syl-subject-edit',
  templateUrl: './syl-subject-edit.component.html',
  styleUrls: ['./syl-subject-edit.component.scss'],
})
export class SylSubjectEditComponent implements OnInit {
  _elem: SubjectInSyllabusDescription | null = null;
  editableElem: SubjectInSyllabusDescription | null = null;

  @Input() set elem(sub: SubjectInSyllabusDescription) {
    this._elem = sub;
    this.subForm = this.fb.group({
      subject: [
        sub.subject,
        [this.autocompleteObjectValidator(), Validators.required],
      ],
      assignedSemester: [sub.assignedSemester, Validators.required],
      completionSemester: [sub.completionSemester],
    });
  }
  @Input() isNew: boolean = true;

  @Input() subjects: Subject[] = [];
  @Input() semesters: ListElement[] = [];

  subForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output()
  saved: EventEmitter<SubjectInSyllabusDescription> = new EventEmitter();

  constructor(private readonly fb: FormBuilder, private alerts: AlertService) {}

  ngOnInit(): void {
    this.filteredSubjects = this.subForm.get('subject')!.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  save() {
    if (this.validateCompletionSemester()) {
      const result = Object.assign(
        new SubjectInSyllabusDescription(),
        this.subForm.value
      );
      this.saved.emit(result);
    }
    else {
      this.alerts.showCustomWarningMessage('Semestr ukończenia jest mniejszy niż przypisany semestr');
      this.alerts.showCustomErrorMessage('Zapis nie powiódł się!');
    }
  }

  delete() {
    this.deleted.emit();
  }

  selectedSubject: string | null = null;
  filteredSubjects: Observable<Subject[]> = new Observable();

  private _filter(value: any): Subject[] {
    const filterValue =
      value instanceof Subject
        ? value.namePl.toLowerCase() + value.code.toLowerCase()
        : value?.toLowerCase();

    return this.subjects.filter((subject) =>
      (subject.namePl.toLowerCase() + subject.code.toLowerCase()).includes(
        filterValue
      )
    );
  }

  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (control.value instanceof Subject) {
        return null;
      }
      return { invalidAutocompleteObject: { value: control?.value } };
    };
  }

  validateCompletionSemester(): boolean {
    if (
      this.subForm.get('completionSemester')?.value &&
      this.subForm.get('completionSemester')?.value <
      this.subForm.get('assignedSemester')?.value
    ) {
      return false;
    }
    return true;
  }

  displaySubject(sub?: Subject): string {
    return sub ? sub.code : '';
  }
}
