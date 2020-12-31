import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Role } from 'src/app/core/enums/user/role.enum';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html',
  styleUrls: ['./user-roles.component.scss']
})
export class UserRolesComponent implements OnInit {

  editableRoles: string[] = [];
  @Input() set userRoles(roles: string[]) {
    this.editableRoles = Object.assign([], roles);
  }
  @Output() rolesSaved: EventEmitter<string[]> = new EventEmitter();

  selectedRole?: Role;
  roles = Object.values(Role);
  
  constructor() { }

  ngOnInit(): void {
  }

  assignRole() {
    if (!this.editableRoles.find(r => r == this.selectedRole?.toString())) {
      this.editableRoles.push(this.selectedRole?.toString() ?? '');
    }
  }

  unassignRole(role: string) {
    this.editableRoles = this.editableRoles.filter(r => r !== role);
  }

  saveRoles() {
    this.rolesSaved.emit(Object.assign([], this.editableRoles));
  }
}
