import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Subject } from 'src/app/core/models/subject/subject';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-year-subject-picker',
  templateUrl: './year-subject-picker.component.html',
  styleUrls: ['./year-subject-picker.component.scss'],
})
export class YearSubjectPickerComponent implements OnInit {
  title: string = '';
  allowsNew: boolean = false;
  selectedSubject: string | null = null;
  selectedYear: string | null = null;

  subjects: Subject[] = [];
  years: string[] = [
    '2015/2016',
    '2016/2017',
    '2017/2018',
    '2018/2019',
    '2019/2020',
    '2020/2021',
    '2021/2022',
  ];
  filteredSubjects: Observable<Subject[]> = new Observable();

  subjectControl = new FormControl();

  constructor(
    public dialogRef: MatDialogRef<YearSubjectPickerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly alerts: AlertService
  ) {
    dialogRef.disableClose = true;
    this.title = data.title;
    this.allowsNew = data.allowsNew ?? false;
  }

  ngOnInit(): void {
    this.loadSubjects();
    this.filteredSubjects = this.subjectControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  submit() {
    if (
      !this.allowsNew ||
      (this.allowsNew &&
        this.subjects.find(
          (s) => s.code.toLowerCase() == this.selectedSubject?.toLowerCase()
        ))
    ) {
      this.dialogRef.close({
        year: this.selectedYear,
        subject: this.selectedSubject,
      });
    } else {
      this.alerts
        .showYesNoDialog(
          'Nowy przedmiot',
          `Przedmiot o wpisanym kodzie ${this.selectedSubject} nie istnieje. Czy chcesz utworzyć nowy?`
        )
        .then((result) => {
          if (result) {
            this.dialogRef.close({
              year: this.selectedYear,
              subject: this.selectedSubject,
            });
          }
        });
    }
  }

  private _filter(value: string): Subject[] {
    this.selectedSubject = value;
    return this.subjects.filter((subject) =>
      (subject.namePl.toLowerCase() + subject.code.toLowerCase()).includes(
        value.toLowerCase()
      )
    );
  }

  validateSubject(subjectCode: string | null): boolean {
    if (this.allowsNew) {
      if (subjectCode) {
        return true;
      }
    } else {
      if (
        subjectCode &&
        this.subjects.find(
          (s) => s.code.toLowerCase() == subjectCode.toLowerCase()
        )
      )
        return true;
    }
    return false;
  }

  loadSubjects() {
  }
}
