import { HttpClient, HttpHeaders, HttpParams, HttpEventType, HttpResponse, HttpRequest } from '@angular/common/http';
import { AfterViewInit, OnInit, Component, ElementRef, ViewChildren, QueryList, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataTablesResponse } from '../../models/DataTablesResponse';
import { Fornecedor } from '../../models/fornecedor';
import { FornecedorService } from '../fornecedor/fornecedor.service';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
import { NgxCoolDialogResult, NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { IOption, SelectComponent } from 'ng-select';
import { CepService } from '../shared/cep.service';
import { Empresa } from '../../models/empresa';
import { EmpresaService } from '../empresa/empresa.service';
import { EmpresaFornecedoresService } from './empresafornecedores.service';
declare var jQuery: any;
declare var $: any;


@Component({
  selector: 'app-empresafornecedores-vincula',
  templateUrl: './empresafornecedores.component.html',
  styleUrls: ['./empresafornecedores.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class EmpresaFornecedoresComponent implements OnInit {
  private id: number;
  public adquirentes: Fornecedor[] = [];
  public adquirentesSelecionados: Fornecedor[] = [];
  public buttonLabel: string = 'Salvar';

  constructor(private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private authProfile: UsuarioProfile,
    private commonService: CommonService,
    private fornecedorService: FornecedorService,
    private empresaFornecedorService: EmpresaFornecedoresService,
    private coolDialogs: NgxCoolDialogsService,
    private loadingService: LoadingService,
    private utilsService: UtilsService
  ) {
    //this.currentVinculacao = null;
    //this.currentFornecedor = null;

  }

  public getAdquirentes(empresaId: number): void {
    this.adquirentes = [];
    this.fornecedorService.getListNaoSelecionadasByEmpresa(empresaId)
      .subscribe((res) => {
        if (res.success) {
          this.adquirentes = res.result;
        } else {
          console.error(res.errors);
        }
      });
  }

  public getAdquirentesSelecionados(empresaId: number): void {
    this.adquirentesSelecionados = [];
    this.fornecedorService.getListSelecionadasByEmpresa(empresaId)
      .subscribe((res) => {
        if (res.success) {
          this.adquirentesSelecionados = res.result;
        } else {
          console.error(res.errors);
        }
      });
  }

  public setbtnRight(): boolean {
    var selectedOpts = $('#adquirentes option:selected');

    for (let index = 0; index < selectedOpts.length; index++) {
      var forneecdorId = Number(selectedOpts[index].value);
      this.adquirentesSelecionados.push(this.adquirentes.find(x => x.id === forneecdorId));
      this.adquirentes = this.adquirentes.filter(h => h.id !== forneecdorId);

      this.empresaFornecedorService.inserir(this.id, forneecdorId)
        .subscribe((res) => {
          if (res.success) {
            this.utilsService.MessageSucessoShow("Vinculação Realizada com sucesso")
            this.router.navigate(['cliente']);
          }
          else {
            this.utilsService.MessageWarningShow("Erro durante a vinculação do cliente")
          }
        });
    };



    return false;

  }

  private setbtnAllRight(): boolean {
    var selectedOpts = $('#adquirentes option');

    for (let index = 0; index < selectedOpts.length; index++) {
      var id = Number(selectedOpts[index].value);
      this.adquirentesSelecionados.push(this.adquirentes.find(x => x.id === id));
      this.adquirentes = this.adquirentes.filter(h => h.id !== id);

      this.empresaFornecedorService.delete(this.id, id)
        .subscribe((res) => {
          if (res.success) {
            this.utilsService.MessageSucessoShow("Remoção da Vinculação Realizada com sucesso")
            this.router.navigate(['cliente']);
          }
          else {
            this.utilsService.MessageWarningShow("Erro durante a remoção vinculação do cliente")
          }
        });

    };

    return false;

  }

  private setbtnAllLeft(): boolean {

    var selectedOpts = $('#adquirentesSelecionados option');

    for (let index = 0; index < selectedOpts.length; index++) {
      var id = Number(selectedOpts[index].value);
      this.adquirentes.push(this.adquirentesSelecionados.find(x => x.id === id));
      this.adquirentesSelecionados = this.adquirentesSelecionados.filter(h => h.id !== id);
    };

    return false;

  }

  public setbtnLeft(): boolean {
    var selectedOpts = $('#adquirentesSelecionados option:selected');

    for (let index = 0; index < selectedOpts.length; index++) {
      var id = Number(selectedOpts[index].value);
      this.adquirentes.push(this.adquirentesSelecionados.find(x => x.id === id));
      this.adquirentesSelecionados = this.adquirentesSelecionados.filter(h => h.id !== id);

      this.empresaFornecedorService.delete(this.id, id)
        .subscribe((res) => {
          if (res.success) {
            this.utilsService.MessageSucessoShow("Remoção da Vinculação Realizada com sucesso")
            this.router.navigate(['cliente']);
          }
          else {
            this.utilsService.MessageWarningShow("Erro durante a remoção vinculação do cliente")
          }
        });

    };
    return false;

  }

  public ngOnInit() {
    const that = this;

    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });

    this.getAdquirentes(this.id);
    this.getAdquirentesSelecionados(this.id);

  }
}


