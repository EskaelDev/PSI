import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { YesNoDialogComponent } from 'src/app/components/shared/yes-no-dialog/yes-no-dialog.component';

const ERROR_MESSAGE = 'Coś poszło nie tak, spróbuj ponownie!';
const LOADING_ERROR_MESSAGE = 'Błąd ładowania danych!';
const DATA_ERROR_MESSAGE = 'Niepoprawne dane!';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  constructor(private toastrService: ToastrService, public dialog: MatDialog) {}

  public showDefaultErrorMessage() {
    this.showCustomErrorMessage(ERROR_MESSAGE);
  }

  public showDefaultLoadingDataErrorMessage() {
    this.showCustomErrorMessage(LOADING_ERROR_MESSAGE);
  }

  public showDefaultWrongDataErrorMessage() {
    this.showCustomErrorMessage(DATA_ERROR_MESSAGE);
  }

  public showCustomErrorMessage(customMessage: string) {
    this.toastrService.error(customMessage);
  }

  public showCustomWarningMessage(customMessage: string) {
    this.toastrService.warning(customMessage);
  }

  public showCustomSuccessMessage(customMessage: string) {
    this.toastrService.success(customMessage);
  }

  public showCustomInfoMessage(customMessage: string) {
    this.toastrService.info(customMessage);
  }

  public showYesNoDialog(title: string, message: string): Promise<boolean> {
    const sub = this.dialog.open(YesNoDialogComponent, {
      height: '320px',
      width: '500px',
      data: {
        title: title,
        message: message
      },
    });
    
    return new Promise(function (resolve) {
      sub.afterClosed().subscribe((result) => {
        resolve(result);
      });
    });
  }
}
