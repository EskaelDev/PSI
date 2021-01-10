import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { User } from 'src/app/core/models/user/user';

@Component({
  selector: 'app-sub-change-supervisor',
  templateUrl: './sub-change-supervisor.component.html',
  styleUrls: ['./sub-change-supervisor.component.scss']
})
export class SubChangeSupervisorComponent implements OnInit {

  teachers: User[] = [];
  currentSupervisor: User = new User();
  selectedTeacher: User | string | null = null;
  filteredTeachers: Observable<User[]> = new Observable();

  teacherControl = new FormControl();

  constructor(public dialogRef: MatDialogRef<SubChangeSupervisorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { 
    this.teachers = data.teachers;
    this.currentSupervisor = data.current;
  }

  ngOnInit(): void {
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
      teacher.name.toLowerCase().includes(filterValue) && this.currentSupervisor.id !== teacher.id
    );
  }

  displayUser(user?: User): string {
    return user ? user.name : '';
  }

  validateTeacher(teacher: User | string | null): boolean {
    if (teacher instanceof User && this.teachers.find(t => t.id === teacher.id)) {
      return true;
    }
    return false;
  }

  submit() {
    const teacher = this.teachers.find(t => t === this.selectedTeacher);
    if (teacher) {
      this.dialogRef.close(teacher);
    }
  }
}
