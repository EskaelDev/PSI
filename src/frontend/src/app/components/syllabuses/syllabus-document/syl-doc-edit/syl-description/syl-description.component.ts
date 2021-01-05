import { Component, Input, OnInit } from '@angular/core';
import { GraduationForm } from 'src/app/core/enums/syllabus/graduation-form.enum';
import { ProfessionalTitle } from 'src/app/core/enums/syllabus/professional-title.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-syl-description',
  templateUrl: './syl-description.component.html',
  styleUrls: ['./syl-description.component.scss']
})
export class SylDescriptionComponent implements OnInit {

  @Input() document: Syllabus = new Syllabus();
  titles = Object.values(ProfessionalTitle);
  graduations = Object.values(GraduationForm);

  constructor() { }

  ngOnInit(): void {
  }

}
