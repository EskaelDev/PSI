import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { GraduationForm } from 'src/app/core/enums/syllabus/graduation-form.enum';
import { ProfessionalTitle } from 'src/app/core/enums/syllabus/professional-title.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-syl-description',
  templateUrl: './syl-description.component.html',
  styleUrls: ['./syl-description.component.scss'],
})
export class SylDescriptionComponent implements OnInit {
  @Input() document: Syllabus = new Syllabus();
  titles = Object.values(ProfessionalTitle);
  graduations = Object.values(GraduationForm);
  @Output() semNum: EventEmitter<any> = new EventEmitter();

  numOfSem = new FormControl([0, [Validators.max(10), Validators.min(1)]]);
  preq = new FormControl(['', Validators.required]);
  finish = new FormControl(['', Validators.required]);
  cont = new FormControl(['', Validators.required]);

  constructor(private alerts: AlertService) {}

  ngOnInit(): void {
    this.numOfSem.markAsTouched();
    this.preq.markAsTouched();
    this.finish.markAsTouched();
    this.cont.markAsTouched();
  }

  semesterNumChanged() {
    if (
      this.document.subjectDescriptions.find(
        (s) =>
          this.document.description &&
          ((s.assignedSemester &&
            s.assignedSemester > this.document.description?.numOfSemesters) ||
            (s.completionSemester &&
              s.completionSemester > this.document.description?.numOfSemesters))
      )
    ) {
      this.alerts.showCustomWarningMessage(
        'Liczba semestrów uległa zmianie! Istnieją przedmioty które posiadają niepoprawny semestr'
      );
    }
    this.semNum.emit();
  }
}
