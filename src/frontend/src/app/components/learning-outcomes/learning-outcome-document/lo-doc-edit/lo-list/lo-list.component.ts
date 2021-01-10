import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';

@Component({
  selector: 'app-lo-list',
  templateUrl: './lo-list.component.html',
  styleUrls: ['./lo-list.component.scss']
})
export class LoListComponent implements OnInit {

  @Input() elems: LearningOutcome[] = [];
  @Input() selected: LearningOutcome | null = null;
  @Output() selectedElem: EventEmitter<LearningOutcome> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select(lo: LearningOutcome) {
    this.selectedElem.emit(lo);
  }
}
