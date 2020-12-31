import { Component, Inject, OnInit, EventEmitter } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ListElement } from 'src/app/core/models/shared/list-element';

@Component({
  selector: 'app-history-popup',
  templateUrl: './history-popup.component.html',
  styleUrls: ['./history-popup.component.scss']
})
export class HistoryPopupComponent implements OnInit {

  values: ListElement[] = [];
  download = new EventEmitter();

  constructor(public dialogRef: MatDialogRef<HistoryPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { 
    this.values = data.values;
  }

  ngOnInit(): void {
  }

  downloadValue(id: string) {
    this.download.emit(id);
  }
}
