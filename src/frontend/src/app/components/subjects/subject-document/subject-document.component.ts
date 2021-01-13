import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { SubjectCardEntryType } from 'src/app/core/enums/subject/subject-card-entry-type.enum';
import { Subject } from 'src/app/core/models/subject/subject';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { SubjectService } from 'src/app/services/subject/subject.service';
import { HistoryPopupComponent } from '../../shared/document/history-popup/history-popup.component';
import { YearSubjectPickerComponent } from '../../shared/document/year-subject-picker/year-subject-picker.component';

@Component({
  selector: 'app-subject-document',
  templateUrl: './subject-document.component.html',
  styleUrls: ['./subject-document.component.scss'],
})
export class SubjectDocumentComponent implements OnInit {
  guidEmpty = AppConsts.EMPTY_ID;
  title = 'przedmiotu';
  isLoading = true;

  subjectDocument: Subject | null = null;
  fosId: string = '';
  specId: string = '';
  code: string = '';
  year: string = '';

  readOnly: boolean = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    public dialog: MatDialog,
    private subjectService: SubjectService,
    private readonly alerts: AlertService,
    private fileHelper: FileHelper
  ) {}

  ngOnInit(): void {
    this.fosId = this.route.snapshot.paramMap.get('fosId') ?? '';
    this.specId = this.route.snapshot.paramMap.get('specId') ?? '';
    this.code = this.route.snapshot.paramMap.get('code') ?? '';
    this.year = decodeURIComponent(
      this.route.snapshot.paramMap.get('year') ?? ''
    );
    this.loadSubject();
  }

  loadSubject() {
    this.subjectService
      .getLatest(this.fosId, this.specId, this.code, this.year)
      .subscribe(
        (sub) => {
          if (sub) {
            this.subjectDocument = sub;
            this.readOnly = !sub.isAdmin && !sub.isSupervisor;
          }
          this.isLoading = false;
        },
        () => {
          this.isLoading = false;
        }
      );
  }

  save() {
    if (this.subjectDocument && this.validateDocument()) {
      this.subjectService.save(this.subjectDocument).subscribe((result) => {
        if (result) {
          this.alerts.showCustomSuccessMessage('Zapisano zmiany');
          this.loadSubject();
        }
      });
    }
    else {
      this.alerts.showCustomErrorMessage('Zapis nieudany!');
    }
  }

  import() {
    const sub = this.dialog.open(YearSubjectPickerComponent, {
      height: '500px',
      width: '500px',
      data: {
        title: 'Importuj z',
        allFields: true
      },
    });

    sub.afterClosed().subscribe((result) => {
      if (result && this.subjectDocument) {
        this.subjectService
          .importFrom(
            this.subjectDocument.id,
            result.fos.code,
            result.spec.code,
            result.subject,
            result.year
          )
          .subscribe((result) => {
            if (result) {
              this.alerts.showCustomSuccessMessage('Zaimportowano');
              this.loadSubject();
            }
          });
      }
    });
  }

  close() {
    this.router.navigate(['/subject/choose']);
  }

  delete() {
    if (this.subjectDocument) {
      this.subjectService
        .delete(this.subjectDocument.id)
        .subscribe((result) => {
          if (result) {
            this.alerts.showCustomSuccessMessage('Usunięto dokument');
            this.close();
          }
        });
    }
  }

  pdf() {
    if (this.subjectDocument) {
      this.subjectService.pdf(this.subjectDocument.id).subscribe(res => {
        this.fileHelper.downloadItem(res.body, `Karta_Przedmiotu_${this.subjectDocument?.namePl}_${this.subjectDocument?.fieldOfStudy.code}_${this.subjectDocument?.specialization.code}_${this.subjectDocument?.academicYear}_${this.subjectDocument?.version}`);
      });
    }
  }

  history() {
    if (this.subjectDocument) {
      this.subjectService
        .history(this.subjectDocument.id)
        .subscribe((history) => {
          const sub = this.dialog.open(HistoryPopupComponent, {
            height: '500px',
            width: '400px',
            data: {
              values: history,
            },
          });

          sub.componentInstance.download.subscribe((version: string) => {
            this.subjectService.pdf(version.split(':')[0]).subscribe(res => {
              this.fileHelper.downloadItem(res.body, `Karta_Przedmiotu_${this.subjectDocument?.namePl}_${this.subjectDocument?.fieldOfStudy.code}_${this.subjectDocument?.specialization.code}_${this.subjectDocument?.academicYear}_${version.split(':')[1]}`);
            });
          });
        });
    }
  }

  validateDocument(): boolean {
    let isValid = true;
    if (!this.subjectDocument?.namePl) {
      this.alerts.showCustomWarningMessage('Formularz Dane posiada niepoprawne pola!');
      isValid = false;
    }
    if (!this.validateCardEntry(SubjectCardEntryType.Prerequisite)) {
      this.alerts.showCustomWarningMessage('Tabela Wymagania wstępne posiada nieuzupełnione pola!');
      isValid = false;
    }
    if (!this.validateCardEntry(SubjectCardEntryType.Goal)) {
      this.alerts.showCustomWarningMessage('Tabela Cele przedmiotu posiada nieuzupełnione pola!');
      isValid = false;
    }
    if (!this.validateCardEntry(SubjectCardEntryType.TeachingTools)) {
      this.alerts.showCustomWarningMessage('Tabela Narzędzia dydaktyczne posiada nieuzupełnione pola!');
      isValid = false;
    }
    if (!this.validateLessons()) {
      this.alerts.showCustomWarningMessage('Do grupy kursów muszą należeć min 2 zajęcia');
      isValid = false;
    }
    return isValid;
  }

  validateCardEntry(type: SubjectCardEntryType): boolean {
    if (this.subjectDocument?.cardEntries.find(e => e.type === type)?.entries.find(e => !e.code || !e.description)) {
      return false;
    }
    return true;
  }

  validateLessons(): boolean {
    const courseGroups = this.subjectDocument?.lessons.filter(l => l.isGroup);
    return courseGroups?.length !== 1;
  }
}
