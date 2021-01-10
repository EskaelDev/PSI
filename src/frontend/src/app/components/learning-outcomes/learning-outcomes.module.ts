import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { LearningOutcomePickerComponent } from './learning-outcome-picker/learning-outcome-picker.component';
import { LearningOutcomeDocumentComponent } from './learning-outcome-document/learning-outcome-document.component';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { LoDocEditComponent } from './learning-outcome-document/lo-doc-edit/lo-doc-edit.component';
import { LoListComponent } from './learning-outcome-document/lo-doc-edit/lo-list/lo-list.component';
import { LoEditComponent } from './learning-outcome-document/lo-doc-edit/lo-edit/lo-edit.component';
import { LoListElemComponent } from './learning-outcome-document/lo-doc-edit/lo-list/lo-list-elem/lo-list-elem.component';

@NgModule({
  declarations: [
    LearningOutcomePickerComponent,
    LearningOutcomeDocumentComponent,
    LoDocEditComponent,
    LoListComponent,
    LoEditComponent,
    LoListElemComponent,
  ],
  imports: [SharedModule, SharedComponentsModule],
})
export class LearningOutcomesModule {}
