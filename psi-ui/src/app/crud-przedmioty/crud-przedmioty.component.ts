import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { HistoryComponent } from '../shared/history/history.component';

interface Iliterature {
    autor: string;
    tytul: string;
    wydawnictwo: string;
    rok: string;
    podstawowa: string;
    isbn: string;

}

const LITERAT: Iliterature[] = [
    {
        autor: 'Lech Madeyski and Tomasz Lewowski',
        isbn: '10.1145/3383219.3383264',
        podstawowa: 'Tak',
        rok: '2020',
        tytul: 'MLCQ: Industry-relevant code smell data set,” in Evaluation and Assessment in Software Engineering',
        wydawnictwo: 'ACM'
    }
]


@Component({
    selector: 'app-crud-przedmioty',
    templateUrl: './crud-przedmioty.component.html',
    styleUrls: ['./crud-przedmioty.component.scss']
})
export class CrudPrzedmiotyComponent implements OnInit {

    subjectFields = ['Prowadzący', 'Kod', 'Nazwa polska', 'Nazwa angielska', 'Typ', 'Rodzaj', 'Język zajęć']

    classFields = ['Typ',
        'Czas na uczelni',
        'Czas w domu',
        'Sposób zaliczenia',
        'ECTS zajec praktycznych',
        'ECTS zajec bezpośrednich',
        'ECTS',
        'Kończący',
        'Badawczy',
        'Grupa przedmiotow']
    literature = ['autor', 'tytul', 'wydawnictwo', 'rok', 'podstawowa', 'isbn'];
    isDetails = false;

    constructor(public dialog: MatDialog) { }

    ngOnInit(): void {
    }

    toggleDetails() {
        this.isDetails = !this.isDetails;
    }
    literatureDS = new MatTableDataSource(LITERAT);

    openDialog(): void {
        this.dialog.open(HistoryComponent, {
            height: '500px',
            width: '400px'
        });
    }

}
