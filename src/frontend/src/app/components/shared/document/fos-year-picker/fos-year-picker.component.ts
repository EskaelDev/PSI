import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';

@Component({
  selector: 'app-fos-year-picker',
  templateUrl: './fos-year-picker.component.html',
  styleUrls: ['./fos-year-picker.component.scss']
})
export class FosYearPickerComponent implements OnInit {

  @Input() title: string = '';
  @Input() visibleButtons: boolean = true;
  @Input() isSpec: boolean = false;
  fieldsOfStudy: FieldOfStudy[] = [];
  specs: Specialization[] = [];
  years: string[] = ['2015/2016', '2016/2017', '2017/2018', '2018/2019', '2019/2020', '2020/2021', '2021/2022'];
  @Output() downloadDoc: EventEmitter<any> = new EventEmitter();
  @Output() editDoc: EventEmitter<any> = new EventEmitter();
  @Output() selectFos: EventEmitter<FieldOfStudy | null> = new EventEmitter();
  @Output() selectSpec: EventEmitter<Specialization | null> = new EventEmitter();
  @Output() selectYear: EventEmitter<string | null> = new EventEmitter();

  selectedFos: FieldOfStudy | null = null;
  selectedSpec: Specialization | null = null;
  selectedYear: string | null = null;

  constructor(private fosService: FieldOfStudyService) { }

  ngOnInit(): void {
    this.loadFieldsOfStudy();
  }

  loadFieldsOfStudy() {
    this.fosService.getMyFieldsOfStudies().subscribe(fieldsOfStudy => {
      this.fieldsOfStudy = fieldsOfStudy;
    });
  }

  selectedFosChanged() {
    this.selectedSpec = null;
    this.selectFos.emit(this.selectedFos);
    this.selectSpec.emit(this.selectedSpec);
    
    if (this.selectedFos) {
      this.specs = this.selectedFos.specializations;
    }
    else {
      this.specs = [];
    }
  }

  download() {
    this.downloadDoc.emit({
      fos: this.selectedFos,
      spec: this.selectedSpec,
      year: this.selectedYear
    });
  }

  edit() {
    this.editDoc.emit({
      fos: this.selectedFos,
      spec: this.selectedSpec,
      year: this.selectedYear
    });
  }
}
