import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { Configuracao } from '../../models/configuracao';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';


@Injectable()
export class ConfiguracaoService {
  private headers: Headers;

  constructor(private authProfile: UsuarioProfile, private commonService: CommonService,private http: Http) {
    this.headers = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      "Content-Type": "application/json"
    });
  }

  public get(): Observable<GenericResult<Configuracao>> {

    let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId.toString();

    return this.http.get(this.commonService.getBaseUrl() + '/configuracao?loggedEmpresaId=' + LoggedclienteId, { headers: this.headers })
      .map(res => res.json());

 
  }
   
  public inserir(configuracao: Configuracao): Observable<GenericResult<Configuracao>> {
    return this.http.post(this.commonService.getBaseUrl() + '/configuracao/Incluir', JSON.stringify(configuracao), {
      headers: this.headers
    }).map(res => res.json());

  }

  public put(id: number, configuracao: Configuracao): Observable<GenericSimpleResult> {
    return this.http.put(this.commonService.getBaseUrl() + '/configuracao/' + id, JSON.stringify(configuracao), { headers: this.headers })
      .map(res => res.json());

  }
  

}
