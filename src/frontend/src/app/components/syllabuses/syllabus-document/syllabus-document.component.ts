import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';
import { FosYearPopupPickerComponent } from '../../shared/document/fos-year-popup-picker/fos-year-popup-picker.component';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';

@Component({
  selector: 'app-syllabus-document',
  templateUrl: './syllabus-document.component.html',
  styleUrls: ['./syllabus-document.component.scss'],
})
export class SyllabusDocumentComponent implements OnInit {
  guidEmpty = AppConsts.EMPTY_ID;
  title = 'programu studiów';
  isLoading = true;

  syllabusDocument: Syllabus = new Syllabus();
  fosId: string = '';
  specId: string = '';
  year: string = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog,
    private readonly alerts: AlertService,
    private syllabusService: SyllabusService
  ) {}

  ngOnInit(): void {
    this.fosId = this.route.snapshot.paramMap.get('fosId') ?? '';
    this.specId = this.route.snapshot.paramMap.get('specId') ?? '';
    this.year = decodeURIComponent(
      this.route.snapshot.paramMap.get('year') ?? ''
    );
    this.loadSyllabus();
  }

  loadSyllabus() {
    this.syllabusService
      .getLatest(this.fosId, this.specId, this.year)
      .subscribe(
        (syl) => {
          if (syl) {
            console.log(syl);
            this.syllabusDocument = syl;
            this.syllabusDocument.deanName = 'Axd';
            if (this.syllabusDocument.description)
            {
              this.syllabusDocument.description.ects = 15;
              this.syllabusDocument.description.prerequisites = 'iii';
              this.syllabusDocument.description.employmentOpportunities = 'uuu';
              this.syllabusDocument.description.possibilityOfContinuation = 'sss';
            }
            
            this.syllabusDocument.intershipType = 'zzz';
            this.syllabusDocument.scopeOfDiplomaExam = "ooo";
          }
          this.isLoading = false;
        },
        () => {
          this.isLoading = false;
        }
      );
  }

  save() {
    if (this.syllabusDocument) {
      this.syllabusService.save(this.syllabusDocument).subscribe((result) => {
        if (result) {
          this.alerts.showCustomSuccessMessage('Zapisano zmiany');
          this.loadSyllabus();
        }
      });
    }
  }

  saveAs() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Zapisz jako',
        isSpec: true,
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result && this.syllabusDocument) {
        this.syllabusService
          .saveAs(
            this.syllabusDocument,
            result.fos.code,
            result.spec.code,
            result.year
          )
          .subscribe((result) => {
            if (result) {
              this.alerts.showCustomSuccessMessage('Zapisano jako');
              this.loadSyllabus();
            }
          });
      }
    });
  }

  import() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Importuj z',
        isSpec: true,
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result && this.syllabusDocument) {
        this.syllabusService
          .importFrom(
            this.syllabusDocument.id,
            result.fos.code,
            result.spec.code,
            result.year
          )
          .subscribe((result) => {
            if (result) {
              this.alerts.showCustomSuccessMessage('Zaimportowano');
              this.loadSyllabus();
            }
          });
      }
    });
  }

  close() {
    this.router.navigate(['/syllabus/choose']);
  }

  delete() {
    if (this.syllabusDocument) {
      this.syllabusService
        .delete(this.syllabusDocument.id)
        .subscribe((result) => {
          if (result) {
            this.alerts.showCustomSuccessMessage('Usunięto dokument');
          }
        });
    }
  }

  pdf() {
    if (this.syllabusDocument) {
      this.syllabusService
        .pdf(this.syllabusDocument.id, null)
        .subscribe(() => {});
    }
  }

  history() {
    if (this.syllabusDocument) {
      this.syllabusService
        .history(this.syllabusDocument.id)
        .subscribe((history) => {
          const sub = this.dialog.open(HistoryPopupComponent, {
            height: '500px',
            width: '400px',
            data: {
              values: history,
            },
          });

          sub.componentInstance.download.subscribe((version: string) => {
            this.syllabusService
              .pdf(this.syllabusDocument?.id ?? '', version)
              .subscribe(() => {});
          });
        });
    }
  }
}
