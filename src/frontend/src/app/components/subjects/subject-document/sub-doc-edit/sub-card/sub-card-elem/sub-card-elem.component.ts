import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { CardEntry } from 'src/app/core/models/subject/card-entry';

@Component({
  selector: 'app-sub-card-elem',
  templateUrl: './sub-card-elem.component.html',
  styleUrls: ['./sub-card-elem.component.scss']
})
export class SubCardElemComponent implements OnInit {

  @Input() entry: CardEntry = new CardEntry();
  idControl = new FormControl('', Validators.required);
  descControl = new FormControl('', Validators.required);
  
  constructor() { }

  ngOnInit(): void {
    this.idControl.markAsTouched();
    this.descControl.markAsTouched();
  }

}
