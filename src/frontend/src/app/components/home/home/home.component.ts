import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/user/role.enum';
import { AlertService } from 'src/app/services/alerts/alert.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  selectedRole?: Role;
  roles = Object.values(Role);

  constructor(
    private readonly alerts: AlertService,
  ) {}

  ngOnInit(): void {
  }
}
