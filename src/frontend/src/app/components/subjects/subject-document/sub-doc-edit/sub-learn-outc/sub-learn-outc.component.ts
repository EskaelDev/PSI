import { Component, Input, OnInit } from '@angular/core';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';
import { LearningOutcomeEvaluation } from 'src/app/core/models/subject/learning-outcome-evaluation';
import { Subject } from 'src/app/core/models/subject/subject';
import { LearningOutcomeService } from 'src/app/services/learning-outcome/learning-outcome.service';

@Component({
  selector: 'app-sub-learn-outc',
  templateUrl: './sub-learn-outc.component.html',
  styleUrls: ['./sub-learn-outc.component.scss']
})
export class SubLearnOutcComponent implements OnInit {

  _document: Subject = new Subject();
  @Input() set document(doc: Subject) {
    this._document = doc;
    this.loadLearningOutcomes();
  }

  selected: LearningOutcomeEvaluation | null = null;
  
  learningOutcomes: LearningOutcome[] = [];
  
  constructor(private readonly learningOutcomeService: LearningOutcomeService) { }

  ngOnInit(): void {
  }

  loadLearningOutcomes() {
    this.learningOutcomes = [];
    this.learningOutcomeService.getLatest(this._document.fieldOfStudy.code, this._document.academicYear).subscribe(outcomes => {
      this.learningOutcomes = outcomes?.learningOutcomes ?? [];
    })
  }

  select(learn: LearningOutcomeEvaluation) {
    this.selected = learn;
  }

  add() {
    this.selected = new LearningOutcomeEvaluation();
  }

  delete() {
    this._document.learningOutcomeEvaluations = this._document.learningOutcomeEvaluations.filter(l => l !== this.selected);
    this.selected = null;
  }

  save(learn: LearningOutcomeEvaluation) {
    this.delete();
    this._document.learningOutcomeEvaluations.push(learn);
  }

}
