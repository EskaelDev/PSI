import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  constructor() {}

  @Input() userForm: FormGroup = new FormGroup({});
  @Input() isNew: boolean = true;
  @Output() passwordReset: EventEmitter<any> = new EventEmitter();

  ngOnInit(): void {}

  resetPassword() {
    this.passwordReset.emit();
  }
}
