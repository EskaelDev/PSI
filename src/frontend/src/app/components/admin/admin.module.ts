import { NgModule } from '@angular/core';
import { UsersComponent } from './users/users.component';
import { UsersListComponent } from './users/users-list/users-list.component';
import { UserComponent } from './users/user/user.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { SharedComponentsModule } from '../shared/shared-components.module';
import { UserFormComponent } from './users/user/user-form/user-form.component';
import { AdministrationComponent } from './administration/administration.component';
import { UserRolesComponent } from './users/user/user-roles/user-roles.component';

@NgModule({
  declarations: [UsersComponent, UsersListComponent, UserComponent, UserFormComponent, AdministrationComponent, UserRolesComponent],
  imports: [SharedModule, SharedComponentsModule],
})
export class AdminModule {}
