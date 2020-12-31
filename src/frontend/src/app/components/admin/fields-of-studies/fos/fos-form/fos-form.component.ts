import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CourseType } from 'src/app/core/enums/field-of-study/course-type.enum';
import { DegreeLevel } from 'src/app/core/enums/field-of-study/degree-level.enum';
import { MainLanguage } from 'src/app/core/enums/field-of-study/main-language.enum';
import { StudiesProfile } from 'src/app/core/enums/field-of-study/studies-profile.enum';
import { User } from 'src/app/core/models/user/user';

@Component({
  selector: 'app-fos-form',
  templateUrl: './fos-form.component.html',
  styleUrls: ['./fos-form.component.scss'],
})
export class FosFormComponent implements OnInit {
  @Input() fosForm: FormGroup = new FormGroup({});
  @Input() isNew: boolean = true;

  languages = Object.values(MainLanguage);
  levels = Object.values(DegreeLevel);
  types = Object.values(CourseType);
  profiles = Object.values(StudiesProfile);

  @Input() supervisors: User[] = [];
  filteredSupervisors: Observable<User[]> = new Observable();

  constructor() {}

  ngOnInit() {
    this.filteredSupervisors = this.fosForm
      .get('supervisor')!
      .valueChanges.pipe(
        startWith(''),
        map((value) => this._filter(value))
      );
  }

  private _filter(value: any): User[] {
    const filterValue =
      value instanceof User ? value.name.toLowerCase() : value.toLowerCase();

    return this.supervisors.filter((supervisor) =>
      supervisor.name.toLowerCase().includes(filterValue)
    );
  }

  displayUser(user?: User): string {
    return user ? user.name : '';
  }
}
