import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';

@Component({
  selector: 'app-fos-specializations',
  templateUrl: './fos-specializations.component.html',
  styleUrls: ['./fos-specializations.component.scss']
})
export class FosSpecializationsComponent implements OnInit {

  editableSpecs: Specialization[] = [];
  @Input() set fosSpecs(specs: Specialization[]) {
    this.editableSpecs = Object.assign([], specs);
  }
  @Output() specsSaved: EventEmitter<Specialization[]> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

}
