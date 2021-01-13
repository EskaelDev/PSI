import { Component, Input, OnInit } from '@angular/core';
import { Literature } from 'src/app/core/models/subject/literature';
import { Subject } from 'src/app/core/models/subject/subject';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-sub-literature',
  templateUrl: './sub-literature.component.html',
  styleUrls: ['./sub-literature.component.scss']
})
export class SubLiteratureComponent implements OnInit {

  @Input() readOnly: boolean = true;
  @Input() document: Subject = new Subject();
  selected: Literature | null = null;
  
  constructor(private alerts: AlertService) { }

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
    if (this.checkIsbnIsUnique(lit)) {
      this.delete();
      this.document.literature.push(lit);
    }
    else {
      this.alerts.showValidationFailMessage('Literatura o podanym numerze ISBN już istnieje!');
    }
  }

  checkIsbnIsUnique(lit: Literature): boolean {
    if(this.document.literature.find(l => l.isbn === lit.isbn) && this.selected?.isbn !== lit.isbn) {
      return false;
    }
    return true;
  }
}
