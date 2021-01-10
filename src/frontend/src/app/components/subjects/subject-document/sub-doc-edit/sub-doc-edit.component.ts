import { Component, Input, OnInit } from '@angular/core';
import { SubjectCardEntryType } from 'src/app/core/enums/subject/subject-card-entry-type.enum';
import { CardEntries } from 'src/app/core/models/subject/card-entries';
import { Subject } from 'src/app/core/models/subject/subject';

@Component({
  selector: 'app-sub-doc-edit',
  templateUrl: './sub-doc-edit.component.html',
  styleUrls: ['./sub-doc-edit.component.scss']
})
export class SubDocEditComponent implements OnInit {
  goalEntry = SubjectCardEntryType.Goal;
  preEntry = SubjectCardEntryType.Prerequisite;
  teachEntry = SubjectCardEntryType.TeachingTools;

  @Input() readOnly: boolean = true;
  _document: Subject = new Subject();
  @Input() set document(doc: Subject) {
    this._document = doc;
  }

  constructor() { }

  ngOnInit(): void {
  }

  getCardEntry(cardType: SubjectCardEntryType): CardEntries {
    return this._document.cardEntries.find(e => e.type == cardType) ?? new CardEntries();
  }
}
