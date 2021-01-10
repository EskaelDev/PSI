import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { Subject } from 'src/app/core/models/subject/subject';
import { User } from 'src/app/core/models/user/user';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';
import { SubjectService } from 'src/app/services/subject/subject.service';

@Component({
  selector: 'app-add-subject',
  templateUrl: './add-subject.component.html',
  styleUrls: ['./add-subject.component.scss']
})
export class AddSubjectComponent implements OnInit {
 
  isLoading = false;
  selectedSupervisor: string | null = null;
  selectedYear: string | null = null;

  supervisors: User[] = [];
  years: string[] = [
    '2015/2016',
    '2016/2017',
    '2017/2018',
    '2018/2019',
    '2019/2020',
    '2020/2021',
    '2021/2022',
  ];
  filteredSupervisors: Observable<User[]> = new Observable();
  subjectForm: FormGroup;

  fieldsOfStudy: FieldOfStudy[] = [];
  specs: Specialization[] = [];

  constructor(
    public dialogRef: MatDialogRef<AddSubjectComponent>,
    private readonly alerts: AlertService,
    private subjectService: SubjectService,
    private fosService: FieldOfStudyService,
    private readonly fb: FormBuilder
  ) {
    this.dialogRef.disableClose = true;
    this.subjectForm = this.fb.group({
      academicYear: ['', Validators.required],
      namePl: ['', Validators.required],
      code: ['', Validators.required],
      fieldOfStudy: ['', Validators.required],
      specialization: ['', Validators.required],
      supervisor: [
        '',
        [this.autocompleteObjectValidator(), Validators.required],
      ],
    });
  }

  ngOnInit(): void {
    this.loadSupervisors();
    this.loadFieldsOfStudies();
    this.filteredSupervisors = this.subjectForm
    .get('supervisor')!.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  loadFieldsOfStudies() {
    this.fosService.getMyFieldsOfStudies().subscribe(fields => {
      this.fieldsOfStudy = fields;
    });
  }

  submit() {
    this.isLoading = true;
    this.subjectService.save(Object.assign(new Subject(), this.subjectForm.value)).subscribe(result => {
      if (result) {
        this.alerts.showCustomSuccessMessage('Dodano przedmiot');
        this.dialogRef.close(true);
      }
      this.isLoading = false;
    },
    () => {
      this.isLoading = false;
    });
  }

  private _filter(value: any): User[] {
    const filterValue =
      value instanceof User ? value.name.toLowerCase() : value?.toLowerCase();

    return this.supervisors.filter((supervisor) =>
      supervisor.name.toLowerCase().includes(filterValue)
    );
  }

  displayUser(user?: User): string {
    return user ? user.name : '';
  }

  loadSupervisors() {
    this.subjectService.getPossibleTeachers().subscribe((supervisors) => {
      this.supervisors = supervisors;
    });
  }

  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (control.value instanceof User) {
        return null;
      }
      return { invalidAutocompleteObject: { value: control.value } };
    };
  }

  selectedFosChanged() {
    this.subjectForm.patchValue({
      specialization: null
    });
    
    if (this.subjectForm.get('fieldOfStudy')) {
      this.specs = this.subjectForm.get('fieldOfStudy')?.value.specializations;
    }
    else {
      this.specs = [];
    }
  }
}
