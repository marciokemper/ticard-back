import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { URLSearchParams } from '@angular/http';
import { EmpresaFornecedor } from '../../models/empresaFornecedor';
import { Fornecedor } from '../../models/fornecedor';


@Injectable()
export class EmpresaFornecedoresService {
  private headers: Headers;
  private headersx: Headers;
  constructor(private authProfile: UsuarioProfile, private commonService: CommonService,private http: Http) {
    this.headers = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
        "Content-Type": "application/x-www-form-urlencoded"
    });

    this.headersx = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    });
  }

  public getById(id: number): Observable<GenericResult<EmpresaFornecedor>> {
    return this.http.get(this.commonService.getBaseUrl() + '/fornecedorcliente/' + id, { headers: this.headers })
      .map(res => res.json());
  }


  public inserir(empresa: number, fornecedor:number): Observable<GenericResult<any>> {
    return this.http.post(this.commonService.getBaseUrl() + `/empresafornecedor/Incluir?empresa=${empresa}&fornecedor=${fornecedor}`,null, {
      headers: this.headers
    }).map(res => res.json());

   
  }

  public delete(empresa: number, fornecedor: number): Observable<GenericSimpleResult> {
    return this.http.delete(this.commonService.getBaseUrl() + `/empresafornecedor/Deletar?empresa=${empresa}&fornecedor=${fornecedor}`, { headers: this.headers })
      .map(res => res.json());
  }



  public inserirLista(fornecedorClienteDadosAcesso: any[]): Observable<GenericResult<any>> {

        



      //var _fornecedorDadosAcessoViews: fornecedorDadosAcessoView[] = [];
      //let _fornecedorDadosAcessoView: fornecedorDadosAcessoView;

      //fornecedorClienteDadosAcesso.forEach(function (value) {
      //  _fornecedorDadosAcessoView = new fornecedorDadosAcessoView();
      //  _fornecedorDadosAcessoView.FornecedorId = value.fkFornecedorClienteId.fkFornecedorId.fornecedorId;
      //  _fornecedorDadosAcessoView.FornecedorClienteId = value.fkFornecedorClienteId.fornecedorClienteId;
      //  _fornecedorDadosAcessoView.ClienteId = value.fkFornecedorClienteId.fkClienteId.clienteId;
      //  _fornecedorDadosAcessoView.Valor = value.fornecedorClienteDadosAcessoValor;
      //  _fornecedorDadosAcessoView.Obrigatorio = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoObrigatorio;
      //  _fornecedorDadosAcessoView.DadosAcessoDescricao = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoDescricao;
      //  _fornecedorDadosAcessoView.DiaVencimento = value.fornecedorClienteDadosAcessoDiaVencimento;
      //  _fornecedorDadosAcessoView.Ativo = value.fkFornecedorClienteId.ativo;
      //  _fornecedorDadosAcessoView.da = value.fkFornecedorClienteId.debitoAutomatico;
      //  _fornecedorDadosAcessoView.ValorAproximado = 0;
      //  _fornecedorDadosAcessoView.DadosAcessoId = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId;
      //  _fornecedorDadosAcessoView.DiasAntes = value.fornecedorClienteDadosAcessoDiasAntes;

      //  _fornecedorDadosAcessoViews.push(_fornecedorDadosAcessoView);
      //});

      //let model = JSON.stringify(_fornecedorDadosAcessoViews);

      //return this.http.post(this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/IncluirLista?model=' + model, null, {
      //  headers: this.headers
      //}).map(res => res.json());

      return null;




  }

  public updateLista(fornecedorClienteDadosAcesso: any[]): Observable<GenericSimpleResult> {

    //var _fornecedorDadosAcessoViews: fornecedorDadosAcessoView[] = [];
    //let _fornecedorDadosAcessoView: fornecedorDadosAcessoView;

    //fornecedorClienteDadosAcesso.forEach(function (value) {
    //  _fornecedorDadosAcessoView = new fornecedorDadosAcessoView();
    //  _fornecedorDadosAcessoView.FornecedorId = value.fkFornecedorClienteId.fkFornecedorId.fornecedorId;
    //  _fornecedorDadosAcessoView.FornecedorClienteId = value.fkFornecedorClienteId.fornecedorClienteId;
    //  _fornecedorDadosAcessoView.ClienteId = value.fkFornecedorClienteId.fkClienteId.clienteId;
    //  _fornecedorDadosAcessoView.Valor = value.fornecedorClienteDadosAcessoValor;
    //  _fornecedorDadosAcessoView.Obrigatorio = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoObrigatorio;
    //  _fornecedorDadosAcessoView.DadosAcessoDescricao = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoDescricao;
    //  _fornecedorDadosAcessoView.DiaVencimento = value.fornecedorClienteDadosAcessoDiaVencimento;
    //  _fornecedorDadosAcessoView.Ativo = value.fkFornecedorClienteId.ativo;
    //  _fornecedorDadosAcessoView.da = value.fkFornecedorClienteId.debitoAutomatico;
    //  _fornecedorDadosAcessoView.ValorAproximado = 0;
    //  _fornecedorDadosAcessoView.DadosAcessoId = value.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId;
    //  _fornecedorDadosAcessoView.DiasAntes =  value.fornecedorClienteDadosAcessoDiasAntes;

    //  _fornecedorDadosAcessoViews.push(_fornecedorDadosAcessoView);
    //});

    //return this.http.put(this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/UpdateLista?model=' + JSON.stringify(_fornecedorDadosAcessoViews),null, {
    //  headers: this.headersx
    //}).map(res => res.json());
    
    return null;
    }

  public executarRobo(fornecedorClienteId: number): Observable<GenericSimpleResult> {

        return this.http.get(this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/getavencerbyid?id=' + fornecedorClienteId, { headers: this.headers })
            .map(res => res.json());

    }

  public TransferirUnidadeNegocio(emissor: number, fornecedorClienteId: number): Observable<GenericSimpleResult> {
        return this.http.put(this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/TransferirUnidadeNegocio?emissor=' + emissor + '&fornecedorClienteId=' + fornecedorClienteId, null, {
            headers: this.headers
        }).map(res => res.json());


    }

  public getByClienteAndFornecedorId(clienteId: number, fornecedorid: number): Observable<GenericResult<EmpresaFornecedor>> {

    return this.http.get(this.commonService.getBaseUrl() + '/fornecedorcliente/getByClienteAndFornecedorId?clienteId=' + clienteId.toString() + '&fornecedorid=' + fornecedorid.toString(), { headers: this.headers })
      .map(res => res.json());

  }

  public deleteVinculacaoCliente(id: number): Observable<GenericSimpleResult> {
    return this.http.delete(this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/' + id, { headers: this.headers })
      .map(res => res.json());
  }

  public getAll(): Observable<GenericResult<EmpresaFornecedor[]>> {
    return this.http.get(this.commonService.getBaseUrl() + '/fornecedorcliente/GetAll/', { headers: this.headers })
      .map(res => res.json());

  }

  public getFornecedorClienteByCliente(clienteId: number): Observable<GenericResult<EmpresaFornecedor[]>> {
    let myParams = new URLSearchParams();

    myParams.append('clienteId', clienteId.toString());
    let options = new RequestOptions({ headers: this.headers, params: myParams });

    return this.http.get(this.commonService.getBaseUrl() + '/fornecedorcliente/getfornecedorclientebycliente/', options)
      .map(res => res.json());

  }

  public put(id: number, fornecedorCliente: EmpresaFornecedor): Observable<GenericSimpleResult> {

    return this.http.put(this.commonService.getBaseUrl() + '/fornecedorcliente/' + id, JSON.stringify(fornecedorCliente), { headers: this.headers })
      .map(res => res.json());
  }

}
