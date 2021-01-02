import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SubjectDocumentComponent } from './subject-document/subject-document.component';
import { SubjectPickerComponent } from './subject-picker/subject-picker.component';
import { AddSubjectComponent } from './subject-picker/add-subject/add-subject.component';

@NgModule({
  declarations: [
    SubjectDocumentComponent,
    SubjectPickerComponent,
    AddSubjectComponent,
  ],
  imports: [SharedModule, SharedComponentsModule],
  entryComponents: [AddSubjectComponent],
})
export class SubjectsModule {}
