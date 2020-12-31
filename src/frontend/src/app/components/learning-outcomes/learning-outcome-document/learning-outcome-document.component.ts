import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';

@Component({
  selector: 'app-learning-outcome-document',
  templateUrl: './learning-outcome-document.component.html',
  styleUrls: ['./learning-outcome-document.component.scss']
})
export class LearningOutcomeDocumentComponent implements OnInit {
  title = 'efektów uczenia się';

  constructor(private readonly route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.snapshot.paramMap.get('fosId');
    decodeURIComponent(this.route.snapshot.paramMap.get('year') ?? '');
  }

}
