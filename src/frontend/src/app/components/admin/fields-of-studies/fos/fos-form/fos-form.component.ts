import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-fos-form',
  templateUrl: './fos-form.component.html',
  styleUrls: ['./fos-form.component.scss']
})
export class FosFormComponent implements OnInit {

  @Input() fosForm: FormGroup = new FormGroup({});
  @Input() isNew: boolean = true;
  
  constructor() { }

  ngOnInit(): void {
  }

}
