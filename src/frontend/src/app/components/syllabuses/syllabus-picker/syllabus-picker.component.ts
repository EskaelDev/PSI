import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileHelper } from 'src/app/helpers/FileHelper';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';

@Component({
  selector: 'app-syllabus-picker',
  templateUrl: './syllabus-picker.component.html',
  styleUrls: ['./syllabus-picker.component.scss']
})
export class SyllabusPickerComponent implements OnInit {
  title = 'program studiów';

  constructor(private readonly router: Router,
    private syllabusService: SyllabusService,
    private fileHelper: FileHelper) { }

  ngOnInit(): void {
  }

  download(choice: any) {
    this.syllabusService.pdfLatest(choice.fos.code, choice.spec.code, choice.year).subscribe(res => {
      if (res) {
        this.fileHelper.downloadItem(res.body, `Program_Studiów__${choice.fos}_${choice.year}`);
      }
    });
  }

  edit(choice: any) {
    this.router.navigate([`/syllabus/document/${choice.fos.code}/${choice.spec.code}/${encodeURIComponent(choice.year)}`]);
  }
}
