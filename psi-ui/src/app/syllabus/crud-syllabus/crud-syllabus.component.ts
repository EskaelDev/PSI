import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HistoryComponent } from '../../shared/history/history.component';
import {FormBuilder, FormGroup} from '@angular/forms';

interface Description{
  name: string;
  value: string;
}

interface Subject{
  code: string;
  name: string;
  w: number;
  c: number;
  l: number;
  p: number;
  s: number;
  ZZU: number;
  CNPS: number;
  ECTS: number;
  examForm: string;
}

interface ModulePoints{
  module: string;
  list: string;
  points: number;
}

interface GroupsOfClasses{
  np: number;
  codeFinal: string;
  nameFinal: string;
  codePart: string;
  namePart: string;
}

interface ClassesDeadline{
  lp: number;
  code: string;
  name: string;
  deadlineSemester: number;
}

const SUBJECTS: Subject[] =
  [
    {code: 'INZ002007L', name: 'Bazy danych', w: 0, c: 0, l: 1, p: 0, s: 0, ZZU: 15, CNPS: 60, ECTS: 2, examForm: 'Zaliczenie' },
    {code: 'INZ002007Wc', name: 'Bazy danych', w: 2, c: 1, l: 0, p: 0, s: 0, ZZU: 45, CNPS: 115 , ECTS: 4, examForm: 'Egzamin' },
    {code: 'INZ002008L', name: 'Metody systemowe i decyzyjne', w: 0, c: 0, l: 1, p: 0, s: 0, ZZU: 15, CNPS: 50, ECTS: 2, examForm: 'Zaliczenie' },
    {code: 'INZ002008Wc', name: 'Metody systemowe i decyzyjne', w: 2, c: 1, l: 0, p: 0, s: 0, ZZU: 45, CNPS: 140, ECTS: 5, examForm: 'Egzamin' },
    {code: 'INZ002009L', name: 'Języki skryptowe', w: 0, c: 0, l: 2, p: 0, s: 0, ZZU: 30, CNPS: 90, ECTS: 3, examForm: 'Zaliczenie' },
    {code: 'INZ002009W', name: 'Języki skryptowe', w: 2, c: 0, l: 0, p: 0, s: 0, ZZU: 30, CNPS: 85, ECTS: 3, examForm: 'Egzamin' },
    // tslint:disable-next-line:max-line-length
    {code: 'INZ002012L', name: 'Podstawy internetu rzeczy', w: 0, c: 0, l: 2, p: 0, s: 0, ZZU: 30, CNPS: 90, ECTS: 3, examForm: 'Zaliczenie' },
    // tslint:disable-next-line:max-line-length
    {code: 'INZ002012W', name: 'Podstawy internetu rzeczy', w: 2, c: 0, l: 0, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2, examForm: 'Egzamin' },
    {code: 'INZ002013L', name: 'Hurtownie danych', w: 0, c: 0, l: 2, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2 , examForm: 'Zaliczenie' },
    {code: 'INZ002013W', name: 'Hurtownie danych', w: 2, c: 0, l: 0, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2, examForm: 'Egzamin' },
    // tslint:disable-next-line:max-line-length
    {code: 'INZ004342Wc', name: 'Logika dla informatyków', w: 2, c: 2, l: 0, p: 0, s: 0, ZZU: 60, CNPS: 150, ECTS: 5 , examForm: 'Egzamin' },
    {code: 'INZ004343L', name: 'Algorytmy i struktury danych', w: 0, c: 0, l: 2, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2 , examForm: 'Zaliczenie' },
    {code: 'INZ004343Wc', name: 'Algorytmy i struktury danych', w: 2, c: 1, l: 0, p: 0, s: 0, ZZU: 45, CNPS: 120, ECTS: 4, examForm: 'Egzamin' },
    // tslint:disable-next-line:max-line-length
    {code: 'INZ004344L', name: 'Architektura komputerów', w: 0, c: 0, l: 2, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2, examForm: 'Zaliczenie' },
    // tslint:disable-next-line:max-line-length
    {code: 'INZ004344W', name: 'Architektura komputerów', w: 2, c: 0, l: 0, p: 0, s: 0, ZZU: 30, CNPS: 60, ECTS: 2, examForm: 'Zaliczenie' }
  ];

