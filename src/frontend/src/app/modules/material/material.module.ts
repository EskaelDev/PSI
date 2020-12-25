import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCardModule } from '@angular/material/card';
import { MatSortModule } from '@angular/material/sort';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
  declarations: [],
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatDialogModule,
    MatListModule,
    MatAutocompleteModule,
    MatCardModule,
    MatSortModule,
    MatCheckboxModule,
    MatSelectModule,
    MatTabsModule,
    MatFormFieldModule
  ],
  exports: [
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatDialogModule,
    MatListModule,
    MatAutocompleteModule,
    MatCardModule,
    MatSortModule,
    MatCheckboxModule,
    MatSelectModule,
    MatTabsModule,
    MatFormFieldModule
  ]
})

export class MaterialModule { }
