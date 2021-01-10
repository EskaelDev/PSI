import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MainLanguage } from 'src/app/core/enums/field-of-study/main-language.enum';
import { KindOfSubject } from 'src/app/core/enums/subject/kind-of-subject.enum';
import { ModuleType } from 'src/app/core/enums/subject/module-type.enum';
import { TypeOfSubject } from 'src/app/core/enums/subject/type-of-subject.enum';
import { Subject } from 'src/app/core/models/subject/subject';
import { User } from 'src/app/core/models/user/user';
import { SubjectService } from 'src/app/services/subject/subject.service';
import { SubChangeSupervisorComponent } from './sub-change-supervisor/sub-change-supervisor.component';

@Component({
  selector: 'app-sub-description',
  templateUrl: './sub-description.component.html',
  styleUrls: ['./sub-description.component.scss']
})
export class SubDescriptionComponent implements OnInit {

  @Input() readOnly: boolean = true;
  @Input() document: Subject = new Subject();
  modules = Object.values(ModuleType);
  kinds = Object.values(KindOfSubject);
  subTypes = Object.values(TypeOfSubject);
  languages = Object.values(MainLanguage);

  selectedTeacher: User | string | null = null;
  teachers: User[] = [];
  filteredTeachers: Observable<User[]> = new Observable();

  teacherControl = new FormControl();

  constructor(private subjectService: SubjectService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadTeachers();
    this.filteredTeachers = this.teacherControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
  }

  private _filter(value: any): User[] { 
    this.selectedTeacher = value;  
    const filterValue =
      value instanceof User ? value.name.toLowerCase() : value?.toLowerCase();

    return this.teachers.filter((teacher) =>
      teacher.name.toLowerCase().includes(filterValue) && !this.document.teachers.find(t => t.id === teacher.id)
    );
  }

  displayUser(user?: User): string {
    return user ? user.name : '';
  }

  loadTeachers() {
    this.subjectService.getPossibleTeachers().subscribe(result => {
      this.teachers = result;
    });
  }

  validateTeacher(teacher: User | string | null): boolean {
    if (teacher instanceof User && this.teachers.find(t => t.id === teacher.id)) {
      return true;
    }
    return false;
  }

  clearTeacher() {
    this.selectedTeacher = null;
    this.teacherControl.patchValue('');
  }

  addTeacher() {
    const teacher = this.teachers.find(t => t === this.selectedTeacher);
    if (teacher) {
      this.document.teachers.push(teacher);
      this.clearTeacher();
    }
  }

  removeTeacher(teacher: User) {
    this.document.teachers = this.document.teachers.filter(t => t.id !== teacher.id);
  }

  changeSupervisor() {
    const sub = this.dialog.open(SubChangeSupervisorComponent, {
      height: '300px',
      width: '500px',
      data: {
        teachers: this.teachers,
        current: this.document.supervisor
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result) {
        this.document.supervisor = result;
      }
    });
  }
}
