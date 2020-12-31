import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { LearningOutcomePickerComponent } from './learning-outcome-picker/learning-outcome-picker.component';
import { LearningOutcomeDocumentComponent } from './learning-outcome-document/learning-outcome-document.component';
import { SharedComponentsModule } from '../shared/shared-components.module';

@NgModule({
  declarations: [
    LearningOutcomePickerComponent,
    LearningOutcomeDocumentComponent,
  ],
  imports: [SharedModule, SharedComponentsModule],
})
export class LearningOutcomesModule {}
