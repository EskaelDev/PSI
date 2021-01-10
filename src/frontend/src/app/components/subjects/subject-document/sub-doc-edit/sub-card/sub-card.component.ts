import { Component, Input, OnInit } from '@angular/core';
import { CardEntries } from 'src/app/core/models/subject/card-entries';
import { CardEntry } from 'src/app/core/models/subject/card-entry';

@Component({
  selector: 'app-sub-card',
  templateUrl: './sub-card.component.html',
  styleUrls: ['./sub-card.component.scss']
})
export class SubCardComponent implements OnInit {

  @Input() readOnly: boolean = true;
  @Input() card: CardEntries = new CardEntries();

  constructor() { }

  ngOnInit(): void {
  }

  remove(entry: CardEntry) {
    this.card.entries = this.card.entries.filter(e => e !== entry);
  }

  add() {
    this.card.entries.push(new CardEntry());
  }
}
