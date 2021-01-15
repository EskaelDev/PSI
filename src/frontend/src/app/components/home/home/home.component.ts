import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/core/enums/user/role.enum';

interface Icons {
  link: string;
  image: string;
  style_id: string;
  name: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {

  public icons: Icons[] = [
    { link: 'syllabus', image: 'school', style_id: 'school_icon', name: 'Programy studiów'},
    { link: 'documents', image: 'file-download', style_id: 'button_docs', name: 'Dokumenty'},
    { link: 'acceptance', image: 'done', style_id: 'button_accept', name: 'Akceptacja'},
    { link: 'admin', image: 'settings', style_id: 'button_admin', name: 'Administracja'},
    { link: 'subject', image: 'insert_drive_file', style_id: 'button_subject', name: 'Przedmioty'},
    { link: 'learning-outcome', image: 'filter_frames', style_id: 'button_learning_outcome', name: 'Efekty uczenia się'},
  ];
  ngOnInit(): void {}
}
