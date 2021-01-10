import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FieldOfStudy } from 'src/app/core/models/field-of-study/field-of-study';
import { ListElement } from 'src/app/core/models/shared/list-element';
import { FieldOfStudyService } from 'src/app/services/field-of-study/field-of-study.service';
import { MessageHubService } from 'src/app/services/message-hub/message-hub.service';

@Component({
  selector: 'app-fos-list',
  templateUrl: './fos-list.component.html',
  styleUrls: ['./fos-list.component.scss'],
})
export class FosListComponent implements OnInit, OnDestroy {
  subscribtions: Subscription[] = [];
  isLoading = true;

  fieldsOfStudy: FieldOfStudy[] = [];
  selectedFos: FieldOfStudy = new FieldOfStudy();

  constructor(
    private fosService: FieldOfStudyService,
    private readonly messageHub: MessageHubService,
    private route: Router
  ) {}

  ngOnInit(): void {
    this.subscribtions.push(
      this.messageHub.selectedFos.subscribe((fos) => {
        this.selectedFos = fos;
      }),
      this.messageHub.fieldsOfStudyChanged.subscribe(() => {
        this.loadFieldsOfStudies();
      })
    );

    this.loadFieldsOfStudies();
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach((s) => {
      s.unsubscribe();
    });
  }

  loadFieldsOfStudies() {
    this.isLoading = true;
    const selectedFosCode = this.selectedFos.code;

    this.fosService.getFieldsOfStudies().subscribe(
      (fieldsOfStudies) => {
        this.fieldsOfStudy = fieldsOfStudies;
        const foundFos = this.fieldsOfStudy.find(
          (u) => u.code === selectedFosCode
        );
        this.messageHub.notifySelectedFos(foundFos ?? new FieldOfStudy());
        this.isLoading = false;
      },
      () => {
        this.fieldsOfStudy = [];
        this.messageHub.notifySelectedFos(new FieldOfStudy());
        this.isLoading = false;
      }
    );
  }

  selectFos(code: string) {
    const fos = this.fieldsOfStudy.find((u) => u.code === code);
    if (fos) {
      this.messageHub.notifySelectedFos(fos);
    }
  }

  newFos() {
    this.messageHub.notifySelectedFos(new FieldOfStudy());
  }

  getElements(users: FieldOfStudy[]): ListElement[] {
    return users.map((u) => new ListElement(u.code, u.name ?? ''));
  }
}
