import { NgModule } from '@angular/core';
import { SearchableListComponent } from './searchable-list/searchable-list.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { ElementEditComponent } from './admin/element-edit/element-edit.component';
import { AdminComponent } from './admin/admin.component';
import { FosYearPickerComponent } from './document/fos-year-picker/fos-year-picker.component';
import { FosDetailsComponent } from './document/fos-details/fos-details.component';
import { ControlButtonsPanelComponent } from './document/control-buttons-panel/control-buttons-panel.component';
import { DocumentComponent } from './document/document.component';
import { FosYearPopupPickerComponent } from './document/fos-year-popup-picker/fos-year-popup-picker.component';
import { HistoryPopupComponent } from './document/history-popup/history-popup.component';
import { FosDetailsElementComponent } from './document/fos-details/fos-details-element/fos-details-element.component';

@NgModule({
  declarations: [
    SearchableListComponent,
    ElementEditComponent,
    AdminComponent,
    FosYearPickerComponent,
    FosDetailsComponent,
    ControlButtonsPanelComponent,
    DocumentComponent,
    FosYearPopupPickerComponent,
    HistoryPopupComponent,
    FosDetailsElementComponent,
  ],
  imports: [SharedModule],
  exports: [
    SearchableListComponent,
    ElementEditComponent,
    AdminComponent,
    FosYearPickerComponent,
    FosDetailsComponent,
    ControlButtonsPanelComponent,
    DocumentComponent,
    FosYearPopupPickerComponent,
    HistoryPopupComponent,
  ],
  entryComponents: [
    FosYearPopupPickerComponent,
    HistoryPopupComponent
  ]
})
export class SharedComponentsModule {}
