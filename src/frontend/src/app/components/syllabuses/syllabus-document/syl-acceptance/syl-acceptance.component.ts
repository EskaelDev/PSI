import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Opinion } from 'src/app/core/enums/syllabus/opinion.enum';
import { State } from 'src/app/core/enums/syllabus/state.enum';
import { Syllabus } from 'src/app/core/models/syllabus/syllabus';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { SyllabusService } from 'src/app/services/syllabus/syllabus.service';

@Component({
  selector: 'app-syl-acceptance',
  templateUrl: './syl-acceptance.component.html',
  styleUrls: ['./syl-acceptance.component.scss'],
})
export class SylAcceptanceComponent implements OnInit {
  @Input() document: Syllabus = new Syllabus();
  @Output() changed: EventEmitter<any> = new EventEmitter();

  constructor(
    private syllabusService: SyllabusService,
    private readonly alerts: AlertService
  ) {}

  ngOnInit(): void {}

  canSend(document: Syllabus) {
    return (
      document.state === State.Draft ||
      (document.state === State.Rejected &&
        document.studentGovernmentOpinion === Opinion.Rejected)
    );
  }

  getActionText(document: Syllabus) {
    if (document.state === State.Draft) return 'Wyślij do akceptacji';
    if (
      document.state === State.Rejected &&
      document.studentGovernmentOpinion === Opinion.Rejected
    )
      return 'Wyślij ponownie do akceptacji';
    return 'Wyślij';
  }

  send() {
    this.alerts.showYesNoDialog('Wysłanie do akceptacji', 'Czy potwierdzasz wysłanie dokumentu do akceptacji? Dokument zostanie zapisany').then(res => {
      if (res) {
        this.syllabusService.sendToAcceptance(this.document).subscribe((result) => {
          if (result) {
            this.alerts.showCustomInfoMessage('Dokument przeszedł w stan tylko do odczytu');
            this.alerts.showCustomSuccessMessage('Wysłano do akceptacji');
            this.changed.emit();
          }
        });
      }
    })
  }

  verify() {
    this.syllabusService.verify(this.document).subscribe(result => {
      if (result) {
        this.alerts.showCustomSuccessMessage('Zweryfikowano');
      }
    });
  }
}
