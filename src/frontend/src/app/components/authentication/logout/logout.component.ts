import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

  constructor(
    private readonly authService: AuthenticationService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.logout();
    this.router.navigate(['/']);
  }

  logout() {
    this.authService.logout();
  }

}
