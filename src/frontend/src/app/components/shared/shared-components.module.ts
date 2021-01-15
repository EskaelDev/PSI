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
import { YesNoDialogComponent } from './yes-no-dialog/yes-no-dialog.component';
import { YearSubjectPickerComponent } from './document/year-subject-picker/year-subject-picker.component';
import { FrameComponent } from './frame/frame.component';
import { FosYearFilterComponent } from './documents-view/fos-year-filter/fos-year-filter.component';
import { DocumentTileComponent } from './documents-view/document-tile/document-tile.component';
import { BackgroudLoggedInComponent } from './backgroud-logged-in/backgroud-logged-in.component';

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
    YesNoDialogComponent,
    YearSubjectPickerComponent,
    FrameComponent,
    FosYearFilterComponent,
    DocumentTileComponent,
    BackgroudLoggedInComponent,
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
        YesNoDialogComponent,
        FrameComponent,
        FosYearFilterComponent,
        DocumentTileComponent,
        BackgroudLoggedInComponent
    ],
  entryComponents: [
    FosYearPopupPickerComponent,
    HistoryPopupComponent,
    YesNoDialogComponent,
    YearSubjectPickerComponent
  ]
})
export class SharedComponentsModule {}
