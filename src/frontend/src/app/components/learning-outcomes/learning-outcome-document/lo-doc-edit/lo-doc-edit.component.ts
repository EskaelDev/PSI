import { Component, Input, OnInit } from '@angular/core';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { LearningOutcome } from 'src/app/core/models/learning-outcome/learning-outcome';
import { LearningOutcomeDocument } from 'src/app/core/models/learning-outcome/learning-outcome-document';

@Component({
  selector: 'app-lo-doc-edit',
  templateUrl: './lo-doc-edit.component.html',
  styleUrls: ['./lo-doc-edit.component.scss']
})
export class LoDocEditComponent implements OnInit {

  _document: LearningOutcomeDocument = new LearningOutcomeDocument();
  @Input() set document(doc: LearningOutcomeDocument) {
    this._document = doc;
    this._document.learningOutcomes = doc.learningOutcomes.sort((lo1, lo2) => lo1.symbol < lo2.symbol ? 1 : 0);
    this.specs = doc.fieldOfStudy.specializations;
  }

  specs: Specialization[] = [];
  
  selectedLo: LearningOutcome | null = null;
  selectedSpec: Specialization | null = null;
 
  constructor() { }

  ngOnInit(): void {
  }

  filterFieldLos(los: LearningOutcome[]): LearningOutcome[] {
    return los.filter(lo => !lo.specialization);
  }

  filterSpecLos(los: LearningOutcome[], spec: Specialization | null): LearningOutcome[] {
    return spec ? los.filter(lo => lo.specialization?.code === spec.code) : [];
  }

  newLo(spec: Specialization | null) {
    this.selectedLo = new LearningOutcome();
    if (spec) {
      this.selectedLo.specialization = spec;
    }
  }

  selectLo(lo: LearningOutcome) {
    this.selectedLo = lo;
  }

  deleteLo() {
    this._document.learningOutcomes = this._document.learningOutcomes.filter(lo => lo !== this.selectedLo);
    this.selectedLo = null;
  }

  saveLo(lo: LearningOutcome) {
    this.deleteLo();
    this._document.learningOutcomes.push(lo);
    this._document.learningOutcomes = this._document.learningOutcomes.sort((lo1, lo2) => lo1.symbol < lo2.symbol ? 1 : 0);
    this.selectedLo = null;
  }

  isNew(lo: LearningOutcome): boolean {
    if (this._document.learningOutcomes.find(l => l === lo)) {
      return false;
    }
    return true;
  }
}
