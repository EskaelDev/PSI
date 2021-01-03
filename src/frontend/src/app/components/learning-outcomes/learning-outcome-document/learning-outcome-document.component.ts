import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { LearningOutcomeDocument } from 'src/app/core/models/learning-outcome/learning-outcome-document';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { LearningOutcomeService } from 'src/app/services/learning-outcome.service';
import { FosYearPopupPickerComponent } from '../../shared/document/fos-year-popup-picker/fos-year-popup-picker.component';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';

@Component({
  selector: 'app-learning-outcome-document',
  templateUrl: './learning-outcome-document.component.html',
  styleUrls: ['./learning-outcome-document.component.scss'],
})
export class LearningOutcomeDocumentComponent implements OnInit {
  title = 'efektów uczenia się';

  learningOutcomeDocument: LearningOutcomeDocument = new LearningOutcomeDocument();
  fosId: string = '';
  year: string = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog,
    private readonly alerts: AlertService,
    private learningOutcomeService: LearningOutcomeService
  ) {}

  ngOnInit(): void {
    this.fosId = this.route.snapshot.paramMap.get('fosId') ?? '';
    this.year = decodeURIComponent(
      this.route.snapshot.paramMap.get('year') ?? ''
    );
    this.loadLearningOutcome();
  }

  loadLearningOutcome() {
    this.learningOutcomeService
      .getLatest(this.fosId, this.year)
      .subscribe((lo) => {
        if (lo) {
          this.learningOutcomeDocument = lo;
        }
      });
  }

  save() {
    this.learningOutcomeService
      .save(this.learningOutcomeDocument)
      .subscribe((result) => {
        if (result) {
          this.alerts.showCustomSuccessMessage('Zapisano zmiany');
          this.loadLearningOutcome();
        }
      });
  }

  saveAs() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Zapisz jako',
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result) {
        this.learningOutcomeService
          .saveAs(this.learningOutcomeDocument, result.fos.code, result.year)
          .subscribe((result) => {
            if (result) {
              this.alerts.showCustomSuccessMessage('Zapisano jako');
              this.loadLearningOutcome();
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
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result) {
        this.learningOutcomeService
          .importFrom(
            this.learningOutcomeDocument.id,
            result.fos.code,
            result.year
          )
          .subscribe((result) => {
            if (result) {
              this.alerts.showCustomSuccessMessage('Zaimportowano');
              this.loadLearningOutcome();
            }
          });
      }
    });
  }

  close() {
    this.router.navigate(['/learning-outcome/choose']);
  }

  delete() {}

  pdf() {}

  history() {
    const sub = this.dialog.open(HistoryPopupComponent, {
      height: '500px',
      width: '400px',
      data: {
        // todo: pass history
        values: [],
      },
    });

    sub.componentInstance.download.subscribe(() => {
      // todo: download document pdf
    });
  }
}
