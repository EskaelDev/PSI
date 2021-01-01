import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-year-subject-picker',
  templateUrl: './year-subject-picker.component.html',
  styleUrls: ['./year-subject-picker.component.scss']
})
export class YearSubjectPickerComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<YearSubjectPickerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { 
      dialogRef.disableClose = true;
    }

  ngOnInit(): void {
  }

}
