import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.scss']
})
export class DocumentComponent implements OnInit {
  @Input() title: string = '';
  
  constructor() { }

  ngOnInit(): void {
  }
}
