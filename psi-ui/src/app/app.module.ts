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
import {MatSortModule} from '@angular/material/sort';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
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
import { PrzedmiotyListaComponent } from './przedmioty-lista/przedmioty-lista.component';
import { CrudPrzedmiotyComponent } from './crud-przedmioty/crud-przedmioty.component';
import { NewSubjectComponent } from './crud-przedmioty/new-subject/new-subject.component';
import { MainMenuComponent } from './home/main-menu/main-menu.component';

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
    PrzedmiotyListaComponent,
    CrudPrzedmiotyComponent,
    NewSubjectComponent,
    MainMenuComponent
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
    MatSortModule,
    MatCheckboxModule,
    MatSelectModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'efektycrud', component: CrudEfektyComponent },
      { path: 'kierunkicrud', component: CrudKierunkiComponent },
      { path: 'login', component: LoginComponent },
      { path: 'forgotten-password', component: ForgottenPasswordComponent },
      { path: 'account', component: AccountComponent },
      { path: 'admin-panel', component: PanelComponent },
      { path: 'kontacrud', component: CrudKontaComponent },
      { path: 'efektychoose', component: ChooseComponent },
      { path: 'przedmiotylista', component: PrzedmiotyListaComponent },
      { path: 'przedmiotycrud', component: CrudPrzedmiotyComponent },
      { path: 'main', component: MainMenuComponent },
    ])
  ],
  entryComponents: [
    HistoryComponent,
    NewSubjectComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
