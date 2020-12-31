import { NgModule } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { NoAccessComponent } from './no-access/no-access.component';

@NgModule({
  declarations: [
    HomeComponent,
    NavbarComponent,
    NotFoundComponent,
    NoAccessComponent,
  ],
  imports: [SharedModule],
  exports: [NavbarComponent],
})
export class HomeModule {}
