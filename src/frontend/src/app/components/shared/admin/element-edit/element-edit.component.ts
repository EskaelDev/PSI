import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-element-edit',
  templateUrl: './element-edit.component.html',
  styleUrls: ['./element-edit.component.scss']
})
export class ElementEditComponent implements OnInit {

  @Input() elementName: string = '';
  @Input() canDelete: boolean = false;
  @Output() remove: EventEmitter<any> = new EventEmitter();
  @Output() save: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  onSave() {
    this.save.emit();
  }

  onRemove() {
    this.remove.emit();
  }
}
