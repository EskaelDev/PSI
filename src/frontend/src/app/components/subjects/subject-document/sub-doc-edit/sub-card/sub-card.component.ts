import {AfterViewChecked, Component, ElementRef, Input, OnInit, ViewChild} from '@angular/core';
import { CardEntries } from 'src/app/core/models/subject/card-entries';
import { CardEntry } from 'src/app/core/models/subject/card-entry';

@Component({
  selector: 'app-sub-card',
  templateUrl: './sub-card.component.html',
  styleUrls: ['./sub-card.component.scss']
})
export class SubCardComponent implements OnInit {

  // @ViewChild('scrollMe') private myScrollContainer: ElementRef;
  @Input() readOnly: boolean = true;
  @Input() card: CardEntries = new CardEntries();

  constructor() { }

  ngOnInit(): void {
    // this.scrollToBottom();
  }

  remove(entry: CardEntry) {
    this.card.entries = this.card.entries.filter(e => e !== entry);
  }

  add() {
    this.card.entries.push(new CardEntry());
    // this.scrollToBottom();
  }
  // scrollToBottom(){
  //   try {
  //     this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
  //   } catch (err) { }
  // }
}


