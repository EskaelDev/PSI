import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SubjectInSyllabusDescription } from 'src/app/core/models/syllabus/subject-in-syllabus-description';

@Component({
  selector: 'app-syl-subject-list-elem',
  templateUrl: './syl-subject-list-elem.component.html',
  styleUrls: ['./syl-subject-list-elem.component.scss']
})
export class SylSubjectListElemComponent implements OnInit {

  @Input() elem: SubjectInSyllabusDescription = new SubjectInSyllabusDescription();
  @Input() selected: SubjectInSyllabusDescription | null = null;

  @Output() selectedElem: EventEmitter<SubjectInSyllabusDescription> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedElem.emit(this.elem);
  }

}
