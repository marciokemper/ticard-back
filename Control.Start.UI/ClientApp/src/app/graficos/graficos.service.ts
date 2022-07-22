import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { Observable } from 'rxjs/Observable';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';

@Injectable()
export class GraficosService {
  private headers: Headers;

  constructor(private authProfile: UsuarioProfile, private commonService: CommonService, private http: Http) {
    this.headers = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      "Content-Type": "application/json"
    });
  }

  public getStatus(): Observable<GenericResult<any>> {

    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
    //let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()

    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetStatusArquivoColetado?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1, { headers: this.headers })
      .map(res => res.json());
  }

  public getValorPorMesProduto(): Observable<GenericResult<any>> {
    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
    //let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()
    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetValorPorMes?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1 + "&typeInvoice=1", { headers: this.headers })
        .map(res => res.json());
  }
  public getValorPorMesServico(): Observable<GenericResult<any>> {
    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
    //let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()
    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetValorPorMes?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1 + "&typeInvoice=2", { headers: this.headers })
        .map(res => res.json());
  }
  public getUltimosArquivoColetado(): Observable<GenericResult<any>> {

    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
    //let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()
    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetUltimosArquivoColetado?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1, { headers: this.headers })
      .map(res => res.json());
  }

  public getValorPorTipo(): Observable<GenericResult<any>> {

    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
    //let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()
    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetTipo?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1, { headers: this.headers })
      .map(res => res.json());
  }

  public getValorTipoMes(): Observable<GenericResult<any>> {

    //let LoggedclienteId = this.authProfile.usuarioProfile.currentUsuario.fornecedor != null ? this.authProfile.usuarioProfile.currentUsuario.id : null;
   // let LoggedEmpresaId = this.authProfile.usuarioProfile.currentUsuario.empresaId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaId.toString()
    return this.http.get(this.commonService.getBaseUrl() + '/NotaFiscal/GetValorTipoMes?LoggedclienteId=' + 1 + '&LoggedEmpresaId=' + 1, { headers: this.headers })
      .map(res => res.json());
  }


}
