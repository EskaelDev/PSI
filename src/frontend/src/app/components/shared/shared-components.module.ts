import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchableListComponent } from './searchable-list/searchable-list.component';
import { SharedModule } from 'src/app/modules/shared.module';

@NgModule({
  declarations: [
    SearchableListComponent
  ],
  imports: [
    SharedModule
  ],
  exports: [
    SearchableListComponent
  ]
})
export class SharedComponentsModule { }
