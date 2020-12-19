import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/role.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  selectedRole: Role = Role.Admin;
  roles = Object.values(Role);

  constructor() { }

  ngOnInit(): void {
  }
}
