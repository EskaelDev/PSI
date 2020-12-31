import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { LearningOutcomeDocument } from 'src/app/core/models/learning-outcome/learning-outcome-document';
import { FosYearPopupPickerComponent } from '../../shared/document/fos-year-popup-picker/fos-year-popup-picker.component';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';

@Component({
  selector: 'app-learning-outcome-document',
  templateUrl: './learning-outcome-document.component.html',
  styleUrls: ['./learning-outcome-document.component.scss']
})
export class LearningOutcomeDocumentComponent implements OnInit {
  title = 'efektów uczenia się';

  learningOutcomeDocument: LearningOutcomeDocument = new LearningOutcomeDocument();
  year: string = '';
  
  constructor(private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.snapshot.paramMap.get('fosId');
    this.year = decodeURIComponent(this.route.snapshot.paramMap.get('year') ?? '');
  }

  save() {

  }

  saveAs() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '48vh',
      width: '500px',
      data: {
        title: 'Zapisz jako'
      }
    });

    sub.afterClosed().subscribe(result => {
      result.fos;
      result.year;
    });
  }

  import() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '48vh',
      width: '500px',
      data: {
        title: 'Importuj z'
      }
    });

    sub.afterClosed().subscribe(result => {
      result.fos;
      result.year;
    });
  }

  close() {
    this.router.navigate(['/learning-outcome/choose']);
  }

  delete() {

  }

  pdf() {

  }

  history() {
    const sub = this.dialog.open(HistoryPopupComponent, {
      height: '500px',
      width: '400px',
      data: {
        // todo: pass history
        values: []
      }
    });

    sub.componentInstance.download.subscribe(() => {
      // todo: download document pdf
    });
  }
}
