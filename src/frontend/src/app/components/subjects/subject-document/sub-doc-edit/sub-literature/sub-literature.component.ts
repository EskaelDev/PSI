import { Component, Input, OnInit } from '@angular/core';
import { Literature } from 'src/app/core/models/subject/literature';
import { Subject } from 'src/app/core/models/subject/subject';

@Component({
  selector: 'app-sub-literature',
  templateUrl: './sub-literature.component.html',
  styleUrls: ['./sub-literature.component.scss']
})
export class SubLiteratureComponent implements OnInit {

  @Input() document: Subject = new Subject();
  selected: Literature | null = null;
  
  constructor() { }

  ngOnInit(): void {
  }

  select(lit: Literature) {
    this.selected = lit;
  }

  add() {
    this.selected = new Literature();
  }

  delete() {
    this.document.literature = this.document.literature.filter(l => l !== this.selected);
    this.selected = null;
  }

  save(lit: Literature) {
    this.delete();
    this.document.literature.push(lit);
  }
}
