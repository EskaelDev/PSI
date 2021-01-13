import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { LearningOutcomeService } from 'src/app/services/learning-outcome/learning-outcome.service';

@Component({
  selector: 'app-learning-outcome-picker',
  templateUrl: './learning-outcome-picker.component.html',
  styleUrls: ['./learning-outcome-picker.component.scss']
})
export class LearningOutcomePickerComponent implements OnInit {
  title = 'efekty uczenia się';

  constructor(private readonly router: Router,
    private learningOutcomeService: LearningOutcomeService,
    private fileHelper: FileHelper) { }

  ngOnInit(): void {
  }

  download(choice: any) {
    this.learningOutcomeService.pdfLatest(choice.fos.code, choice.year).subscribe(res => {
      if (res) {
        this.fileHelper.downloadItem(res.body, `EfektyKształcenia_${choice.fos}_${choice.year}`);
      }
    });
  }

  edit(choice: any) {
    this.router.navigate([`/learning-outcome/document/${choice.fos.code}/${encodeURIComponent(choice.year)}`]);
  }
}
