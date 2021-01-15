import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserCredentials } from 'src/app/core/models/user/user-credentials';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { TokenStorageService } from 'src/app/services/authentication/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  isLoading = false;
  loginInvalid = false;
  navigateUrl = '/';

  loginForm: FormGroup;

  constructor(
    private authService: AuthenticationService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly fb: FormBuilder,
    private readonly tokenStorage: TokenStorageService
  ) {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    const paramKey = 'returnUrl';
    this.navigateUrl = this.route.snapshot.queryParams[paramKey] || '/';

    // if user is already logged in navigate to next page
    if (this.tokenStorage.getToken()) {
      this.router.navigate([this.navigateUrl]);
    }
  }

  login(): void {
    this.isLoading = true;
    this.loginInvalid = false;
    const credentials = new UserCredentials(
      this.loginForm.get('email')?.value,
      this.loginForm.get('password')?.value
    );
    this.authService.login(credentials).subscribe(
      () => {
        this.router.navigate([decodeURIComponent(this.navigateUrl)]);
      },
      () => {
        this.isLoading = false;
      }
    );
  }
}
