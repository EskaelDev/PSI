import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SyllabusPickerComponent } from './syllabus-picker/syllabus-picker.component';
import { SyllabusDocumentComponent } from './syllabus-document/syllabus-document.component';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SyllabusAcceptanceComponent } from './syllabus-acceptance/syllabus-acceptance.component';
import { SylDocEditComponent } from './syllabus-document/syl-doc-edit/syl-doc-edit.component';
import { SylDescriptionComponent } from './syllabus-document/syl-doc-edit/syl-description/syl-description.component';
import { SylSubjectComponent } from './syllabus-document/syl-doc-edit/syl-subject/syl-subject.component';
import { SylSubjectEditComponent } from './syllabus-document/syl-doc-edit/syl-subject/syl-subject-edit/syl-subject-edit.component';
import { SylSubjectListElemComponent } from './syllabus-document/syl-doc-edit/syl-subject/syl-subject-list-elem/syl-subject-list-elem.component';
import { SylAcceptanceComponent } from './syllabus-document/syl-acceptance/syl-acceptance.component';
import { PointsLimitComponent } from './syllabus-document/syl-doc-edit/points-limit/points-limit.component';
import { PointsElemComponent } from './syllabus-document/syl-doc-edit/points-limit/points-elem/points-elem.component';

@NgModule({
  declarations: [
    SyllabusPickerComponent,
    SyllabusDocumentComponent,
    SyllabusAcceptanceComponent,
    SylDocEditComponent,
    SylDescriptionComponent,
    SylSubjectComponent,
    SylSubjectEditComponent,
    SylSubjectListElemComponent,
    SylAcceptanceComponent,
    PointsLimitComponent,
    PointsElemComponent,
  ],
  imports: [SharedModule, SharedComponentsModule],
})
export class SyllabusesModule {}
