import { Component, Input,  OnInit } from '@angular/core';
import { onSideNavChange, animateText } from '../../animations/animations';
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
    {name: 'Inbox', link: 'some-link', icon: 'home'},
    {name: 'Starred', link: 'some-link', icon: 'get_app'},
    {name: 'Send email', link: 'some-link', icon: 'filter_frames'},
    {name: 'Send email', link: 'some-link', icon: 'insert_drive_file'},
    {name: 'Send email', link: 'some-link', icon: 'school'},
    {name: 'Send email', link: 'some-link', icon: 'settings'},
    {name: 'Send email', link: 'some-link', icon: 'perm_identity'},
    {name: 'Send email', link: 'some-link', icon: 'power_settings_new'},
  ];

  constructor(private sidenavService: NavbarService) { }

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
