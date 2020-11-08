import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { CrudEfektyComponent } from './crud-efekty/crud-efekty.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HistoryComponent } from './shared/history/history.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';

@NgModule({
  declarations: [
    AppComponent,
    CrudEfektyComponent,
    HomeComponent,
    HistoryComponent
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
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'efektycrud', component: CrudEfektyComponent },
    ])
  ],
  entryComponents: [
    HistoryComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
