import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';

@Component({
  selector: 'app-lo-list-elem',
  templateUrl: './lo-list-elem.component.html',
  styleUrls: ['./lo-list-elem.component.scss']
})
export class LoListElemComponent implements OnInit {

  @Input() elem: LearningOutcome = new LearningOutcome();
  @Input() selected: LearningOutcome | null = null;

  @Output() selectedElem: EventEmitter<LearningOutcome> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedElem.emit(this.elem);
  }
}
