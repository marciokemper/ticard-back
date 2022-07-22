import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { Documento } from '../../models/documento';

@Injectable()
export class DocumentoService {
  private headers: Headers;

  constructor(private authProfile: UsuarioProfile, private commonService: CommonService, private http: Http) {
    this.headers = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      "Content-Type": "application/json"
    });
  }

  public getById(id: number): Observable<GenericResult<Documento>> {

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/' + id, { headers: this.headers })
      .map(res => res.json());
  }

  public inserir(nf: Documento): Observable<GenericResult<Documento>> {

    //nf.usuarioInclusao = this.authProfile.usuarioProfile.currentUsuario;
   // nf.fornecedor = this.authProfile.usuarioProfile.currentUsuario.fornecedor;
    //console.log(nf);
    return this.http.post(this.commonService.getBaseUrl() + '/notafiscal/Incluir', JSON.stringify(nf), {
      headers: this.headers
    }).map(res => res.json());
  }

  public aprovarDocumento(nf: Documento): Observable<GenericSimpleResult> {
    return this.http.put(this.commonService.getBaseUrl() + '/arquivo/aprovarDocumento', JSON.stringify(nf), { headers: this.headers })
      .map(res => res.json());
  }

  public validarNF(nf: Documento): Observable<GenericSimpleResult> {

    return this.http.put(this.commonService.getBaseUrl() + '/notafiscal/validarNF', JSON.stringify(nf), { headers: this.headers })
      .map(res => res.json());
  }

  public rejeitarDocumento(nf: Documento): Observable<GenericSimpleResult> {
    return this.http.put(this.commonService.getBaseUrl() + '/arquivo/rejeitarDocumento', JSON.stringify(nf), { headers: this.headers })
      .map(res => res.json());
  }

  public put(id: number, nf: Documento): Observable<GenericSimpleResult> {

    return this.http.put(this.commonService.getBaseUrl() + '/notafiscal/' + id, JSON.stringify(nf), { headers: this.headers })
      .map(res => res.json());

  }


  public delete(id: number): Observable<GenericSimpleResult> {
    return this.http.delete(this.commonService.getBaseUrl() + '/notafiscal/' + id, { headers: this.headers })
      .map(res => res.json());
  }

  public getAll(): Observable<GenericResult<Documento[]>> {

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/', { headers: this.headers })
      .map(res => res.json());

  }

  public getAllByFilter(filter: string): Observable<GenericResult<Documento[]>> {

    let loggedclienteId = this.authProfile.usuarioProfile.currentUsuario.id.toString();

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/GetAllByFilter?LoggedclienteId=' + loggedclienteId + '&filter=' + filter, { headers: this.headers })
      .map(res => res.json());

  }

  public GetAllByFilter(fornecedor: string, Cnpj: string, cliente: string, situacao: string): Observable<GenericResult<any>> {

    return this.http.get(this.commonService.getBaseUrl() + `/arquivo/GetAllByFilter?fornecedor=${fornecedor}&cnpj=${Cnpj}&cliente=${cliente}&situacao=${situacao}`, { headers: this.headers })
      .map(res => res.json());
  }

  public GetAllByOficina(oficinaId: number, codigo: string): Observable<GenericResult<any>> {

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/GetAllByOficina?oficina=' + oficinaId.toString() + '&codigo=' + codigo, { headers: this.headers })
      .map(res => res.json());
  }

  public GetRelatorio(dataInicialDia: number, dataInicialMes: number, dataInicialAno: number, dataFinalDia: number, dataFinalMes: number, dataFinalAno: number, tipoRelatorio: number): any {

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/GetRelatorio?dataInicialDia=' + dataInicialDia + '&dataInicialMes=' + dataInicialMes + '&dataInicialAno=' + dataInicialAno 
    + '&dataFinalDia=' + dataFinalDia + '&dataFinalMes=' + dataFinalMes + '&dataFinalAno=' + dataFinalAno + '&tipoRelatorio=' + tipoRelatorio, { headers: this.headers })
      ;
  }

  public getListSelecionadasByNotaDebito(notadebitoId: number | null): Observable<GenericResult<Documento[]>> {

    return this.http.get(this.commonService.getBaseUrl() + '/notafiscal/ListSelecionadasByNotaDebito?notadebito=' + notadebitoId, { headers: this.headers })
      .map(res => res.json());
  }


}
