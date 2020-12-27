import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchableListComponent } from './searchable-list/searchable-list.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { ElementEditComponent } from './admin/element-edit/element-edit.component';
import { AdminComponent } from './admin/admin.component';

@NgModule({
  declarations: [
    SearchableListComponent,
    ElementEditComponent,
    AdminComponent
  ],
  imports: [
    SharedModule
  ],
  exports: [
    SearchableListComponent,
    ElementEditComponent,
    AdminComponent
  ]
})
export class SharedComponentsModule { }
