import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-user-tags',
  templateUrl: './user-tags.component.html',
  styleUrls: ['./user-tags.component.scss'],
})
export class UserTagsComponent implements OnInit {
  @Input() roles: string[] = [];
  @Input() isEditing: boolean = false;
  @Output() editRoles: EventEmitter<any> = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  edit() {
    this.editRoles.emit();
  }
}
