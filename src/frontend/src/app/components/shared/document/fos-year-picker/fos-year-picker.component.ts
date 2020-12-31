import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';

@Component({
  selector: 'app-fos-year-picker',
  templateUrl: './fos-year-picker.component.html',
  styleUrls: ['./fos-year-picker.component.scss']
})
export class FosYearPickerComponent implements OnInit {

  @Input() title: string = '';
  fieldsOfStudy: FieldOfStudy[] = [];
  years: string[] = ['2015/2016', '2016/2017', '2017/2018', '2018/2019', '2019/2020', '2020/2021', '2021/2022'];
  @Output() downloadDoc: EventEmitter<any> = new EventEmitter();
  @Output() editDoc: EventEmitter<any> = new EventEmitter();

  selectedFos: FieldOfStudy | null = null;
  selectedYear: string | null = null;

  constructor(private fosService: FieldOfStudyService) { }

  ngOnInit(): void {
  }

  loadFieldsOfStudy() {
    this.fosService.getMyFieldsOfStudies().subscribe(fieldsOfStudy => {
      this.fieldsOfStudy = fieldsOfStudy;
    })
  }

  download() {
    this.downloadDoc.emit({
      fos: this.selectedFos,
      year: this.selectedYear
    });
  }

  edit() {
    this.editDoc.emit({
      fos: this.selectedFos,
      year: this.selectedYear
    });
  }
}
