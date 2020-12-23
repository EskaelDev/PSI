import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/authentication/login/login.component';
import { HomeComponent } from './components/home/home/home.component';
import { NoAccessComponent } from './components/home/no-access/no-access.component';
import { NotFoundComponent } from './components/home/not-found/not-found.component';
import { AuthGuard } from './interceptors/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full',
  },
  { 
    path: 'login', component: LoginComponent
  },
  { 
    path: 'home', component: HomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'noaccess',
    component: NoAccessComponent
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
