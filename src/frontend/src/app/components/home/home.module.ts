import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { NoAccessComponent } from './no-access/no-access.component';

@NgModule({
  declarations: [
    HomeComponent,
    NotFoundComponent,
    NoAccessComponent,
  ],
  imports: [SharedModule]
})
export class HomeModule {}
