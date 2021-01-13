import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { LearningOutcomeService } from 'src/app/services/learning-outcome/learning-outcome.service';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';
import { SubjectCardsComponent } from './subject-cards/subject-cards.component';

@Component({
  selector: 'app-documents',
  templateUrl: './documents.component.html',
  styleUrls: ['./documents.component.scss']
})
export class DocumentsComponent implements OnInit {

  syllabuses: Syllabus[] = [];

  constructor(
    private syllabusService: SyllabusService,
    private learningOutcomeService: LearningOutcomeService,
    private fileHelper: FileHelper,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadDocuments(null, null, null);
  }

  criteriaChanged(criteria: any) {
    this.loadDocuments(criteria.fos?.code, criteria.spec?.code, criteria.year);
  }

  loadDocuments(fos: string | null, spec: string | null, year: string | null) {
    this.syllabusService.getDocuments(fos, spec, year).subscribe((res) => {
      this.syllabuses = res;
    });
  }

  downloadSyllabus(syllabus: Syllabus) {
    this.syllabusService.pdf(syllabus.id).subscribe((res) => {
      if (res) {
        this.fileHelper.downloadItem(
          res.body,
          `Program_Studiów_${syllabus.fieldOfStudy.code}_${syllabus.specialization.code}_${syllabus.academicYear}`
        );
      }
    });
  }

  downloadPlan(syllabus: Syllabus) {
    this.syllabusService.planPdf(syllabus.id).subscribe((res) => {
      if (res) {
        this.fileHelper.downloadItem(
          res.body,
          `Plan_Studiów_${syllabus.fieldOfStudy.code}_${syllabus.specialization.code}_${syllabus.academicYear}`
        );
      }
    });
  }

  downloadLearningOutcomes(syllabus: Syllabus) {
    this.learningOutcomeService.pdfLatest(syllabus.fieldOfStudy.code, syllabus.academicYear).subscribe(res => {
      if (res) {
        this.fileHelper.downloadItem(res.body, `EfektyKształcenia_${syllabus.fieldOfStudy.code}_${syllabus.academicYear}`);
      }
    });
  }

  downloadSubjects(syllabus: Syllabus) {
    const sub = this.dialog.open(SubjectCardsComponent, {
      height: '500px',
      width: '600px',
      data: {
        syllabus: syllabus
      },
    });
  }
}
