import { Component, Input, OnInit } from '@angular/core';
import { ListElement } from 'src/app/core/models/shared/list-element';
import { Subject } from 'src/app/core/models/subject/subject';
import { SubjectInSyllabusDescription } from 'src/app/core/models/syllabus/subject-in-syllabus-description';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { SubjectService } from 'src/app/services/subject/subject.service';

@Component({
  selector: 'app-syl-subject',
  templateUrl: './syl-subject.component.html',
  styleUrls: ['./syl-subject.component.scss']
})
export class SylSubjectComponent implements OnInit {

  @Input() document: Syllabus = new Syllabus();

  selected: SubjectInSyllabusDescription | null = null;
  subjects: Subject[] = [];
  @Input() semesters: ListElement[] = [];
  
  constructor(private alerts: AlertService,
    private subjectService: SubjectService) { }

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects() {
    this.subjectService.getAll(this.document.fieldOfStudy.code, this.document.specialization.code, this.document.academicYear).subscribe(subs => {
      this.subjects = subs;
    });
  }

  select(sub: SubjectInSyllabusDescription) {
    this.selected = sub;
  }

  add() {
    this.selected = new SubjectInSyllabusDescription();
  }

  delete() {
    this.document.subjectDescriptions = this.document.subjectDescriptions.filter(s => s !== this.selected);
    this.selected = null;
  }

  save(sub: SubjectInSyllabusDescription) {
    if (this.checkSubjectIsUnique(sub)) {
      this.delete();
      this.document.subjectDescriptions.push(sub);
    }
    else {
      this.alerts.showCustomErrorMessage('Wybrany przedmiot już znajduje się na liście!');
    }
  }

  checkSubjectIsUnique(sub: SubjectInSyllabusDescription): boolean {
    if(this.document.subjectDescriptions.find(s => s.subject?.code === sub.subject?.code) && this.selected?.subject?.code !== sub.subject?.code) {
      return false;
    }
    return true;
  }
}
