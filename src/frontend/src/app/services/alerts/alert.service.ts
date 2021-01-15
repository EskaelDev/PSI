import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { YesNoDialogComponent } from 'src/app/components/shared/yes-no-dialog/yes-no-dialog.component';

const ERROR_MESSAGE = 'Coś poszło nie tak, spróbuj ponownie!';
const LOADING_ERROR_MESSAGE = 'Błąd ładowania danych!';
const DATA_ERROR_MESSAGE = 'Niepoprawne dane!';
const DOC_DOWNLOAD_FAIL_MESSAGE = 'Błąd pobierania dokumentu!';
const ELEMENT_SAVE_ERROR_MESSAGE = 'Zapis elementu nieudany!';

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

  public showValidationFailMessage(validationMessage: string) {
    this.showCustomWarningMessage(validationMessage);
    this.showCustomErrorMessage(ELEMENT_SAVE_ERROR_MESSAGE);
  }

  public showDefaultDocumentDownloadFailMessage() {
    this.showCustomErrorMessage(DOC_DOWNLOAD_FAIL_MESSAGE);
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
