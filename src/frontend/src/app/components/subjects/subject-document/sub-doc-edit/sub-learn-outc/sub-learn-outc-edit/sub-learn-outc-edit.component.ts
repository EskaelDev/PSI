import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GradingSystem } from 'src/app/core/enums/subject/grading-system.enum';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';
import { LearningOutcomeEvaluation } from 'src/app/core/models/subject/learning-outcome-evaluation';

@Component({
  selector: 'app-sub-learn-outc-edit',
  templateUrl: './sub-learn-outc-edit.component.html',
  styleUrls: ['./sub-learn-outc-edit.component.scss']
})
export class SubLearnOutcEditComponent implements OnInit {

  @Input() readOnly: boolean = true;
  _elem: LearningOutcomeEvaluation | null = null;
  editableElem: LearningOutcomeEvaluation | null = null;

  @Input() set elem(learn: LearningOutcomeEvaluation) {
    this._elem = learn;
    this.editableElem = Object.assign(new LearningOutcomeEvaluation(), learn);
    this.learnForm = this.fb.group({
      gradingSystem: [learn.gradingSystem, Validators.required],
      learningOutcomeSymbol: [learn.learningOutcomeSymbol, Validators.required],
      description: [learn.description],
    });
  }

  gradings = Object.values(GradingSystem);
  @Input() learningOutcomes: LearningOutcome[] = [];

  learnForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output() saved: EventEmitter<LearningOutcomeEvaluation> = new EventEmitter();

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    this.saved.emit(Object.assign(new LearningOutcome(), this.learnForm.value));
  }

  delete() {
    this.deleted.emit();
  }
}
