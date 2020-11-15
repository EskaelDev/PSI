import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NewSubjectComponent } from '../crud-przedmioty/new-subject/new-subject.component';

interface SubjectVM {
    symbol: string;
    name: string;
    supervisor: string;
    type: string;
    kind: string;
    language: string;
}
// Technologie wsp wytw oprogr
// Inżynieria pozyskiwania i ochr
// Bezpieczeństwo sys web i mob
// Projektowanie sys informat
// Podstawy biz i ochr wł intel
// Zast rozw chmur aplik webowych
// Zaawansowane sys baz danych
const LEARING_OUTCOMES: SubjectVM[] = [
    {
        symbol: 'INZ_TWWO',
        name: 'Technologie wsp wytw oprogr',
        supervisor: 'Jan Kowalski',
        type: 'Kierunkowy',
        kind: 'Obowiązkowy',
        language: 'Polski'
    },
    {
        symbol: 'INZ_IPIO',
        name: 'Analiza matematyczna',
        supervisor: 'Jan Kowalski',
        type: 'Ogólny',
        kind: 'Matematyka',
        language: 'Polski'
    },
    {
        symbol: 'MGR_INFS',
        name: 'Praca inżynierska',
        supervisor: 'Jan Nowak',
        type: 'Praca dyplomowa',
        kind: 'Obowiązkowy',
        language: 'Polski'
    }
];

@Component({
    selector: 'app-przedmioty-lista',
    templateUrl: './przedmioty-lista.component.html',
    styleUrls: ['./przedmioty-lista.component.scss']
})

export class PrzedmiotyListaComponent implements OnInit {
    displayedColumns: string[] = [
        'symbol',
        'name',
        'supervisor',
        'type',
        'kind',
        'language',
        'action'
    ];

    @ViewChild(MatSort) sort: MatSort;

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
    }
    dataSource = new MatTableDataSource(LEARING_OUTCOMES);
    constructor(public dialog: MatDialog) { }


    ngOnInit(): void {
    }

    addSubject() {
    this.dialog.open(NewSubjectComponent, {
      height: '400px',
      width: '700px'
    });
    }

}








