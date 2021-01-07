import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { SubjectDocumentComponent } from './subject-document/subject-document.component';
import { SubjectPickerComponent } from './subject-picker/subject-picker.component';
import { AddSubjectComponent } from './subject-picker/add-subject/add-subject.component';
import { SubDocEditComponent } from './subject-document/sub-doc-edit/sub-doc-edit.component';
import { SubCardComponent } from './subject-document/sub-doc-edit/sub-card/sub-card.component';
import { SubDescriptionComponent } from './subject-document/sub-doc-edit/sub-description/sub-description.component';
import { SubLiteratureComponent } from './subject-document/sub-doc-edit/sub-literature/sub-literature.component';
import { SubLitElemComponent } from './subject-document/sub-doc-edit/sub-literature/sub-lit-elem/sub-lit-elem.component';
import { SubLitEditComponent } from './subject-document/sub-doc-edit/sub-literature/sub-lit-edit/sub-lit-edit.component';
import { SubLearnOutcComponent } from './subject-document/sub-doc-edit/sub-learn-outc/sub-learn-outc.component';
import { SubLessonsComponent } from './subject-document/sub-doc-edit/sub-lessons/sub-lessons.component';
import { SubLearnOutcElemComponent } from './subject-document/sub-doc-edit/sub-learn-outc/sub-learn-outc-elem/sub-learn-outc-elem.component';
import { SubLearnOutcEditComponent } from './subject-document/sub-doc-edit/sub-learn-outc/sub-learn-outc-edit/sub-learn-outc-edit.component';
import { SubLessonElemComponent } from './subject-document/sub-doc-edit/sub-lessons/sub-lesson-elem/sub-lesson-elem.component';
import { SubLessonEditComponent } from './subject-document/sub-doc-edit/sub-lessons/sub-lesson-edit/sub-lesson-edit.component';

@NgModule({
  declarations: [
    SubjectDocumentComponent,
    SubjectPickerComponent,
    AddSubjectComponent,
    SubDocEditComponent,
    SubCardComponent,
    SubDescriptionComponent,
    SubLiteratureComponent,
    SubLitElemComponent,
    SubLitEditComponent,
    SubLearnOutcComponent,
    SubLessonsComponent,
    SubLearnOutcElemComponent,
    SubLearnOutcEditComponent,
    SubLessonElemComponent,
    SubLessonEditComponent,
  ],
  imports: [SharedModule, SharedComponentsModule],
  entryComponents: [AddSubjectComponent],
})
export class SubjectsModule {}
