import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-backgroud-logged-in',
  templateUrl: './backgroud-logged-in.component.html',
  styleUrls: ['./backgroud-logged-in.component.scss']
})
export class BackgroudLoggedInComponent implements OnInit {
  @Input() mainTitle: string = '';
  @Input() subtitle: string = '';

  constructor() {
  }

  ngOnInit(): void {
  }

}
