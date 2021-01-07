import { Component, Input, OnInit } from '@angular/core';
import { MainLanguage } from 'src/app/core/enums/field-of-study/main-language.enum';
import { KindOfSubject } from 'src/app/core/enums/subject/kind-of-subject.enum';
import { ModuleType } from 'src/app/core/enums/subject/module-type.enum';
import { TypeOfSubject } from 'src/app/core/enums/subject/type-of-subject.enum';
import { Subject } from 'src/app/core/models/subject/subject';

@Component({
  selector: 'app-sub-description',
  templateUrl: './sub-description.component.html',
  styleUrls: ['./sub-description.component.scss']
})
export class SubDescriptionComponent implements OnInit {

  @Input() document: Subject = new Subject();
  modules = Object.values(ModuleType);
  kinds = Object.values(KindOfSubject);
  subTypes = Object.values(TypeOfSubject);
  languages = Object.values(MainLanguage);

  constructor() { }

  ngOnInit(): void {
  }

}
