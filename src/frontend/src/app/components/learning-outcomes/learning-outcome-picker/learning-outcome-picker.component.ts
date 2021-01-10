import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-learning-outcome-picker',
  templateUrl: './learning-outcome-picker.component.html',
  styleUrls: ['./learning-outcome-picker.component.scss']
})
export class LearningOutcomePickerComponent implements OnInit {
  title = 'efekty uczenia się';

  constructor(private readonly router: Router) { }

  ngOnInit(): void {
  }

  download(choice: any) {
    choice.fos;
    choice.year;
  }

  edit(choice: any) {
    this.router.navigate([`/learning-outcome/document/${choice.fos.code}/${encodeURIComponent(choice.year)}`]);
  }
}
