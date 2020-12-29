import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';

@Component({
  selector: 'app-fos-specializations',
  templateUrl: './fos-specializations.component.html',
  styleUrls: ['./fos-specializations.component.scss']
})
export class FosSpecializationsComponent implements OnInit {

  editableSpecs: Specialization[] = [];
  selectedSpec: Specialization = new Specialization();
  isNew = true;

  @Input() set fosSpecs(specs: Specialization[]) {
    this.editableSpecs = Object.assign([], specs);
  }
  @Output() specsSaved: EventEmitter<Specialization[]> = new EventEmitter();

  specializationForm: FormGroup;

  constructor(
    private readonly fb: FormBuilder
  ) {
    this.specializationForm = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
    });
  }

  ngOnInit(): void {
  }

  editSpec(spec: Specialization) {
    this.selectedSpec = spec;
    this.isNew = spec.code.length === 0;
    this.specializationForm.patchValue({
      code: spec.code,
      name: spec.name
    });
  }

  addSpec() {
    this.editSpec(new Specialization());
  }

  removeSpec(spec: Specialization) {
    this.editableSpecs = this.editableSpecs.filter(s => s.code !== spec.code);
    if (this.selectedSpec.code == spec.code) {
      this.selectedSpec = new Specialization();
    }
  }

  saveCurrent() {
    this.selectedSpec.name = this.specializationForm.get('name')?.value;
    if (!this.editableSpecs.find(s => s.code === this.selectedSpec.code)) {
      this.selectedSpec.code = this.specializationForm.get('code')?.value;
      this.editableSpecs.push(this.selectedSpec);
    }
    this.addSpec();
  }

  saveSpecs() {
    this.specsSaved.emit(Object.assign([], this.editableSpecs));
  }
}
