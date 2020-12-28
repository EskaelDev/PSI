import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { AlertService } from 'src/app/services/alerts/alert.service';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';

@Component({
  selector: 'app-fos',
  templateUrl: './fos.component.html',
  styleUrls: ['./fos.component.scss'],
})
export class FosComponent implements OnInit, OnDestroy {
  subscribtions: Subscription[] = [];
  guidEmpty = AppConsts.EMPTY_ID;

  originalFos: FieldOfStudy = new FieldOfStudy();

  fosForm: FormGroup;

  specializations: Specialization[] = [];

  constructor(
    private fosService: FieldOfStudyService,
    private readonly messageHub: MessageHubService,
    private readonly alerts: AlertService,
    private readonly fb: FormBuilder
  ) {
    this.fosForm = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedFos.subscribe((fos) => {
        this.originalFos = fos;
        this.fosForm.patchValue({
          code: fos.code,
          name: fos.name,
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  saveFos() {
    const editedFos = Object.assign([], this.originalFos);
    editedFos.code = this.fosForm.get('code')?.value;
    editedFos.name = this.fosForm.get('name')?.value;

    this.fosService.saveFos(editedFos).subscribe((fos) => {
      this.messageHub.notifyFieldsOfStudiesChanged();
      this.alerts.showCustomSuccessMessage('Zmiany zapisane');
    });
  }

  removeFos() {
    this.fosService.deleteFos(this.originalFos.code).subscribe(() => {
      this.messageHub.notifyFieldsOfStudiesChanged();
      this.alerts.showCustomSuccessMessage('Kierunek usunięty');
    });
  }

  updateSpecs(newSpecializations: Specialization[]) {
    this.originalFos.specializations = newSpecializations;
  }
}
