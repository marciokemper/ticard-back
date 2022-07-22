import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { Fornecedor } from '../../models/fornecedor';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { IOption } from 'ng-select';
import { Empresa } from '../../models/empresa';

@Injectable()
export class ArquivoService {
  private headers: Headers;

  constructor(private authProfile: UsuarioProfile, private commonService: CommonService, private http: Http) {
    this.headers = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      "Content-Type": "application/json"
    });
  }

  getCharacters(): Array<IOption> {
    return this.cloneOptions(ArquivoService.PLAYER_ONE);
  }

  loadCharacters(): Observable<Array<IOption>> {
    return this.loadOptions(ArquivoService.PLAYER_ONE);
  }

  getCharactersWithDisabled(): Array<IOption> {
    let characters: Array<IOption> = this.cloneOptions(ArquivoService.PLAYER_ONE);
    characters[1].disabled = true;
    characters[4].disabled = true;
    return characters;
  }

  getCharactersWithMarked(): Array<IOption> {
    let characters: Array<any> = this.cloneOptions(ArquivoService.PLAYER_ONE);
    characters[0].marked = true;
    characters[1].marked = false;
    characters[2].marked = true;
    characters[3].marked = true;
    characters[4].marked = false;
    return characters;
  }

  getCountries(): Array<IOption> {
    return this.cloneOptions(ArquivoService.COUNTRIES);
  }

  loadCountries(): Observable<Array<IOption>> {
    return this.loadOptions(ArquivoService.COUNTRIES);
  }

  private loadOptions(options: Array<IOption>): Observable<Array<IOption>> {
    return new Observable((obs) => {
      setTimeout(() => {
        obs.next(this.cloneOptions(options));
        obs.complete();
      }, 5000);
    });
  }

  private cloneOptions(options: Array<IOption>): Array<IOption> {
    return options.map(option => ({ value: option.value, label: option.label }));
  }

  private static readonly PLAYER_ONE: Array<IOption> = [
    { value: '0', label: 'Aech' },
    { value: '1', label: 'Art3mis' },
    { value: '2', label: 'Daito' },
    { value: '3', label: 'Parzival' },
    { value: '4', label: 'Shoto' }
  ];

  private static readonly COUNTRIES: Array<IOption> = [
    { value: 'AF', label: 'Afghanistan' },
    { value: 'AX', label: 'Åland Islands' },
    { value: 'AL', label: 'Albania' },
    { value: 'DZ', label: 'Algeria' },
    { value: 'AS', label: 'American Samoa' },
    { value: 'AD', label: 'Andorra' },
    { value: 'AO', label: 'Angola' },
    { value: 'AI', label: 'Anguilla' },
    { value: 'LA', label: 'Lao People\'s Democratic Republic' },
    { value: 'LV', label: 'Latvia' },
    { value: 'LB', label: 'Lebanon' },
    { value: 'LS', label: 'Lesotho' },
    { value: 'LR', label: 'Liberia' },
    { value: 'LY', label: 'Libyan Arab Jamahiriya' },
    { value: 'LI', label: 'Liechtenstein' },
    { value: 'LT', label: 'Lithuania' },
    { value: 'LU', label: 'Luxembourg' },
    { value: 'MO', label: 'Macao' },
    { value: 'MK', label: 'Macedonia, the former Yugoslav Republic of' },
    { value: 'EH', label: 'Western Sahara' },
    { value: 'YE', label: 'Yemen' },
    { value: 'ZM', label: 'Zambia' },
    { value: 'ZW', label: 'Zimbabwe' }
  ];


  public getAllByFilter(filter: string): Observable<GenericResult<Empresa[]>> {

    return this.http.get(this.commonService.getBaseUrl() + '/empresa/GetAllByFilter?filter=' + filter, { headers: this.headers })
      .map(res => res.json());

  }

  public getById(id: number): Observable<GenericResult<Empresa>> {
    return this.http.get(this.commonService.getBaseUrl() + '/empresa/' + id, { headers: this.headers })
      .map(res => res.json());
  }


  public inserir(empresa: Empresa): Observable<GenericResult<Empresa>> {

    return this.http.post(this.commonService.getBaseUrl() + '/empresa/Incluir', JSON.stringify(empresa), {
      headers: this.headers
    }).map(res => res.json());

  }


  public getAll(): Observable<GenericResult<Empresa[]>> {

    return this.http.get(this.commonService.getBaseUrl() + '/empresa/getAll/', { headers: this.headers })
      .map(res => res.json());

  }

  public put(id: number, empresa: Empresa): Observable<GenericSimpleResult> {

    return this.http.put(this.commonService.getBaseUrl() + '/empresa/' + id, JSON.stringify(empresa), { headers: this.headers })
      .map(res => res.json());

  }

  public getAllByAtivo(): Observable<GenericResult<Empresa[]>> {

    return this.http.get(this.commonService.getBaseUrl() + '/empresa/getAllByAtivo', { headers: this.headers })
      .map(res => res.json());

  }
  
  public delete(id: number): Observable<GenericSimpleResult> {
    return this.http.delete(this.commonService.getBaseUrl() + '/empresa/' + id, { headers: this.headers })
      .map(res => res.json());
  }

}
