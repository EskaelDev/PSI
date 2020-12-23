import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

const ERROR_MESSAGE = 'Something went wrong, try again!';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  constructor(private toastrService: ToastrService) {}

  public showDefaultErrorMessage() {
    this.showCustomErrorMessage(ERROR_MESSAGE);
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

  public showYesNoDialog(message: string): any {
    //todo: custom popup
  }
}
