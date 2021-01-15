import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LearningOutcomeCategory } from 'src/app/core/enums/learning-outcome/learning-outcome-category.enum';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';

@Component({
  selector: 'app-lo-edit',
  templateUrl: './lo-edit.component.html',
  styleUrls: ['./lo-edit.component.scss'],
})
export class LoEditComponent implements OnInit {
  _elem: LearningOutcome | null = null;
  editableElem: LearningOutcome | null = null;

  @Input() set elem(lo: LearningOutcome) {
    this._elem = lo;
    this.editableElem = Object.assign(new LearningOutcome(), lo);
    this.loForm = this.fb.group({
      symbol: [lo.symbol, Validators.required],
      category: [lo.category, Validators.required],
      description: [lo.description],
      u1degreeCharacteristics: [
        lo.u1degreeCharacteristics,
        Validators.required,
      ],
      s2degreePrk: [lo.s2degreePrk],
      s2degreePrkeng: [lo.s2degreePrkeng],
    });
  }
  @Input() isNew: boolean = true;

  loForm: FormGroup = this.fb.group({});

  categories = Object.values(LearningOutcomeCategory);

  @Output() deletedLo: EventEmitter<any> = new EventEmitter();
  @Output() savedLo: EventEmitter<LearningOutcome> = new EventEmitter();

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    const newLo = Object.assign(new LearningOutcome(), this.loForm.value);
    newLo.specialization = this._elem?.specialization;
    this.savedLo.emit(newLo);
  }

  delete() {
    this.deletedLo.emit();
  }
}
