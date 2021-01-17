import { Component, Input, OnInit } from '@angular/core';
import { onSideNavChange, animateText } from '../../animations/animations';
import { MatSidenav } from '@angular/material/sidenav';
import { NavbarService } from '../../services/navbar/navbar.service';
import { NgxPermissionsService } from 'ngx-permissions';

interface Page {
  link: string;
  name: string;
  icon: string;
}

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.scss'],
  animations: [onSideNavChange, animateText],
})
export class LeftMenuComponent implements OnInit {
  @Input() sidenav: MatSidenav | undefined;

  public sideNavState = false;
  public linkText = false;

  public possiblePages: Page[] = [
    { name: 'Strona główna', link: '/home', icon: 'home' },
    { name: 'Dokumenty', link: '/documents', icon: 'get_app' },
  ];

  public userNavigation: Page[] = [
    { name: '', link: '/account', icon: 'perm_identity' },
    { name: '', link: '/logout', icon: 'power_settings_new' },
  ];

  public pages: Page[] = [];

  constructor(
    private sidenavService: NavbarService,
    private readonly permissions: NgxPermissionsService
  ) {}

  onSidenavToggle(): void {
    this.sideNavState = !this.sideNavState;

    setTimeout(() => {
      this.linkText = this.sideNavState;
    }, 200);
    this.sidenavService.sideNavState$.next(this.sideNavState);
  }

  ngOnInit(): void {
    this.loadActionButtons();
  }

  loadActionButtons() {
    this.permissions.permissions$.subscribe((p) => {
      this.pages = [];
      this.possiblePages.forEach((p) => {
        this.pages.push(p);
      });

      this.permissions.hasPermission(['Admin', 'Teacher']).then((res) => {
        if (res) {
          this.pages.push({
            name: 'Programy studiów',
            link: '/subject/choose',
            icon: 'filter_frames',
          });
          this.pages.push({
            name: 'Przedmioty',
            link: '/subject/choose',
            icon: 'insert_drive_file',
          });
          this.pages.push({
            name: 'Efekty uczenia się',
            link: '/learning-outcome/choose',
            icon: 'school',
          });
        }
      });

      this.permissions
        .hasPermission(['Admin', 'StudentGovernment'])
        .then((res) => {
          if (res) {
            this.pages.push({
              name: 'Akceptacja',
              link: '/syllabus/acceptance',
              icon: 'done',
            });
          }
        });

      this.permissions.hasPermission('Admin').then((res) => {
        if (res) {
          this.pages.push({
            name: 'Administracja',
            link: '/admin/menu',
            icon: 'settings',
          });
        }
      });
    });
  }
}
