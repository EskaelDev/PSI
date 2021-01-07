import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Literature } from 'src/app/core/models/subject/literature';

@Component({
  selector: 'app-sub-lit-edit',
  templateUrl: './sub-lit-edit.component.html',
  styleUrls: ['./sub-lit-edit.component.scss']
})
export class SubLitEditComponent implements OnInit {

  _elem: Literature | null = null;
  editableElem: Literature | null = null;

  @Input() set elem(lit: Literature) {
    this._elem = lit;
    this.editableElem = Object.assign(new Literature(), lit);
    this.litForm = this.fb.group({
      authors: [lit.authors, Validators.required],
      title: [lit.title, Validators.required],
      distributor: [lit.distributor],
      year: [lit.year],
      isPrimary: [lit.isPrimary],
      isbn: [lit.isbn, Validators.required],
    });
  }
  @Input() isNew: boolean = true;

  litForm: FormGroup = this.fb.group({});

  @Output() deleted: EventEmitter<any> = new EventEmitter();
  @Output() saved: EventEmitter<Literature> = new EventEmitter();

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {}

  save() {
    this.saved.emit(Object.assign(this._elem, this.litForm.value));
  }

  delete() {
    this.deleted.emit();
  }
}
