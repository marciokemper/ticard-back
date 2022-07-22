import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { GenericSimpleResult, GenericResult } from '../../models/genericResult';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { HttpClient, HttpResponse } from '@angular/common/http';

@Injectable()
export class NavMenuService {
  //private baseUrl: string = "http://162.241.41.48:5000/api/tela";

  constructor(private httpClient: HttpClient) {

  }

  //public list(): Observable<GenericResult<Tela[]>> {

  //  return this.httpClient.get(this.baseUrl)
  //    .map((response: any) => response.json());
  //}

}
