import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { Subject } from 'src/app/core/models/subject/subject';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';
import { YearSubjectPickerComponent } from '../../shared/document/year-subject-picker/year-subject-picker.component';

@Component({
  selector: 'app-subject-document',
  templateUrl: './subject-document.component.html',
  styleUrls: ['./subject-document.component.scss']
})
export class SubjectDocumentComponent implements OnInit {
  guidEmpty = AppConsts.EMPTY_ID;
  title = 'przedmiotu';
  isLoading = true;

  subjectDocument: Subject = new Subject();
  fosId: string = '';
  specId: string = '';
  code: string = '';
  year: string = '';
  
  constructor(private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.fosId = this.route.snapshot.paramMap.get('fosId') ?? '';
    this.specId = this.route.snapshot.paramMap.get('specId') ?? '';
    this.code = this.route.snapshot.paramMap.get('code') ?? '';
    this.year = decodeURIComponent(this.route.snapshot.paramMap.get('year') ?? '');
  }

  save() {

  }

  saveAs() {
    const sub = this.dialog.open(YearSubjectPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Zapisz jako',
        allowsNew: true
      }
    });

    sub.afterClosed().subscribe(result => {
      result.fos;
      result.year;
    });
  }

  import() {
    const sub = this.dialog.open(YearSubjectPickerComponent, {
      height: '500px',
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
    this.router.navigate(['/subject/choose']);
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
