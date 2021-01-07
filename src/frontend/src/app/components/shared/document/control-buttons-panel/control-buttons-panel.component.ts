import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-control-buttons-panel',
  templateUrl: './control-buttons-panel.component.html',
  styleUrls: ['./control-buttons-panel.component.scss']
})
export class ControlButtonsPanelComponent implements OnInit {

  @Input() isNew: boolean = true;
  @Input() canSave: boolean = true;
  @Input() blockSaveAs: boolean = false;
  @Output() save: EventEmitter<any> = new EventEmitter();
  @Output() saveAs: EventEmitter<any> = new EventEmitter();
  @Output() import: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();
  @Output() delete: EventEmitter<any> = new EventEmitter();
  @Output() pdf: EventEmitter<any> = new EventEmitter();
  @Output() history: EventEmitter<any> = new EventEmitter();
  
  constructor() { }

  ngOnInit() {
    
  }
}
