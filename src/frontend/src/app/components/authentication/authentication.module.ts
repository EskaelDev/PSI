import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
  declarations: [LoginComponent, LogoutComponent],
  imports: [SharedModule],
})
export class AuthenticationModule {}
