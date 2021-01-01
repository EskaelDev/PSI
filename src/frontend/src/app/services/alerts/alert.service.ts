import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

const ERROR_MESSAGE = 'Coś poszło nie tak, spróbuj ponownie!';
const LOADING_ERROR_MESSAGE = 'Błąd ładowania danych!';
const DATA_ERROR_MESSAGE = 'Niepoprawne dane!';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  constructor(private toastrService: ToastrService) {}

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

  public showYesNoDialog(message: string): boolean {
    //todo: custom popup
    return false;
  }
}
