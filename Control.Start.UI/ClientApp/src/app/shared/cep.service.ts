import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { GenericResult } from '../../models/genericResult';

@Injectable()
export class CepService {
  constructor(private http: Http) { }

  consultaCEP(cep: string) {

    cep = cep.replace(/\D/g, '');

    if (cep !== '') {
      const validacep = /^[0-9]{8}$/;

      if (validacep.test(cep)) {
        return  this.http.get(`//viacep.com.br/ws/${cep}/json`);
      }
    }

    return null;
  }
}


