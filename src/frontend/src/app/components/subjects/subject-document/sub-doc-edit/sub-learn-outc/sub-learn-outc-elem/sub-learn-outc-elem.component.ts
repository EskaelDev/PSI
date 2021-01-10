import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LearningOutcomeEvaluation } from 'src/app/core/models/subject/learning-outcome-evaluation';

@Component({
  selector: 'app-sub-learn-outc-elem',
  templateUrl: './sub-learn-outc-elem.component.html',
  styleUrls: ['./sub-learn-outc-elem.component.scss']
})
export class SubLearnOutcElemComponent implements OnInit {

  @Input() elem: LearningOutcomeEvaluation = new LearningOutcomeEvaluation();
  @Input() selected: LearningOutcomeEvaluation | null = null;

  @Output() selectedElem: EventEmitter<LearningOutcomeEvaluation> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedElem.emit(this.elem);
  }
}
