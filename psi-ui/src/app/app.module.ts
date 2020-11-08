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
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CrudKierunkiComponent } from './admin/crud-kierunki/crud-kierunki.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    CrudEfektyComponent,
    HomeComponent,
    HistoryComponent,
    CrudKierunkiComponent
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
    MatSelectModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'efektycrud', component: CrudEfektyComponent },
      { path: 'kierunkicrud', component: CrudKierunkiComponent },
    ])
  ],
  entryComponents: [
    HistoryComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
