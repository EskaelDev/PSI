import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/user/role.enum';

interface Icons {
  link: string;
  image: string;
  style_id: string;
  name: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {

  ngOnInit(): void {}
}
