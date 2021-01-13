import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'src/app/core/models/subject/subject';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { SubjectService } from 'src/app/services/subject/subject.service';

@Component({
  selector: 'app-subject-cards',
  templateUrl: './subject-cards.component.html',
  styleUrls: ['./subject-cards.component.scss']
})
export class SubjectCardsComponent implements OnInit {

  subjects: Subject[] = [];
  syllabus: Syllabus = new Syllabus();

  constructor(public dialogRef: MatDialogRef<SubjectCardsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private subjectService: SubjectService,
    private fileHelper: FileHelper) { 
    this.subjects = data.syllabus.subjectDescriptions.map((sd: { subject: any; }) => sd.subject);
    this.syllabus = data.syllabus;
  }

  ngOnInit(): void {
  }

  downloadSubject(subject: Subject) {
    this.subjectService.pdf(subject.id).subscribe(res => {
      if (res) {
        this.fileHelper.downloadItem(res.body, `Karta_Przedmiotu_${subject.namePl}_${this.syllabus.fieldOfStudy.code}_${this.syllabus.specialization.code}_${subject.academicYear}`);
      }
    });
  }

}