@Component({
  selector: 'app-crud-syllabus',
  templateUrl: './crud-syllabus.component.html',
  styleUrls: ['./crud-syllabus.component.scss']
})

export class CrudSyllabusComponent implements OnInit {


  DESCRIPTION: Description[] =
    [
      {name: 'Prerekwizyty:', value: 'Rekrutacja: Konkurs wyników egzaminu maturalnego z wybranych przedmiotów'},
      {name: 'Możliwość kontynuacji studiów:', value: 'Możliwość podjęcia studiów II stopnia'},
      {name: 'Tytuł zawodowy:', value: 'inżynier'},
      {name: 'Forma zakończenia studiów:', value: 'egzamin dyplomowy'},
      {name: 'Sylwetka absolwenta:', value: ''}
      ];

  modulePoints: ModulePoints[] =
    [
      { module: 'Lista modułów kierunkowych ', list: 'Przedmioty obowiązkowe kierunkowe', points: 86 },
      { module: 'Lista modułów kierunkowych ', list: 'Przedmioty wybieralne kierunkowe', points: 57 },
      { module: 'Lista modułów kształcenia ogólnego ', list: 'Języki obce', points: 5 },
      { module: 'Lista modułów kształcenia ogólnego ', list: 'Przedmioty humanistyczno - menadżerskie', points: 9 },
      { module: 'Lista modułów kształcenia ogólnego ', list: 'Technologie informacyjne', points: 9 },
      { module: 'Lista modułów kształcenia ogólnego ', list: 'Zajęcia sportowe', points: 0 },
      { module: 'Lista modułów specjalnościowych  ', list: 'Przedmioty obowiązkowe specjalnościowe ', points: 0 },
      { module: 'Lista modułów specjalnościowych  ', list: 'Przedmioty wybieralne specjalnościowe  ', points: 0 },
      { module: 'Lista modułów z zakresu nauk podstawowych ', list: 'Fizyka', points: 10 },
      { module: 'Lista modułów z zakresu nauk podstawowych ', list: 'Matematyka', points: 29 },
      { module: 'Lista modułów z zakresu nauk podstawowych ', list: 'Elektronika i miernictwo', points: 0 },
      { module: 'Moduł praca dyplomowa ', list: 'Obowiązkowe', points: 0 },
      { module: 'Moduł praca dyplomowa ', list: 'Wybieralne', points: 0 },
      { module: 'Moduł praktyk ', list: 'Obowiązkowe', points: 0 },
      { module: 'Moduł praktyk ', list: 'Wybieralne', points: 5 }
    ];

