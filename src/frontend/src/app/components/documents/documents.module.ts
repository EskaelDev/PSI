import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { DocumentsComponent } from './documents.component';
import { SubjectCardsComponent } from './subject-cards/subject-cards.component';

@NgModule({
  declarations: [DocumentsComponent, SubjectCardsComponent],
  imports: [SharedModule, SharedComponentsModule],
  entryComponents: [SubjectCardsComponent],
})
export class DocumentsModule {}
