import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';

@Component({
  selector: 'app-fos-tags',
  templateUrl: './fos-tags.component.html',
  styleUrls: ['./fos-tags.component.scss']
})
export class FosTagsComponent implements OnInit {

  @Input() specs: Specialization[] = [];
  @Input() isEditing: boolean = false;
  @Output() editSpecs: EventEmitter<any> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  edit(){
    this.editSpecs.emit();
  }

}
