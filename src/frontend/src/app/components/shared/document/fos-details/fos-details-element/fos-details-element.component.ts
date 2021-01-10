import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-fos-details-element',
  templateUrl: './fos-details-element.component.html',
  styleUrls: ['./fos-details-element.component.scss']
})
export class FosDetailsElementComponent implements OnInit {

  @Input() name: string = '';
  @Input() content: string = '';
  @Input() translatedContent: string | null = null;

  constructor() { }

  ngOnInit(): void {
  }

}
