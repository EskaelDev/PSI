import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { User } from 'src/app/core/models/user/user';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';

@Component({
  selector: 'app-add-subject',
  templateUrl: './add-subject.component.html',
  styleUrls: ['./add-subject.component.scss']
})
export class AddSubjectComponent implements OnInit {
 
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

  constructor(
    public dialogRef: MatDialogRef<AddSubjectComponent>,
    private readonly alerts: AlertService,
    private fosService: FieldOfStudyService,
    private readonly fb: FormBuilder
  ) {
    this.subjectForm = this.fb.group({
      academicYear: ['', Validators.required],
      namePl: ['', Validators.required],
      code: ['', Validators.required],
      supervisor: [
        '',
        [this.autocompleteObjectValidator(), Validators.required],
      ],
    });
  }

  ngOnInit(): void {
    this.loadSupervisors();
    this.filteredSupervisors = this.subjectForm
    .get('supervisor')!.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  submit() {
    
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
    // todo: change to subject service
    this.fosService.getPossibleSupervisors().subscribe((supervisors) => {
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
}
