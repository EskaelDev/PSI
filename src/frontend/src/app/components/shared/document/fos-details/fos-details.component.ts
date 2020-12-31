import { Component, Input, OnInit } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';

@Component({
  selector: 'app-fos-details',
  templateUrl: './fos-details.component.html',
  styleUrls: ['./fos-details.component.scss']
})
export class FosDetailsComponent implements OnInit {

  @Input() fos: FieldOfStudy = new FieldOfStudy();
  @Input() year: string = '';
  @Input() specialization: Specialization | null = new Specialization();

  constructor() { }

  ngOnInit(): void {
  }

}
