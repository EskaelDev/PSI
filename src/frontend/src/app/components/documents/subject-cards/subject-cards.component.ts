import { Component, EventEmitter, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'src/app/core/models/subject/subject';
import { SubjectService } from 'src/app/services/subject/subject.service';

@Component({
  selector: 'app-subject-cards',
  templateUrl: './subject-cards.component.html',
  styleUrls: ['./subject-cards.component.scss']
})
export class SubjectCardsComponent implements OnInit {

  subjects: Subject[] = [];

  constructor(public dialogRef: MatDialogRef<SubjectCardsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private subjectService: SubjectService) { 
    this.subjects = data.subjects;
  }

  ngOnInit(): void {
  }

  downloadSubject(subject: Subject) {
    this.subjectService.pdf(subject.id).subscribe(() => {

    });
  }

}
