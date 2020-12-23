import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/user/role.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  selectedRole?: Role;
  roles = Object.values(Role);

  ngOnInit(): void {
  }
}
