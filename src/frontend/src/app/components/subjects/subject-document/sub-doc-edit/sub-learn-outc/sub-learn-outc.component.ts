import { Component, Input, OnInit } from '@angular/core';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';
import { LearningOutcomeEvaluation } from 'src/app/core/models/subject/learning-outcome-evaluation';
import { Subject } from 'src/app/core/models/subject/subject';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { LearningOutcomeService } from 'src/app/services/learning-outcome/learning-outcome.service';

@Component({
  selector: 'app-sub-learn-outc',
  templateUrl: './sub-learn-outc.component.html',
  styleUrls: ['./sub-learn-outc.component.scss']
})
export class SubLearnOutcComponent implements OnInit {

  @Input() readOnly: boolean = true;
  _document: Subject = new Subject();
  @Input() set document(doc: Subject) {
    this._document = doc;
    this.loadLearningOutcomes();
  }

  selected: LearningOutcomeEvaluation | null = null;
  
  learningOutcomes: LearningOutcome[] = [];
  
  constructor(private readonly learningOutcomeService: LearningOutcomeService,
    private alerts: AlertService) { }

  ngOnInit(): void {
  }

  loadLearningOutcomes() {
    this.learningOutcomes = [];
    this.learningOutcomeService.getLatestReadOnly(this._document.fieldOfStudy.code, this._document.academicYear).subscribe(outcomes => {
      this.learningOutcomes = outcomes?.learningOutcomes ?? [];
      this.learningOutcomes = this.learningOutcomes.filter(lo => !lo.specialization || lo.specialization.code === this._document.specialization.code);
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
    if (this.checkLoIsUnique(learn)) {
      this.delete();
    this._document.learningOutcomeEvaluations.push(learn);
    }
    else {
      this.alerts.showValidationFailMessage('Wybrany kod efektu uczenia się jest już opisany!');
    }
  }

  checkLoIsUnique(lo: LearningOutcomeEvaluation): boolean {
    if(this._document.learningOutcomeEvaluations.find(l => l.learningOutcomeSymbol === lo.learningOutcomeSymbol) && this.selected?.learningOutcomeSymbol !== lo.learningOutcomeSymbol) {
      return false;
    }
    return true;
  }
}
