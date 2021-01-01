import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SubjectDocumentComponent } from './subject-document/subject-document.component';
import { SubjectPickerComponent } from './subject-picker/subject-picker.component';

@NgModule({
  declarations: [SubjectDocumentComponent, SubjectPickerComponent],
  imports: [SharedModule, SharedComponentsModule],
})
export class SubjectsModule {}