  groupsOfClasses: GroupsOfClasses[] =
    [
      {np: 1, codeFinal: 'INZ004342W', nameFinal: 'Logika dla informatyków', codePart: 'INZ004342C' , namePart: 'Logika dla informatyków'},
      {np: 2, codeFinal: 'INZ004343W', nameFinal: 'Algorytmy i struktury danych', codePart: 'INZ004343C' , namePart: 'Algorytmy i struktury danych'},
      {np: 3, codeFinal: 'INZ004348W', nameFinal: 'Paradygmaty programowania', codePart: 'INZ004348C' , namePart: 'Paradygmaty programowania'},
      {np: 4, codeFinal: 'INZ004353W', nameFinal: 'Podstawy inżynierii oprogram.', codePart: 'INZ004353C' , namePart: 'Podstawy inżynierii oprogram.'},
      {np: 5, codeFinal: 'INZ002008W', nameFinal: 'Metody systemowe i decyzyjne', codePart: 'INZ002008C' , namePart: 'Metody systemowe i decyzyjne'},
      {np: 6, codeFinal: 'INZ002017P', nameFinal: 'Bazy danych', codePart: 'INZ002017S' , namePart: 'Bazy danych'},
      {np: 7, codeFinal: 'INZ004342W', nameFinal: 'Zespołowe przedsięwzięcie inż.', codePart: 'INZ004342C' , namePart: 'Zespołowe przedsięwzięcie inż.'},
      {np: 8, codeFinal: 'INZ004339W', nameFinal: 'Program. struktur. i obiektowe\n', codePart: 'INZ004339C' , namePart: 'Program. struktur. i obiektowe\n'},
      {np: 9, codeFinal: 'INZ004340W', nameFinal: 'Organizacja systemów komputer.', codePart: 'INZ004340C' , namePart: 'Organizacja systemów komputer.'},
      {np: 10, codeFinal: 'FZP001082W', nameFinal: 'Fizyka I', codePart: 'FZP001082C' , namePart: 'Fizyka I'},
      {np: 11, codeFinal: 'FZP001135W', nameFinal: 'Fizyka II', codePart: 'FZP001135C' , namePart: 'Fizyka II'},
      {np: 12, codeFinal: 'INZ004341W', nameFinal: 'Matematyka dyskretna', codePart: 'INZ004341C' , namePart: 'Matematyka dyskretna'},
    ];

  displayedColumns: string[] = ['code', 'tyoe', 'name', 'hoursPerWeek', 'ZZU', 'CNPS', 'ECTS', 'exam'];
  // tslint:disable-next-line:max-line-length
  // displayedColumns: string[] = ['Kod kursu/grupy kursów', 'Typ zajęć', 'Nazwa kursu/grupy kursów', 'Tygodniowa liczba godzin', 'Liczba godz. ZZU w semestrze', 'Liczba godz. CNPS w semestrze', 'Liczba pkt. ECTS w semestrze', 'Forma zaliczenia'];

  classesDeadline: ClassesDeadline[] =
    [
      {lp: 1, code: 'INZ002007L' , name: 'Bazy danych', deadlineSemester: 6 },
      {lp: 2, code: 'INZ002008L' , name: 'Metody systemowe i decyzyjne', deadlineSemester: 6 },
      {lp: 3, code: 'INZ002008Wc' , name: 'Metody systemowe i decyzyjne', deadlineSemester: 6 },
      {lp: 4, code: 'INZ002009L' , name: 'Języki skryptowe', deadlineSemester: 6 },
      {lp: 5, code: 'INZ002009W' , name: 'Języki skryptowe', deadlineSemester: 6 },
      {lp: 6, code: 'INZ002012L' , name: 'Podstawy internetu rzeczy', deadlineSemester: 6 },
      {lp: 7, code: 'INZ002012W' , name: 'Podstawy internetu rzeczy', deadlineSemester: 6 },
      {lp: 8, code: 'INZ002013L' , name: 'Hurtownie danych', deadlineSemester: 6 },
      {lp: 9, code: 'INZ002013W' , name: 'Hurtownie danych', deadlineSemester: 6 },
      {lp: 10, code: 'INZ004342Wc' , name: 'Logika dla informatyków', deadlineSemester: 6 },
    ];

  newClassesDeadline: string[] = ['Podstawy inżynierii oprogram', 'Cyberbezpieczeństwo', 'Sztucz intelig i inżyn wiedzy'];

