import {Component, Input, OnInit} from '@angular/core';
import {onSideNavChange, animateText} from '../../animations/animations';
import {MatSidenav} from '@angular/material/sidenav';
import {NavbarService} from '../../services/navbar/navbar.service';


interface Page {
  link: string;
  name: string;
  icon: string;
}

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss'],
  animations: [onSideNavChange, animateText]
})
export class LeftMenuComponent implements OnInit {
  @Input() sidenav: MatSidenav | undefined;

  public sideNavState = false;
  public linkText = false;

  public pages: Page[] = [
    {name: 'Strona główna', link: '/home', icon: 'home'},
    {name: 'Dokumenty', link: '/documents', icon: 'get_app'},
    {name: 'Programy studiów', link: '/syllabus/choose', icon: 'filter_frames'},
    {name: 'Przedmioty', link: '/subject/choose', icon: 'insert_drive_file'},
    {name: 'Efekty uczenia się', link: '/learning-outcome/choose', icon: 'school'},
    {name: 'Administracja', link: '/syllabus/acceptance', icon: 'settings'}
  ];

  public userNavigation: Page[] = [
    {name: '', link: '/account', icon: 'perm_identity'},
    {name: '', link: '/logout', icon: 'power_settings_new'},
  ];

  constructor(private sidenavService: NavbarService) {
  }

  onSidenavToggle(): void {
    this.sideNavState = !this.sideNavState;

    setTimeout(() => {
      this.linkText = this.sideNavState;
    }, 200);
    this.sidenavService.sideNavState$.next(this.sideNavState);
  }

  ngOnInit(): void {
  }

}
