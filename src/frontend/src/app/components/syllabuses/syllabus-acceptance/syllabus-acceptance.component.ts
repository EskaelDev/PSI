import { Component, OnInit } from '@angular/core';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';

@Component({
  selector: 'app-syllabus-acceptance',
  templateUrl: './syllabus-acceptance.component.html',
  styleUrls: ['./syllabus-acceptance.component.scss'],
})
export class SyllabusAcceptanceComponent implements OnInit {
  syllabuses: Syllabus[] = [];
  criteria: any;

  constructor(
    private syllabusService: SyllabusService,
    private fileHelper: FileHelper,
    private alerts: AlertService
  ) {}

  ngOnInit(): void {
    this.loadSyllabuses(null, null, null);
  }

  criteriaChanged(criteria: any) {
    this.criteria = criteria;
    this.loadSyllabuses(criteria.fos?.code, criteria.spec?.code, criteria.year);
  }

  loadSyllabuses(fos: string | null, spec: string | null, year: string | null) {
    this.syllabusService.getToAccept(fos, spec, year).subscribe((res) => {
      this.syllabuses = res;
    });
  }

  download(syllabus: Syllabus) {
    this.syllabusService.pdf(syllabus.id).subscribe((res) => {
      if (res) {
        this.fileHelper.downloadItem(
          res.body,
          `Program_Studiów_${syllabus.fieldOfStudy.code}_${syllabus.specialization.code}_${syllabus.academicYear}`
        );
      }
    });
  }

  reject(syllabus: Syllabus) {
    this.alerts.showYesNoDialog('Odrzucenie', `Czy potwierdzasz odrzucenie programu studiów?`).then(res => {
      if (res) {
        this.syllabusService.reject(syllabus.id).subscribe(res => {
          if (res) {
            this.alerts.showCustomSuccessMessage('Odrzucono program studiów');
            this.loadSyllabuses(this.criteria?.fos?.code, this.criteria?.spec?.code, this.criteria?.year);
          }
        });
      }
    });
  }

  accept(syllabus: Syllabus) {
    this.alerts.showYesNoDialog('Akceptacja', `Czy potwierdzasz akceptację programu studiów?`).then(res => {
      if (res) {
        this.syllabusService.accept(syllabus.id).subscribe(res => {
          if (res) {
            this.alerts.showCustomSuccessMessage('Zaakceptowano program studiów');
            this.loadSyllabuses(this.criteria?.fos?.code, this.criteria?.spec?.code, this.criteria?.year);
          }
        });
      }
    });
  }
}
