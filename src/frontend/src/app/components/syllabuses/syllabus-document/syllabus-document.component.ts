import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { FosYearPopupPickerComponent } from '../../shared/document/fos-year-popup-picker/fos-year-popup-picker.component';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';

@Component({
  selector: 'app-syllabus-document',
  templateUrl: './syllabus-document.component.html',
  styleUrls: ['./syllabus-document.component.scss']
})
export class SyllabusDocumentComponent implements OnInit {
  title = 'programu studiów';

  syllabusDocument: Syllabus = new Syllabus();
  
  constructor(private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.snapshot.paramMap.get('fosId');
    this.route.snapshot.paramMap.get('specId');
    decodeURIComponent(this.route.snapshot.paramMap.get('year') ?? '');
  }

  save() {

  }

  saveAs() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Zapisz jako',
        isSpec: true
      }
    });

    sub.afterClosed().subscribe(result => {
      result.fos;
      result.year;
    });
  }

  import() {
    const sub = this.dialog.open(FosYearPopupPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Importuj z',
        isSpec: true
      }
    });

    sub.afterClosed().subscribe(result => {
      result.fos;
      result.year;
    });
  }

  close() {
    this.router.navigate(['/syllabus/choose']);
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
