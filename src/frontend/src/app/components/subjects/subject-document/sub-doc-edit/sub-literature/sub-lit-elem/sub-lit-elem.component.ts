import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Literature } from 'src/app/core/models/subject/literature';

@Component({
  selector: 'app-sub-lit-elem',
  templateUrl: './sub-lit-elem.component.html',
  styleUrls: ['./sub-lit-elem.component.scss']
})
export class SubLitElemComponent implements OnInit {

  @Input() elem: Literature = new Literature();
  @Input() selected: Literature | null = null;

  @Output() selectedElem: EventEmitter<Literature> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  select() {
    this.selectedElem.emit(this.elem);
  }

}
