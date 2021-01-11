import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ClassForm } from 'src/app/core/models/subject/class-form';

@Component({
  selector: 'app-sub-lessons-program',
  templateUrl: './sub-lessons-program.component.html',
  styleUrls: ['./sub-lessons-program.component.scss']
})
export class SubLessonsProgramComponent implements OnInit {

  @Input() entry: ClassForm = new ClassForm();
  hoursControl = new FormControl(0, [Validators.max(200), Validators.min(1)]);
  descControl = new FormControl('', Validators.required);
  
  constructor() { }

  ngOnInit(): void {
    this.hoursControl.markAsTouched();
    this.descControl.markAsTouched();
  }

}
