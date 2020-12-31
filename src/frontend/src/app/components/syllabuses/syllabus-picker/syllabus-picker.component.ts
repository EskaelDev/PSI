import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-syllabus-picker',
  templateUrl: './syllabus-picker.component.html',
  styleUrls: ['./syllabus-picker.component.scss']
})
export class SyllabusPickerComponent implements OnInit {
  title = 'program studiów';

  constructor(private readonly router: Router) { }

  ngOnInit(): void {
  }

  download(choice: any) {
    choice.fos;
    choice.year;
  }

  edit(choice: any) {
    this.router.navigate([`/syllabus/document/${choice.fos.id}/${encodeURIComponent(choice.year)}`]);
  }
}
