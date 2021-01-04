import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SyllabusPickerComponent } from './syllabus-picker/syllabus-picker.component';
import { SyllabusDocumentComponent } from './syllabus-document/syllabus-document.component';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SyllabusAcceptanceComponent } from './syllabus-acceptance/syllabus-acceptance.component';

@NgModule({
  declarations: [SyllabusPickerComponent, SyllabusDocumentComponent, SyllabusAcceptanceComponent],
  imports: [SharedModule, SharedComponentsModule],
})
export class SyllabusesModule {}
