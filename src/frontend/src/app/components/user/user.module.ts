import { NgModule } from '@angular/core';
import { AccountComponent } from './account/account.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';



@NgModule({
  declarations: [AccountComponent],
  imports: [
    SharedModule, SharedComponentsModule
  ]
})
export class UserModule { }
