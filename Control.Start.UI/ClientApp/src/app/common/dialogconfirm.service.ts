import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
@Injectable()
export class DialogConfirmService {
  swal(message?: string) {
    return new Promise(resolve => {
      return resolve(window.confirm(message || 'Confirma ?'));    

    });
  }
}
