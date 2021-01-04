import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/user/user';
import { TokenStorageService } from 'src/app/services/authentication/token-storage.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  user: User | null = null;

  constructor(private tokenService: TokenStorageService) { }

  ngOnInit(): void {
    this.user = this.tokenService.getUser();
    console.log(this.user);
  }

}
