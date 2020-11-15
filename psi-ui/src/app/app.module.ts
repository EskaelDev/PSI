import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { CrudEfektyComponent } from './efekty/crud-efekty/crud-efekty.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HistoryComponent } from './shared/history/history.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CrudKierunkiComponent } from './admin/crud-kierunki/crud-kierunki.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './user/login/login.component';
import { ForgottenPasswordComponent } from './user/forgotten-password/forgotten-password.component';
import { AccountComponent } from './user/account/account.component';
import { PanelComponent } from './admin/panel/panel.component';
import { CrudKontaComponent } from './admin/crud-konta/crud-konta.component';
import { ChooseComponent } from './efekty/choose/choose.component';
import { ChooseSyllabusComponent } from './syllabus/choose-syllabus/choose-syllabus.component';
import { CrudSyllabusComponent } from './syllabus/crud-syllabus/crud-syllabus.component';
import {MatTabsModule} from "@angular/material/tabs";

@NgModule({
  declarations: [
    AppComponent,
    CrudEfektyComponent,
    HomeComponent,
    HistoryComponent,
    CrudKierunkiComponent,
    LoginComponent,
    ForgottenPasswordComponent,
    AccountComponent,
    PanelComponent,
    CrudKontaComponent,
    ChooseComponent,
    ChooseSyllabusComponent,
    CrudSyllabusComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    FlexLayoutModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatDialogModule,
    MatListModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatCardModule,
    MatSelectModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent},
      {path: 'efektycrud', component: CrudEfektyComponent},
      {path: 'kierunkicrud', component: CrudKierunkiComponent},
      {path: 'login', component: LoginComponent},
      {path: 'forgotten-password', component: ForgottenPasswordComponent},
      {path: 'account', component: AccountComponent},
      {path: 'admin-panel', component: PanelComponent},
      {path: 'kontacrud', component: CrudKontaComponent},
      {path: 'efektychoose', component: ChooseComponent},
      {path: 'syllabus-choose', component: ChooseSyllabusComponent},
      {path: 'syllabus-crud', component: CrudSyllabusComponent},
    ]),
    MatTabsModule
  ],
  entryComponents: [
    HistoryComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
