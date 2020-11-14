import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-crud-przedmioty',
  templateUrl: './crud-przedmioty.component.html',
  styleUrls: ['./crud-przedmioty.component.scss']
})
export class CrudPrzedmiotyComponent implements OnInit {

  constructor() { }

  subjectFields = ['Kod', 'Nazwa polska', 'Nazwa angielska', 'Typ', 'Rodzaj', 'Język zajęć']

  classFields = ['Typ', 'Czas na uczelni', 'Czas w domu', 'Sposób zaliczenia', 'ECTS', 'Kończący', 'Badawczy']
  ngOnInit(): void {
  }

}
