import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/throw';
import { Response } from '@angular/http';
import { environment } from '../../environments/environment';

@Injectable()
export class CommonService {
  //private baseUrl = 'https://api.contas.controlnex.com.br/api';
  private baseUrl = environment.apiUrl;
  
  constructor() { }

  getBaseUrl(): string {
    return this.baseUrl;
  }

  handleFullError(error: Response) {
    return Observable.throw(error);
  }

  handleError(error: Response): Observable<any> {
    let errorMessage = error.json();
    console.error(errorMessage);
    return Observable.throw(errorMessage.error || 'Server error');
  }
}
