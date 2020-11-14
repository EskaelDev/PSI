import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HistoryComponent } from '../shared/history/history.component';

@Component({
  selector: 'app-crud-przedmioty',
  templateUrl: './crud-przedmioty.component.html',
  styleUrls: ['./crud-przedmioty.component.scss']
})
export class CrudPrzedmiotyComponent implements OnInit {

  subjectFields = ['Kod', 'Nazwa polska', 'Nazwa angielska', 'Typ', 'Rodzaj', 'Język zajęć']

  classFields = ['Typ', 'Czas na uczelni', 'Czas w domu', 'Sposób zaliczenia', 'ECTS', 'Kończący', 'Badawczy']

  isDetails = false;
  
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  toggleDetails() {
    this.isDetails = !this.isDetails;
  }

  openDialog(): void {
    this.dialog.open(HistoryComponent, {
      height: '500px',
      width: '400px'
    });
  }

}
