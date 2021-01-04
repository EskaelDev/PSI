import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'src/app/core/models/subject/subject';
import { AddSubjectComponent } from './add-subject/add-subject.component';

@Component({
  selector: 'app-subject-picker',
  templateUrl: './subject-picker.component.html',
  styleUrls: ['./subject-picker.component.scss']
})
export class SubjectPickerComponent implements OnInit {

  searchPhrase = '';
  subjects: Subject[] = [];
  filteredSubjects: Subject[] = [];
  years: string[] = ['2015/2016', '2016/2017', '2017/2018', '2018/2019', '2019/2020', '2020/2021', '2021/2022'];
  selectedYear: string | null = null;
  onlyMy: boolean = false;

  constructor(private readonly router: Router,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadSubjects();
  }

  filterSubjects() {
    if (this.searchPhrase?.length > 0) {
      this.filteredSubjects = Object.assign(
        [],
        this.subjects.filter((element) =>
          (element.namePl.toLowerCase() + element.code.toLowerCase()).includes(this.searchPhrase.toLowerCase())
        )
      );
    } else {
      this.filteredSubjects = Object.assign([], this.subjects);
    }
  }

  loadSubjects() {

  }

  selectedYearChanged() {
    this.loadSubjects();
  }

  mySubjectsChanged() {
    this.loadSubjects();
  }

  edit(subject: Subject) {
    this.router.navigate([`/subject/document/${subject.code}/${encodeURIComponent(subject.academicYear)}`]);
  }

  newSubject() {
    const sub = this.dialog.open(AddSubjectComponent, {
      height: '500px',
      width: '500px'
    });

    sub.afterClosed().subscribe(() => {
      this.loadSubjects();
    });
  }
}
