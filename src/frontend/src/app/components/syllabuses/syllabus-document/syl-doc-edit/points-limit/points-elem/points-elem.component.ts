import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { PointLimit } from 'src/app/core/models/syllabus/point-limit';

@Component({
  selector: 'app-points-elem',
  templateUrl: './points-elem.component.html',
  styleUrls: ['./points-elem.component.scss']
})
export class PointsElemComponent implements OnInit {

  @Input() elem: PointLimit = new PointLimit();
  pointsControl = new FormControl(0, [Validators.max(200), Validators.min(0)]);

  constructor() { }

  ngOnInit(): void {
    this.pointsControl.markAsTouched();
  }

}
