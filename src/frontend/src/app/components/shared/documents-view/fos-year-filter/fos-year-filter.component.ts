import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';

@Component({
  selector: 'app-fos-year-filter',
  templateUrl: './fos-year-filter.component.html',
  styleUrls: ['./fos-year-filter.component.scss'],
})
export class FosYearFilterComponent implements OnInit {
  @Input() isSpec: boolean = false;
  fieldsOfStudy: FieldOfStudy[] = [];
  specs: Specialization[] = [];
  years: string[] = [
    '2015/2016',
    '2016/2017',
    '2017/2018',
    '2018/2019',
    '2019/2020',
    '2020/2021',
    '2021/2022',
  ];

  selectedFos: FieldOfStudy | null = null;
  selectedSpec: Specialization | null = null;
  selectedYear: string | null = null;

  @Output() criteriaChange: EventEmitter<any> = new EventEmitter();

  constructor(private fosService: FieldOfStudyService) {}

  ngOnInit(): void {
    this.loadFieldsOfStudy();
  }

  loadFieldsOfStudy() {
    this.fosService.getFieldsOfStudies().subscribe((fieldsOfStudy) => {
      this.fieldsOfStudy = fieldsOfStudy;
    });
  }

  selectedFosChanged() {
    this.selectedSpec = null;

    if (this.selectedFos) {
      this.specs = this.selectedFos.specializations;
    } else {
      this.specs = [];
    }
    this.selectionChanged();
  }

  selectionChanged() {
    this.criteriaChange.emit({
      fos: this.selectedFos,
      year: this.selectedYear,
      spec: this.selectedSpec,
    });
  }
}
