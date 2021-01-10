import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { Subject } from 'src/app/core/models/subject/subject';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';
import { SubjectService } from 'src/app/services/subject/subject.service';
import { AddSubjectComponent } from './add-subject/add-subject.component';

@Component({
  selector: 'app-subject-picker',
  templateUrl: './subject-picker.component.html',
  styleUrls: ['./subject-picker.component.scss'],
})
export class SubjectPickerComponent implements OnInit {
  isLoading = false;
  searchPhrase = '';
  subjects: Subject[] = [];
  filteredSubjects: Subject[] = [];
  fieldsOfStudy: FieldOfStudy[] = [];
  specs: Specialization[] = [];
  years: string[] = [
    '2015/2016',
    '2016/2017',
    '2017/2018',
    '2018/2019',
    '2019/2020',
    '2020/2021',
    '2021/2022',
  ];

  selectedFos: FieldOfStudy | null = null;
  selectedSpec: Specialization | null = null;
  selectedYear: string | null = null;
  whereSupervisor: boolean = false;
  whereTeacher: boolean = false;

  constructor(
    private readonly router: Router,
    public dialog: MatDialog,
    private fosService: FieldOfStudyService,
    private subjectService: SubjectService
  ) {}

  ngOnInit(): void {
    this.loadFieldsOfStudy();
  }

  filterSubjects() {
    this.filteredSubjects = Object.assign(
      [],
      this.subjects.filter(
        (element) =>
          (this.searchPhrase?.length > 0
            ? (
                element.namePl.toLowerCase() + element.code.toLowerCase()
              ).includes(this.searchPhrase.toLowerCase())
            : true) &&
          (this.whereSupervisor && this.whereTeacher
            ? element.isSupervisor || element.isTeacher
            : (this.whereSupervisor ? element.isSupervisor : true) &&
              (this.whereTeacher ? element.isTeacher : true))
      )
    );
  }

  loadFieldsOfStudy() {
    this.fosService.getFieldsOfStudies().subscribe((fieldsOfStudy) => {
      this.fieldsOfStudy = fieldsOfStudy;
    });
  }

  selectedFosChanged() {
    this.selectedSpec = null;

    if (this.selectedFos) {
      this.specs = this.selectedFos.specializations;
    } else {
      this.specs = [];
    }
    this.loadSubjects();
  }

  loadSubjects() {
    this.subjects = [];
    if (this.selectedSpec && this.selectedYear) {
      this.isLoading = true;
      this.subjectService
        .getAll(
          this.selectedFos?.code ?? '',
          this.selectedSpec.code,
          this.selectedYear
        )
        .subscribe(
          (subjects) => {
            this.subjects = subjects;
            this.filterSubjects();
            this.isLoading = false;
          },
          () => {
            this.isLoading = false;
          }
        );
    }
  }

  edit(subject: Subject) {
    this.router.navigate([
      `/subject/document/${subject.code}/${this.selectedFos?.code}/${
        this.selectedSpec?.code
      }/${encodeURIComponent(subject.academicYear)}`,
    ]);
  }

  newSubject() {
    const sub = this.dialog.open(AddSubjectComponent, {
      height: '700px',
      width: '500px',
    });

    sub.afterClosed().subscribe(() => {
      this.loadSubjects();
    });
  }
}
