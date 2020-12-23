import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { SharedModule } from './modules/shared.module';
import { HomeModule } from './components/home/home.module';
import { AuthenticationModule } from './components/authentication/authentication.module';
import { AdminModule } from './components/admin/admin.module';
import { LearningOutcomesModule } from './components/learning-outcomes/learning-outcomes.module';
import { SubjectsModule } from './components/subjects/subjects.module';
import { SyllabusesModule } from './components/syllabuses/syllabuses.module';
import { DocumentsModule } from './components/documents/documents.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    SharedModule,
    HomeModule,
    AuthenticationModule,
    AdminModule,
    LearningOutcomesModule,
    SubjectsModule,
    SyllabusesModule,
    DocumentsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
