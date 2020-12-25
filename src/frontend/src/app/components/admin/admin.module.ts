import { NgModule } from '@angular/core';
import { UsersComponent } from './users/users.component';
import { UsersListComponent } from './users/users-list/users-list.component';
import { UserComponent } from './users/user/user.component';
import { SharedModule } from 'src/app/modules/shared.module';

@NgModule({
  declarations: [
    UsersComponent, 
    UsersListComponent, 
    UserComponent
  ],
  imports: [SharedModule],
})
export class AdminModule {}
