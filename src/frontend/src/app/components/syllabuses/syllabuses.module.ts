import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SyllabusPickerComponent } from './syllabus-picker/syllabus-picker.component';
import { SyllabusDocumentComponent } from './syllabus-document/syllabus-document.component';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SyllabusAcceptanceComponent } from './syllabus-acceptance/syllabus-acceptance.component';
import { SylDocEditComponent } from './syllabus-document/syl-doc-edit/syl-doc-edit.component';
import { SylDescriptionComponent } from './syllabus-document/syl-doc-edit/syl-description/syl-description.component';

@NgModule({
  declarations: [SyllabusPickerComponent, SyllabusDocumentComponent, SyllabusAcceptanceComponent, SylDocEditComponent, SylDescriptionComponent],
  imports: [SharedModule, SharedComponentsModule],
})
export class SyllabusesModule {}