  classesDeadlineNames: string[] = ['L.p.', 'Kod kursu', 'Nazwa kursu', 'Termin zaliczenia do... (nr semestru)'];
  examScope: string =
    '1. Podstawowe układy cyfrowe: bramki logiczne, przełączniki, układy sekwencyjne.\n' +
    '2. Arytmetyka dwójkowa, funkcje boolowskie, tablice Karnaugh.\n' +
    '3. Programowanie strukturalne - zasady. Przegląd instrukcji strukturalnych.\n' +
    '4. Programowanie obiektowe - podstawowe pojęcia, zastosowania.\n' +
    '5. Podstawowe operacje na zbiorach, funkcjach i relacjach. Rachunek zdań. Rachunek kwantyfikatorów.\n' +
    '6. Deterministyczne automaty skończone - definicja, zastosowania.\n' +
    '7. Przykładowe architektury komputerów: von Neumana, Princeton, Harvard.\n' +
    '8. Procesory typu RISC i CISC - charakterystyka, różnice.\n' +
    '9. Grafy. Drzewa rozpinające. Cykle Eulera i Hamiltona. Spójność. Algorytmy przechodzenia po grafie.\n' +
    '10. Pojęcie algorytmu. Algorytmy sortowania. Algorytmy wyszukiwania.\n' +
    '11. Podstawy analizy algorytmów. Złożoność obliczeniowa.\n' +
    '12. Warstwowa struktura systemu operacyjnego, pojęcie jądra systemu.\n' +
    '13. Model warstwowy OSI.\n' +
    '14. Protokoły warstwy łącza danych. Sieć Ethernet. Stos protokołów internetowych TCP/IP.\n' +
    '15. Protokoły warstwy aplikacji.\n' +
    '16. Techniki efektywnego programowania - przykłady.\n' +
    '17. Zarządzanie pamięcią. Typowe problemy. Wskaźniki.\n' +
    '18. Dobór paradygmatów programowania do rozwiązywania problemów informatycznych.\n' +
    '19. Programowanie funkcyjne a programowanie imperatywne.\n' +
    '20. Abstrakcyjne typy danych i ich realizacja w językach programowania.\n' +
    '21. Algorytmy identyfikacji obiektów statycznych. Analityczne i numeryczne metody optymalizacji.\n' +
    '22. Specyfika Internetu Rzeczy, obszary zastosowań, rozwiązywanie problemów z adresowaniem dużej liczby urządzeń, ich\n' +
    'rozproszeniem i bardzo dużą ilością generowanych danych\n' +
    '23. Rozwiązania sprzętowe wspierające komunikację i protokoły komunikacyjne wykorzystywane w sprzęcie wbudowanym i Internecie\n' +
    'Rzeczy\n' +
    '24. Modele baz danych. Relacyjna baza danych. Normalizacja. Transakcje.\n' +
    '25. Język SQL. Charakterystyka. Podjęzyki.\n' +
    '26. Modele cyklu życia oprogramowania.\n' +
    '27. Metodyki wytwarzania oprogramowania.\n' +
    '28. Zastosowanie list, zbiorów i słowników w języku Python.\n' +
    '29. Różnice i podobieństwa języków Java i Python\n' +
    '30. Zasady programowanie równoległego w języku skryptowym Python\n' +
    '31. UML jako język specyfikacji projektu. Diagramy i ich zastosowanie.\n' +
    '32. Wzorce architektoniczne i projektowe - klasyfikacja, przykłady, zastosowania.\n' +
    '33. Metody ochrony danych.\n' +
    '34. Podstawowe algorytmy kryptograficzne.\n' +
    '35. Wielowymiarowe modelowanie danych (transakcyjne i analityczne systemy danych, rodzaje wielowymiarowych struktur OLAP)\n' +
    '36. Proces ETL.\n' +
    '37. Wyrażenia i dyrektywy MDX.\n' +
    '38. Metody przetwarzania wiedzy w systemach ekspertowych.\n' +
    '39. Wnioskowanie w logice niemonotonicznej - zadanie planowania.';

  dataSource = SUBJECTS;
  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {
  }

  openDialog(): void {
    this.dialog.open(HistoryComponent, {
      height: '500px',
      width: '400px'
    });
  }
}
