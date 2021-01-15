import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Subscription } from 'rxjs';
import { AppConsts } from 'src/app/core/consts/app-consts';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { Specialization } from 'src/app/core/models/field-of-study/specialization';
import { User } from 'src/app/core/models/user/user';
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
  isEditSpecs = false;

  originalFos: FieldOfStudy = new FieldOfStudy();

  fosForm: FormGroup;
  supervisors: User[] = [];

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
      language: ['', Validators.required],
      level: ['', Validators.required],
      type: ['', Validators.required],
      profile: ['', Validators.required],
      discipline: [''],
      branchOfScience: [''],
      supervisor: [
        '',
        [this.autocompleteObjectValidator(), Validators.required],
      ],
    });
  }

  ngOnInit(): void {
    this.loadSupervisors();
    this.subscribtions.push(
      this.messageHub.selectedFos.subscribe((fos) => {
        this.originalFos = fos;
        this.fosForm.patchValue({
          code: fos.code,
          name: fos.name,
          language: fos.language,
          level: fos.level,
          type: fos.type,
          profile: fos.profile,
          discipline: fos.discipline,
          branchOfScience: fos.branchOfScience,
          supervisor: fos.supervisor,
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (control.value instanceof User) {
        return null;
      }
      return { invalidAutocompleteObject: { value: control.value } };
    };
  }

  loadSupervisors() {
    this.fosService.getPossibleSupervisors().subscribe((supervisors) => {
      this.supervisors = supervisors;
    });
  }

  saveFos() {
    const editedFos = Object.assign(this.originalFos, this.fosForm.value);

    this.fosService.saveFos(editedFos).subscribe((result) => {
      if (result) {
        this.messageHub.notifyFieldsOfStudiesChanged();
        this.alerts.showCustomSuccessMessage('Zmiany zapisane');
      }
    });
  }

  removeFos() {
    this.alerts
      .showYesNoDialog(
        'Usunięcie kierunku',
        `Czy napewno usunąć kierunek ${this.originalFos.name}?`
      )
      .then((res) => {
        if (res) {
          this.fosService
            .deleteFos(this.originalFos.code)
            .subscribe((result) => {
              if (result) {
                this.messageHub.notifyFieldsOfStudiesChanged();
                this.alerts.showCustomSuccessMessage('Kierunek usunięty');
              }
            });
        }
      });
  }

  openEditSpecsWindow() {
    this.isEditSpecs = true;
  }

  updateSpecs(newSpecializations: Specialization[]) {
    this.originalFos.specializations = newSpecializations;
    this.isEditSpecs = false;
  }
}
