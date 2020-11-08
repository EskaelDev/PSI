import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HistoryComponent } from '../shared/history/history.component';

export interface LearningOutcome {
  symbol: string;
  description: string;
  universal: string;
  level6: string;
  level6eng: string;
}

const LEARING_OUTCOMES: LearningOutcome[] = [
  {symbol: 'KINF_W01', description: 'Posiada podstawową wiedzę ogólną z zakresu wybranych gałęzi matematyki: analizy matematycznej, algebry liniowej i geometrii analitycznej, logiki matematycznej, matematyki dyskretnej oraz rachunku prawdopodobieństwa i statystyki matematycznej, tworzącą podstawy teoretyczne konieczne do rozwiązywania informatycznych problemów inżynierskich', universal: 'P6U_W', level6: 'P6S_WG', level6eng: ''},
  {symbol: 'KINF_W03', description: 'Ma podstawową wiedzę w zakresie wybranych działów fizyki', universal: 'P6U_W', level6: 'P6S_WG', level6eng: ''},
  {symbol: 'KINF_W04', description: 'Zna i rozumie podstawowe struktury danych, algorytmy oraz konstrukcje programistyczne w różnych językach programowania', universal: 'P6U_W', level6: 'P6S_WG', level6eng: 'P6S_WG_inż'},
];


@Component({
  selector: 'app-crud-efekty',
  templateUrl: './crud-efekty.component.html',
  styleUrls: ['./crud-efekty.component.scss']
})
export class CrudEfektyComponent implements OnInit {

  displayedColumns: string[] = ['symbol', 'description', 'universal', 'level6', 'level6eng', 'actions'];
  dataSource = LEARING_OUTCOMES;

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
