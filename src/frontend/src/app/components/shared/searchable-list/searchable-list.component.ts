import { Component, EventEmitter, Input, Output } from '@angular/core';
import { element } from 'protractor';
import { ListElement } from 'src/app/core/models/shared/list-element';

@Component({
  selector: 'app-searchable-list',
  templateUrl: './searchable-list.component.html',
  styleUrls: ['./searchable-list.component.scss']
})
export class SearchableListComponent {
  
  @Input() isLoading = true;
  @Input() selectedId: string = '';

  @Output() elementSelected: EventEmitter<any> = new EventEmitter();
  @Output() newElementChosen: EventEmitter<any> = new EventEmitter();
  @Output() back: EventEmitter<any> = new EventEmitter();

  searchPhrase = '';

  _elements: ListElement[] = [];
  @Input() set elements(elements: ListElement[]) {
    this._elements = elements;
    this.filterElements();
  }
  filteredElements: ListElement[] = [];

  constructor() {}

  filterElements() {
    if (this.searchPhrase?.length > 0) {
      this.filteredElements = Object.assign(
        [],
        this._elements.filter(
          (element) =>
            element.name
              .toLowerCase()
              .includes(this.searchPhrase.toLowerCase())
        )
      );
    } else {
      this.filteredElements = Object.assign([], this._elements);
    }
    
  }

  selectElement(element: ListElement) {
    this.elementSelected.emit(element.id);
  }

  newElement() {
    this.newElementChosen.emit();
  }

  goBack() {
    this.back.emit();
  }
}
