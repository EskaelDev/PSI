import { Component, Input, OnInit } from '@angular/core';
import { ModuleType } from 'src/app/core/enums/subject/module-type.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';

@Component({
  selector: 'app-points-limit',
  templateUrl: './points-limit.component.html',
  styleUrls: ['./points-limit.component.scss']
})
export class PointsLimitComponent implements OnInit {

  @Input() document: Syllabus = new Syllabus();
  moduleTypes = Object.values(ModuleType);
  
  constructor() { }

  ngOnInit(): void {

  }

  filterElements(moduleType: ModuleType) {
    return this.document.pointLimits.filter(pl => pl.moduleType === moduleType);
  }
}
