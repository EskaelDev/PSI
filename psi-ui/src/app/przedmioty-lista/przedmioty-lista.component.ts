import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

interface SubjectVM {
    symbol: string;
    name: string;
    supervisor: string;
    ECTS: number;
    hoursAtUniversity: number;
    hoursAtHome: number;
    type: string;
    crediting: string;
    field: string;
    form: string;
    kind: string;
    group: boolean;
    specialization: string;
    level: string;

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
        ECTS: 4,
        hoursAtUniversity: 4,
        hoursAtHome: 2,
        type: 'Laboratorium',
        crediting: 'Ocena',
        field: 'Informatyka stosowana',
        form: 'Dzienne',
        kind: 'Obowiązkowy',
        group: false,
        specialization: 'Inżynieria Oprogramowania',
        level: 'Inżynier'
    },
    {
        symbol: 'INZ_IPIO',
        name: 'Inżynieria pozyskiwania i ochr',
        supervisor: 'Jan Kowalski',
        ECTS: 7,
        hoursAtUniversity: 2,
        hoursAtHome: 6,
        type: 'Projekt',
        crediting: 'Ocena',
        field: 'Informatyka stosowana',
        form: 'Zaoczne',
        kind: 'Nieobowiązkowy',
        group: false,
        specialization: 'Inżynieria Oprogramowania',
        level: 'Inżynier'
    },
    {
        symbol: 'MGR_INFS',
        name: 'Programowanie III',
        supervisor: 'Jan Nowak',
        ECTS: 2,
        hoursAtUniversity: 16,
        hoursAtHome: 75,
        type: 'Wykład',
        crediting: 'Egzamin',
        field: 'Danologia',
        form: 'Dzienne',
        kind: 'Obowiązkowy',
        group: true,
        specialization: 'Sieci neuronowe',
        level: 'Magister'
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
        'ECTS',
        'hoursAtUniversity',
        'hoursAtHome',
        'type',
        'crediting',
        'field',
        'form',
        'kind',
        'group',
        'specialization',
        'level'
    ];

    @ViewChild(MatSort) sort: MatSort;

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
    }
    dataSource = new MatTableDataSource(LEARING_OUTCOMES);
    constructor() { }


    ngOnInit(): void {
    }

}
