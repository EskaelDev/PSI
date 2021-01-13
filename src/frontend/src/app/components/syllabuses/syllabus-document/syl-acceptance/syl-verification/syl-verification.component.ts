import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';

@Component({
  selector: 'app-syl-verification',
  templateUrl: './syl-verification.component.html',
  styleUrls: ['./syl-verification.component.scss']
})
export class SylVerificationComponent implements OnInit {

  syllabus: Syllabus = new Syllabus();
  isLoading = true;
  isErrorState = false;
  results: string[] | null = null;

  constructor(public dialogRef: MatDialogRef<SylVerificationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private syllabusService: SyllabusService) { 
      this.dialogRef.disableClose = true;
    this.syllabus = data;
  }

  ngOnInit(): void {
    this.verify();
  }

  verify() {
    this.syllabusService.verify(this.syllabus).subscribe(result => {
      this.isLoading = false;
      if (result) {
        this.results = result;
      }
      else {
        this.isErrorState = true;
      }
    });
  }
}
