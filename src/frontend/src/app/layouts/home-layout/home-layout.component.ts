import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {NavbarService} from '../../services/navbar/navbar.service';

@Component({
  selector: 'app-home-layout',
  templateUrl: './home-layout.component.html',
  styleUrls: ['./home-layout.component.scss']
})
export class HomeLayoutComponent implements OnInit {

  public onSideNavChange: boolean | undefined;

  constructor(
    private sidenavService: NavbarService) {
    this.sidenavService.sideNavState$.subscribe(res => {
      console.log(res);
      this.onSideNavChange = res;
    });
  }

  ngOnInit(): void {
  }

}
