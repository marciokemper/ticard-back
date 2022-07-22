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
declare var jQuery: any;
declare var $: any;


@Component({
  selector: 'app-empresa-vincula',
  templateUrl: './empresavincula.component.html',
  styleUrls: ['./empresavincula.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class EmpresaVinculaComponent implements OnInit {
  @ViewChild('selectx') public ngSelect: SelectComponent;
  @ViewChild('selectx3') public ngSelect3: SelectComponent;
  @ViewChild('selectx2') public ngSelect2: SelectComponent;
  @ViewChild('selectEmissor') public ngSelectEmissor: SelectComponent;
  @ViewChild('selectBatchEmissor') public ngSelectBatchEmissor: SelectComponent;

  vinculacoes: Empresa[];
  //public clientes: Cliente[] = [];
  //vinculacoesDadosAcesso: FornecedorClienteDadosAcesso[];
  //public lstDadosAcesso: DadosAcesso[] = [];
  public fornecedores: Fornecedor[] = [];
  public fornecedoresRobo: Fornecedor[] = [];
  dtOptions: DataTables.Settings = {};
  dtVinculaOptions: DataTables.Settings = {};
  public inEditMode: boolean = false;
  public selectedFornecedor: string = '';
  public selectedEmissor: string = '';
  public lote: string = '';
  public selectedBatchEmissor: string = '';
  public diaVencimento: number = 0;
  public diaAntesVencimento: number = 0;
  public currentVinculacao: Empresa;
  //public fornecedorDadosAcesso: FornecedorDadosAcesso;
  //public fornecedorClienteDadosAcesso: FornecedorClienteDadosAcesso;
  public buttonLabel: string = 'Create';
  private id: number;
  public isdiavencimento: boolean = true;
  public lblvalor: string = 'Valor';
  public progress: number;
  public lblvalorPlaceholder: string = '';
  public isdiasantesvencimento: boolean = true;
  public isColetar: boolean = false;
  public isParsear: boolean = false;
  public message: string;
  public controlHidden: boolean = true;
  public dadosAcessoUrlCriacao: string = '';
  public fornecedorClienteId: number = 0;
  emissores = [];

  emissoresRobo = [];
  unidades = [];
  public isEdit: boolean = false;
  public isManualmente: boolean = false;
  public isManualEmissor: boolean = false;
  public currentFornecedor: Fornecedor;
 // public tiposFornecedor: TipoFornecedor[] = [];
  classificacoes: Array<IOption>;
  _classificacao = [];
  selectedCharacter: string = '3';
  situacao: string;
  mesReferencia: number;
  anoReferencia: number;
  dtVencimento: Date;
  dtEmissao: Date;
  documento: string;
  totalFormated: string;
  codigoBarras: string;
  fornecedorCliente_id: number;
  classificacao: string = '';

  //public clasificacao: centrocusto[] = [];
  selectedClassificao: string = '';
  constructor(private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    private authProfile: UsuarioProfile,
    private commonService: CommonService,
    private service: EmpresaService,
    //private dadosAcessoService: DadosAcessoService,
    //private serviceArquivoColetado: ContaColetadaService,
    //private serviceVincula: ClientevinculaService,
    private cepService: CepService,
    /*private clienteService: ClienteService,*/
    //private classificacaoService: CentrocustoService,
    private coolDialogs: NgxCoolDialogsService,
    //private fornecedorDadosAcessoService: FornecedorDadosAcessoService,
    private loadingService: LoadingService,
    private utilsService: UtilsService,
    //private dateService: DateService
  ) {
    this.currentVinculacao = null;
    //this.currentFornecedor = null;

    this.getClientes();
  }

  public getClientes(): void {


    //this.clientes = [];
    //this.clienteService.getAllByLoggedcliente()
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.clientes = res.result;
    //      for (var _i = 0; _i < this.clientes.length; _i++) {
    //        this.unidades.push({ label: this.clientes[_i].clienteNome, value: this.clientes[_i].clienteId });
    //      }
    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
  }

  public async UnidadeClassificacaofilterInputChanged(value): Promise<void> {
    this.getClassificacoes(value);
  }

  public getClassificacao(filter) {
    this.classificacao = filter.value;
  }

  public getClassificacoes(filter) {
    //this.clasificacao = [];

    //this.classificacaoService.getAllByFilter(filter)
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.clasificacao = res.result;

    //      this.classificacoes = this.clasificacao.map((obj: centrocusto) => ({
    //        value: obj.centroCustoId.toString(),
    //        label: obj.centroCustoNome
    //      }));

    //      this.classificacoes = this.cloneOptions(this.classificacoes);

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });

  }

  public getTiposFornecedor(): void {
    //this.tiposFornecedor = [];
    //this.service.getListaTiposFornecedor()
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.tiposFornecedor = res.result;
    //    }
    //  });
  }

  public remove(id: number): void {
    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Todos os vínculos serão removidos, continuar?', {
      theme: 'default',
      okButtonText: 'Excluir',
      cancelButtonText: 'Sair',
      color: '#3a3d42',
      title: 'Atenção'
    });

    dialog.subscribe(res => {
      if (res == true) {

        this.service.delete(id)
          .subscribe((res) => {
            if (res.success) {
              this.utilsService.MessageSucessoShow("Filial deletada com sucesso")
              jQuery("#grdListaVincula").DataTable().ajax.reload();
            }
            else {
              this.utilsService.MessageWarningShow("Filial não pode ser excluído")
              console.error(res.errors);
            }
          });
      }
    });

  }

  public exitpopup(): void {

    this.isColetar = false;
    this.isParsear = false;
    jQuery("#grdListaVincula").DataTable().ajax.reload();

  }

  public requisicaoEmissor(): void {
    this.router.navigate(['requisicao']);
  }

  public removeVinculoDadosAcesso(id: number): void {

    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Deseja remover essa vinculação?', {
      theme: 'default',
      okButtonText: 'Excluir',
      cancelButtonText: 'Sair',
      color: '#3a3d42',
      title: 'Atenção'
    });

    dialog.subscribe(async res => {
      //if (res == true) {

      //  this.loadingService.Show();
      //  await this.serviceVincula.deleteVinculacaoCliente(id)
      //    .subscribe((res) => {
      //      if (res.success) {
      //        this.dadosAcessoService.getByClienteFornecedor(this.id, Number(this.selectedFornecedor), this.fornecedorClienteId)
      //          .subscribe((res) => {
      //            if (res.success) {
      //              this.lstDadosAcesso = res.result;
      //            } else {
      //              console.error(res.errors);
      //            }
      //          });

      //        this.utilsService.MessageSucessoShow("Vínculo deletado com sucesso")
      //        jQuery("#grdListaVinculaDados").DataTable().ajax.reload();


      //      }
      //      else {
      //        this.utilsService.MessageWarningShow("Vínculo não pode ser excluído")
      //        console.error(res.errors);
      //      }
      //    });

      //  this.loadingService.Hide();
      //}
    });

  }

  public async getSelectedEmissor(value): Promise<void> {
    this.selectedEmissor = value.value;
  }

  public async getSelectedBatchEmissor(value): Promise<void> {
    this.selectedBatchEmissor = value.value;
    this.lote = ''
    this.loadingService.Show();

    //await this.dadosAcessoService.GetByDataAcessoId(value.value)
    //  .subscribe(async (res) => {
    //    if (res.success) {
    //      this.lote = res.result[0].line;
    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
    //this.loadingService.Hide();

  }

  public async getDadosAcesso(value): Promise<void> {
    //this.selectedFornecedor = value.value;
    //this.loadingService.Show();
    //this.dadosAcessoUrlCriacao = '';
    //this.controlHidden = true;
    //jQuery("#grdListaVinculaDados").DataTable().ajax.reload();
    //this.lstDadosAcesso = [];
    //await this.dadosAcessoService.getByClienteFornecedor(this.id, value.value, this.fornecedorClienteId)
    //  .subscribe(async (res) => {
    //    if (res.success) {
    //      this.lstDadosAcesso = res.result;
    //      if (this.lstDadosAcesso.length > 0) {
    //        this.dadosAcessoUrlCriacao = this.lstDadosAcesso[0].dadosAcessoUrlCriacao;
    //        this.controlHidden = this.dadosAcessoUrlCriacao == null ? true : false;
    //      }
    //      else
    //        this.controlHidden = true;

    //      await this.serviceVincula.getByClienteAndFornecedorId(this.id, Number(this.selectedFornecedor))
    //        .subscribe((res2) => {
    //          if (res2.success) {

    //            this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = res2.result == null ? false : res2.result.coletar;
    //            this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = res2.result == null ? false : res2.result.parsear;
    //          } else {
    //            console.error(res2.errors);
    //          }
    //        });

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
    //this.loadingService.Hide();

  }

  public async getEmissorId(value): Promise<void> {
    //this.selectedFornecedor = value.value;

    //await this.serviceVincula.getByClienteAndFornecedorId(this.id, Number(this.selectedFornecedor))
    //  .subscribe((res2) => {
    //    if (res2.success) {
    //      this.currentVinculacao.fornecedorClienteId = res2.result == null ? 0 : res2.result.fornecedorClienteId;
    //      this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fornecedorClienteId = res2.result == null ? 0 : this.currentVinculacao.fornecedorClienteId;
    //      this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = res2.result == null ? false : res2.result.coletar;
    //      this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = res2.result == null ? false : res2.result.parsear;
    //    } else {
    //      console.error(res2.errors);
    //    }
    //  });

    //this.currentVinculacao.fkFornecedorId = new Fornecedor();
    //this.currentVinculacao.fkFornecedorId.fornecedorId = Number(this.selectedFornecedor);

    //this.currentVinculacao.fkClienteId = new Cliente();
    //this.currentVinculacao.fkClienteId.clienteId = Number(this.id);

    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = Number(this.selectedFornecedor);

    //this.currentVinculacao.fkFornecedorId.fkTipoFornecedorId = new TipoFornecedor();

  }

  public async SetDadosAcesso(value): Promise<void> {
    //this.selectedFornecedor = value;
    //this.loadingService.Show();
    //this.dadosAcessoUrlCriacao = '';
    //this.controlHidden = true;
    //jQuery("#grdListaVinculaDados").DataTable().ajax.reload();
    //this.lstDadosAcesso = [];
    //await this.dadosAcessoService.getByClienteFornecedor(this.id, value, this.fornecedorClienteId)
    //  .subscribe(async (res) => {
    //    if (res.success) {
    //      this.lstDadosAcesso = res.result;
    //      this.diaVencimento = this.lstDadosAcesso[0].diaVencimento;
    //      this.diaAntesVencimento = this.lstDadosAcesso[0].diaAntesVencimento;

    //      if (this.lstDadosAcesso.length > 0) {
    //        this.dadosAcessoUrlCriacao = this.lstDadosAcesso[0].dadosAcessoUrlCriacao;
    //        this.controlHidden = this.dadosAcessoUrlCriacao == null ? true : false;
    //      }
    //      else
    //        this.controlHidden = true;

    //      await this.serviceVincula.getByClienteAndFornecedorId(this.id, Number(this.selectedFornecedor))
    //        .subscribe((res2) => {
    //          if (res2.success) {

    //            this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = res2.result == null ? false : res2.result.coletar;
    //            this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = res2.result == null ? false : res2.result.parsear;
    //          } else {
    //            console.error(res2.errors);
    //          }
    //        });

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
    //this.loadingService.Hide();

  }

  private cloneOptions(options: Array<IOption>): Array<IOption> {
    return options.map(option => ({
      value: option.value,
      label: option.label
    }));
  }

  getCharacters(value2): Array<IOption> {
    return this.cloneOptions(value2);
  }


  public async SetCentroCusto(value: string): Promise<void> {
    //this._classificacao = [];
    //await this.classificacaoService.getByFilter(value)
    //  .subscribe(async (res) => {
    //    if (res.success) {
    //      this.clasificacao = res.result;

    //      this.classificacoes = this.clasificacao.map((obj: centrocusto) => ({
    //        value: obj.centroCustoId.toString(),
    //        label: obj.centroCustoNome
    //      }));

    //      this.classificacoes = this.cloneOptions(this.classificacoes);

    //      this.ngSelect3.select(value.toString());


    //    } else {
    //      console.error(res.errors);
    //    }
    //  });

  }

  public async getTipoDadoAcesso(value): Promise<void> {
    //this.isdiavencimento = true;
    //this.lblvalor = 'Valor';
    //this.lblvalorPlaceholder = '';
    //this.isdiasantesvencimento = true;
    //if (value == '')
    //  return;
    //this.loadingService.Show();
    //this.fornecedorDadosAcesso = new FornecedorDadosAcesso();


    //await this.fornecedorDadosAcessoService.getByFornecedorAndDataAcessoId(Number(this.selectedFornecedor), value)
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.isdiavencimento = res.result.fornecedorDadosAcessoTipoDado == '1' ? true : false;
    //      this.lblvalor = res.result.fornecedorDadosAcessoDadosAcessoId.dadosAcessoNome;
    //      this.lblvalorPlaceholder = res.result.fornecedorDadosAcessoDadosAcessoId.dadosAcessoDescricao
    //      this.isdiasantesvencimento = res.result.fornecedorDadosAcessoTipoDado == '1' ? true : false;
    //      this.fornecedorDadosAcesso = res.result;

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
    //this.loadingService.Hide();

  }

  public edit(vinculacao: Empresa): void {
    //var date = this.dateService.convertTZ(new Date(), "America/Sao_Paulo");
    //this.isEdit = true;
    //this.classificacao = '';
    //this.currentVinculacao = vinculacao;
    //this.currentVinculacao.arquivoColetado = new ArquivoColetado();
    //this.currentVinculacao.arquivoColetado.dtInclusao = date;
    //this.selectedFornecedor = this.currentVinculacao.fkFornecedorId.fornecedorId.toString();
    //this.id = this.currentVinculacao.fkClienteId.clienteId;

    //this.isColetar = this.currentVinculacao.coletar;
    //this.isParsear = this.currentVinculacao.parsear;
    //this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = this.isColetar;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = this.isParsear;

    //this.classificacoes = null;
    //if (vinculacao.fkCentroCustoID != null && vinculacao.fkCentroCustoID.centroCustoId > 0) {
    //  this.classificacao = vinculacao.fkCentroCustoID.centroCustoId.toString();
    //  this.SetCentroCusto(this.classificacao);

    //}

    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = Number(this.selectedFornecedor);
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = '';
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = 0;
    //this.fornecedorClienteId = vinculacao.fornecedorClienteId;
    //this.SetDadosAcesso(Number(this.selectedFornecedor));


    //this.inEditMode = true;
    //this.buttonLabel = 'Salvar';
  }

  public AdicionarManualmente(vinculacao: Empresa): void {

    //if (this.ngSelectEmissor != undefined)
    //  this.ngSelectEmissor.clear();
    //var date = this.dateService.convertTZ(new Date(), "America/Sao_Paulo");
    //this.situacao = '';
    //this.mesReferencia = 0;
    //this.anoReferencia = 0;
    //this.dtVencimento = date;
    //this.dtEmissao = date;
    //this.documento = '';
    //this.totalFormated = '';
    //this.codigoBarras = '';
    //this.fornecedorCliente_id = vinculacao.fornecedorClienteId;
    //$("#filer_input2").val('');

    //this.currentVinculacao = new fornecedorCliente();
    //this.currentVinculacao.arquivoColetado = new ArquivoColetado();
    //this.currentVinculacao.arquivoColetado.status = "1";
    //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId = new fornecedorCliente();
    //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fornecedorClienteId = vinculacao.fornecedorClienteId;
    //this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = this.id.toString();
    //this.currentVinculacao.arquivoColetado.situacao = '';
    //this.currentVinculacao.fkClienteId = new Cliente();
    //this.currentVinculacao.fkClienteId.clienteId = this.id;


    //this.isColetar = true;
    //this.isParsear = false;
    //this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = this.isColetar;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = this.isParsear;

    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = '';
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = 0;

    //this.isManualmente = true;
    //this.isManualEmissor = true;



  }

  public AdicionarEmissor(): void {

    this.getTiposFornecedor();

    this.currentFornecedor = new Fornecedor();
    //this.currentFornecedor.tipoFornecedorId = "";

  }

  public TransferirCreate(vinculacao: Empresa): void {
    //if (this.ngSelect2 != undefined)
    //  this.ngSelect2.clear();

    //this.selectedEmissor = '';
    //this.currentVinculacao = vinculacao;

  }

  public executarRobo(id: number): void {
    //this.serviceVincula.executarRobo(id)
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.utilsService.MessageSucessoShow("Robô disparado com sucesso!")
    //    } else {
    //      this.utilsService.MessageWarningShow(res.errors[0])
    //      console.error(res.errors);
    //    }
    //  });

  }

  public AdicionarDocumento(vinculacao: Empresa): void {
    //this.isManualmente = false;
    //this.isManualEmissor = false;

    //if (this.ngSelectEmissor != undefined)
    //  this.ngSelectEmissor.clear();

    //this.currentVinculacao = vinculacao;
    //this.currentVinculacao.arquivoColetado = new ArquivoColetado();
    //this.currentVinculacao.arquivoColetado.status = "1";
    //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId = new fornecedorCliente();
    //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fornecedorClienteId = this.currentVinculacao.fornecedorClienteId;
    //this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = vinculacao.fkClienteId.clienteId.toString();

    //this.selectedFornecedor = this.currentVinculacao.fkFornecedorId.fornecedorId.toString();
    //this.id = this.currentVinculacao.fkClienteId.clienteId;


    //this.isColetar = this.currentVinculacao.coletar;
    //this.isParsear = this.currentVinculacao.parsear;
    //this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = this.isColetar;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = this.isParsear;

    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = Number(this.selectedFornecedor);
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = '';
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = 0;

    //this.currentVinculacao.arquivoColetado = new ArquivoColetado();

  }

  public create(): void {
    this.isEdit = false;
    //this.diaVencimento = 0;
    //this.selectedFornecedor = '';
    //if (this.ngSelect != undefined)
    //  this.ngSelect.clear();

    //if (this.ngSelect3 != undefined)
    //  this.ngSelect3.clear();

    //var date = this.dateService.convertTZ(new Date(), "America/Sao_Paulo");
    //this.diaAntesVencimento = 0;
    //this.lstDadosAcesso = null;
    //this.classificacao = '';
    this.currentVinculacao = new Empresa();
    //this.currentVinculacao.ativo = true;
    //this.currentVinculacao.debitoAutomatico = false;
    //this.currentVinculacao.documentoQuantidade = 0;
    //this.currentVinculacao.fkCentroCustoID = new centrocusto();
    //this.currentVinculacao.arquivoColetado = new ArquivoColetado();
    //this.currentVinculacao.arquivoColetado.dtInclusao = date;
    //this.currentVinculacao.fornecedorClienteId = 0;
    //this.currentVinculacao.fkFornecedorId = new Fornecedor();
    //this.currentVinculacao.fkClienteId = new Cliente();
    //this.currentVinculacao.arquivoColetado.situacao = '';
    //this.currentVinculacao.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = this.isColetar;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = this.isParsear;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = 0;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = '';
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = 0;
    //this.fornecedorClienteId = 0;


   // this.ref.nativeElement.clear();
    jQuery("#grdListaVinculaDados").DataTable().ajax.reload();
    this.inEditMode = false;
    this.buttonLabel = 'Vincular';

    $('#formVincula').parsley().reset();
    $('#formVincula').parsley().on('field:validated', function () {
      var ok = $('.parsley-error').length === 0;
      $('.alert-info').toggleClass('hidden', !ok);
      $('.alert-warning').toggleClass('hidden', ok);
    })
      .on('form:submit', function () {
        return false;
      });

  }

  public getFornecedores(): void {
    //this.fornecedores = [];
    //this.service.getAll()
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.fornecedores = res.result;
    //      for (var _i = 0; _i < this.fornecedores.length; _i++) {
    //        this.emissores.push({ label: this.fornecedores[_i].fornecedorNome, value: this.fornecedores[_i].fornecedorId });
    //      }

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
  }

  public getFornecedoresRobo(): void {
    //this.fornecedoresRobo = [];
    //this.service.getRoboAll()
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.fornecedoresRobo = res.result;
    //      for (var _i = 0; _i < this.fornecedoresRobo.length; _i++) {
    //        this.emissoresRobo.push({ label: this.fornecedoresRobo[_i].fornecedorNome, value: this.fornecedoresRobo[_i].fornecedorId });
    //      }

    //    } else {
    //      console.error(res.errors);
    //    }
    //  });
  }

  public pegaCep(fornecedor: Fornecedor): void {
    //this.loadingService.Show();
    //this.cepService.consultaCEP(fornecedor.fornecedorCep)
    //  .subscribe((res) => {
    //    let body = res.json();
    //    this.currentFornecedor.fornecedorLogradouro = body.logradouro;
    //    this.currentFornecedor.fornecedorBairro = body.bairro;
    //    this.currentFornecedor.fornecedorCidade = body.localidade;
    //    this.currentFornecedor.fornecedorUF = body.uf;
    //    this.currentFornecedor.fornecedorComplemento = '';
    //    this.currentFornecedor.fornecedorNumero = '';
    //    this.loadingService.Hide();
    //  });

  }

  public getDados(): void {

    //this.lstfornecedorClienteDadosAcesso = [];
    //for (var _i = 0; _i < this.lstDadosAcesso.length; _i++) {
    //  this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();

    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();

    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkCentroCustoID = new centrocusto();
    //  if (this.classificacao != '') {
    //    this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkCentroCustoID.centroCustoId = Number(this.classificacao);
    //  }

    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = Number(this.selectedFornecedor);
    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fornecedorClienteId = Number(this.fornecedorClienteId);
    //  if (this.currentVinculacao.valorAproximado != undefined)
    //    this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.valorAproximado = this.GetTotal(this.currentVinculacao.valorAproximado.toString());
    //  else
    //    this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.valorAproximado = null;

    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fornecedorCodigoERP = this.currentVinculacao.fornecedorCodigoERP;

    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //  this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = Number(this.id);

    //  this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //  this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //  this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoObrigatorio = this.lstDadosAcesso[_i].dadosAcessoObrigatorio;
    //  this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = this.lstDadosAcesso[_i].dadosAcessoId;
    //  this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoUrlCriacao = this.lstDadosAcesso[_i].dadosAcessoDescricao;

    //  this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = $('#valor_' + this.lstDadosAcesso[_i].dadosAcessoId).val();
    //  this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = encodeURIComponent(this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor.replace('#', '___'));

    //  this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = $('#diavencimento').val();
    //  this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = $('#diaAntesvencimento').val();

    //  this.lstfornecedorClienteDadosAcesso.push(this.fornecedorClienteDadosAcesso);

    //}
  }

  public saveEmissor(): void {

    //this.currentFornecedor.fornecedorIdentificador = "_naorobo";
    //this.currentFornecedor.fornecedorIsRobo = false;
    //if ($('#formAddEmissor').parsley().isValid()) {
    //  this.service.inserir(this.currentFornecedor)
    //    .subscribe((res) => {
    //      if (res.success) {
    //        this.utilsService.MessageSucessoShow("Inclusão de Emissor Realizada com sucesso")
    //        this.getFornecedores();
    //      }
    //      else {
    //        this.utilsService.MessageWarningShow("Erro durante a inclusão do Emissor")
    //      }
    //    });
    //}
  }

  public save(): void {

    if ($('#formVincula').parsley().isValid()) {

      if (!this.isEdit) {
        this.saveNewVincula();
      } else {
        this.updateNewVincula();
      }
    }
  }

  public TransferirEmissor(): void {

    //if (this.selectedEmissor == '')
    //  return;

    //this.loadingService.Show();
    //this.serviceVincula.TransferirUnidadeNegocio(Number(this.selectedEmissor), Number($('#fornecedorClienteId').val()))
    //  .subscribe((res) => {
    //    this.loadingService.Hide();
    //    if (res.success) {
    //      this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
    //      jQuery("#frmTrasferenciaUnidade").modal("hide");
    //      jQuery("#grdListaVincula").DataTable().ajax.reload();
    //    }
    //    else {
    //      if (res.errors.length > 0)
    //        this.utilsService.MessageWarningShow(res.errors[0])
    //      else
    //        this.utilsService.MessageWarningShow("Erro durante a alteração do Vinculação")
    //    }
    //  });


  }

  public saveNewVincula(): void {

    this.loadingService.Show();
    this.currentVinculacao.empresaPaiId = this.id;
    this.service.inserir(this.currentVinculacao)
      .subscribe((res) => {
        this.loadingService.Hide();
        if (res.success) {
          this.utilsService.MessageSucessoShow("Inclusão Filial com sucesso")
          jQuery("#frmVinculaFornecedor").modal("hide");
          jQuery("#grdListaVincula").DataTable().ajax.reload();
        }
        else {
          if (res.errors.length > 0)
            this.utilsService.MessageWarningShow(res.errors[0])
          else
            this.utilsService.MessageWarningShow("Erro durante a inclusão do Filial")
        }
      });

  }

  public updateNewVincula(): void {

    ////this.loadingService.Show();

    ////for (var _i = 0; _i < this.lstfornecedorClienteDadosAcesso.length; _i++) {
    ////  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.ativo = this.currentVinculacao.ativo;
    ////  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.debitoAutomatico = this.currentVinculacao.debitoAutomatico;
    ////  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.documentoQuantidade = this.currentVinculacao.documentoQuantidade;
    ////}

    ////this.serviceVincula.updateLista(this.lstfornecedorClienteDadosAcesso)
    ////  .subscribe((res) => {
    ////    if (res.success) {
    ////      this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
    ////      jQuery("#frmVinculaFornecedor").modal("hide");
    ////      jQuery("#grdListaVincula").DataTable().ajax.reload();
    ////    }
    ////    else {
    ////      if (res.errors.length > 0)
    ////        this.utilsService.MessageWarningShow(res.errors[0])
    ////      else
    ////        this.utilsService.MessageWarningShow("Erro durante a alteração do Vinculação")
    ////    }
    ////  });
    ////this.loadingService.Hide();
  }

  private saveNewVinculacao(): void {
    //this.getDados();
    //this.loadingService.Show();

    //for (var _i = 0; _i < this.lstfornecedorClienteDadosAcesso.length; _i++) {
    //  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.ativo = this.currentVinculacao.ativo;
    //  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.debitoAutomatico = this.currentVinculacao.debitoAutomatico;
    //  this.lstfornecedorClienteDadosAcesso[_i].fkFornecedorClienteId.documentoQuantidade = this.currentVinculacao.documentoQuantidade;
    //}

    //this.serviceVincula.inserirLista(this.lstfornecedorClienteDadosAcesso)
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
    //      jQuery("#frmVinculaFornecedor").modal("hide");
    //      jQuery("#grdListaVincula").DataTable().ajax.reload();
    //    }
    //    else {
    //      if (res.errors.length > 0)
    //        this.utilsService.MessageWarningShow(res.errors[0])
    //      else
    //        this.utilsService.MessageWarningShow("Erro durante a alteração do Vinculação")
    //    }
    //  });
    //this.loadingService.Hide();

  }

  private GetTotal(value: string): number {

    var _valor = value.replace(',', '').replace('.', '');
    _valor = _valor.substring(0, _valor.length - 2) + '.' + _valor.substring(_valor.length - 2);
    return Number(_valor);
  }

  private insertManual(files): void {


    //if (!$('#formUploadManual').parsley().isValid())
    //  return;

    ////if (this.selectedEmissor == '')
    ////  return;

    //if (files.length === 0) {
    //  return;
    //}

    //this.currentVinculacao.arquivoColetado.nomeArquivoManual = files[0].name;
    //this.currentVinculacao.arquivoColetado.situacao = this.situacao;
    //this.currentVinculacao.arquivoColetado.mesReferencia = this.mesReferencia;
    //this.currentVinculacao.arquivoColetado.anoReferencia = this.anoReferencia;
    //this.currentVinculacao.arquivoColetado.dtVencimento = this.dtVencimento;
    //this.currentVinculacao.arquivoColetado.dtInclusao = this.dtEmissao;
    //this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = this.documento



    //this.currentVinculacao.arquivoColetado.totalFormated = this.totalFormated;
    //this.currentVinculacao.arquivoColetado.codigoBarras = this.codigoBarras;
    //this.currentVinculacao.fkFornecedorId = new Fornecedor();
    //this.currentVinculacao.fkFornecedorId.fornecedorId = 1;

    //this.currentVinculacao.arquivoColetado.dtInclusao = this.currentVinculacao.arquivoColetado.dtVencimento;

    //var _date = `"${this.currentVinculacao.arquivoColetado.anoReferencia}-${this.currentVinculacao.arquivoColetado.mesReferencia}-01`;

    //this.currentVinculacao.arquivoColetado.nrMesReferencia = new Date(_date);

    //this.currentVinculacao.arquivoColetado.total = this.GetTotal(this.currentVinculacao.arquivoColetado.totalFormated);

    //this.loadingService.Show();
    //this.serviceArquivoColetado.validaInclusaoManual(this.currentVinculacao, this.currentVinculacao.arquivoColetado.dtVencimento.toString(), this.currentVinculacao.arquivoColetado.total.toString(), this.dtEmissao, this.documento)
    //  .subscribe(async (res) => {
    //    if (res.success) {

    //      const formData = new FormData();

    //      for (let file of files)
    //        formData.append(file.name, file);

    //      const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/arquivoColetado/UploadDocumentoFile', formData, {
    //        reportProgress: true,
    //      });

    //      this.http.request(uploadReq).subscribe(event => {
    //        if (event.type === HttpEventType.Response) {
    //          this.currentVinculacao.arquivoColetado.isManual = true;
    //          this.currentVinculacao.arquivoColetado.status = "1";
    //          this.currentVinculacao.arquivoColetado.dtInclusao = this.dtEmissao;
    //          this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = '';
    //          this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = this.documento
    //          this.currentVinculacao.arquivoColetado.observacao = this.documento

    //          this.currentVinculacao.arquivoColetado.caminhoArquivo = event.body['fullPath'];
    //          this.currentVinculacao.arquivoColetado.fkFornecedorClienteId = new fornecedorCliente();
    //          this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fornecedorClienteId = this.fornecedorCliente_id;
    //          this.currentVinculacao.arquivoColetado.codigoIdentificacaoCliente = this.currentVinculacao.fkClienteId.clienteId.toString();
    //          this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //          //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkFornecedorId.fornecedorId = this.currentVinculacao.fkFornecedorId.fornecedorId;
    //          //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkFornecedorId.fornecedorNome = this.currentVinculacao.fkFornecedorId.fornecedorNome;
    //          this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkFornecedorId.fkTipoFornecedorId = new TipoFornecedor();
    //          this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkClienteId = new Cliente();
    //          //this.currentVinculacao.arquivoColetado.fkFornecedorClienteId.fkClienteId.clienteId = this.currentVinculacao.fkClienteId.clienteId;
    //          if ($('#formUploadManual').parsley().isValid()) {
    //            this.serviceArquivoColetado.inserir(this.currentVinculacao.arquivoColetado)
    //              .subscribe((res) => {
    //                this.loadingService.Hide();
    //                this.utilsService.MessageSucessoShow("Documento adicionado com sucesso!")
    //                jQuery("#frmUploadManual").modal("hide");
    //              });
    //          } else
    //            this.loadingService.Hide();
    //        }
    //      });



    //    } else {
    //      console.error(res.errors);
    //      this.utilsService.MessageWarningShow(res.errors[0].toString())
    //      this.loadingService.Hide();
    //    }
    //  });


  }

  private insertBatch(files): void {

    //if (files.length === 0) {
    //  return;
    //}

    //this.loadingService.Show();

    //const formData = new FormData();

    //for (let file of files)
    //  formData.append(file.name, file);

    //const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/fornecedorclientedadosacesso/UploadBatchFile', formData, {
    //  reportProgress: true,
    //});

    //this.http.request(uploadReq).subscribe(event => {
    //  //if (event.type === HttpEventType.Response) {
    //  //  this.currentVinculacao.arquivoColetado.caminhoArquivo = event.body['fullPath'];
    //  //  if ($('#formbatch').parsley().isValid()) {
    //  //    this.fornecedorDadosAcessoService.inserirBatch(event.body['fullPath'])
    //  //      .subscribe((res) => {
    //  //        this.loadingService.Hide();
    //  //        this.utilsService.MessageSucessoShow("Documento adicionado com sucesso!")
    //  //        jQuery("#frmbatch").modal("hide");
    //  //      });
    //  //  } else
    //  //    this.loadingService.Hide();
    //  //}
    //});


  }

  public ngOnInit() {
    const that = this;

    this.route.params.subscribe((params) => {
      this.id = params['id'];
    });

    //this.getFornecedores();
    //this.getFornecedoresRobo();



    //this.fornecedorClienteDadosAcesso = new FornecedorClienteDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId = new fornecedorCliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.coletar = false;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.parsear = false;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId = new Fornecedor();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkFornecedorId.fornecedorId = 0;
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId = new Cliente();
    //this.fornecedorClienteDadosAcesso.fkFornecedorClienteId.fkClienteId.clienteId = this.id;
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId = new FornecedorDadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId = new DadosAcesso();
    //this.fornecedorClienteDadosAcesso.fkFornecedorDadosAcessoId.fornecedorDadosAcessoDadosAcessoId.dadosAcessoId = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoValor = '';
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiaVencimento = 0;
    //this.fornecedorClienteDadosAcesso.fornecedorClienteDadosAcessoDiasAntes = 0;



    this.dtVinculaOptions = {
      pagingType: 'numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: false,
      responsive: true,
      ordering: false,
      paging: false,
      language: {
        processing: "",
        lengthMenu: "Mostrar  _MENU_  Registros",
        zeroRecords: "",
        info: "Mostrando de _START_ ate _END_ de _TOTAL_ registros",
        infoEmpty: "Mostrando de 0 ate 0 de 0 registros",
        infoFiltered: "",
        infoPostFix: "",
        search: "Pesquisar:",
        url: "",
        paginate: {
          first: "Primeiro",
          previous: "Anterior",
          next: "Proximo",
          last: "Ultimo"
        }
      },
      ajax: (dataTablesParameters: any, callback) => {
        this.loadingService.Show()

        //let params = new HttpParams().set('fornecedorClienteId', this.fornecedorClienteId.toString())

        //that.http
        //  .post<DataTablesResponse>(
        //    this.commonService.getBaseUrl() + '/fornecedorclienteDadosacesso/',
        //    dataTablesParameters, {
        //    params,
        //    headers: new HttpHeaders({
        //      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
        //      "Content-Type": "application/json"
        //    })
        //  }
        //  ).subscribe(resp => {
        //    //that.vinculacoesDadosAcesso = resp.data;
        //    this.loadingService.Hide()
        //    callback({
        //      recordsTotal: resp.recordsTotal,
        //      recordsFiltered: resp.recordsFiltered,
        //      data: []
        //    });
        //  });
      },
      columnDefs:
        [
          {
            targets: 0,
            visible: true,
            orderable: false
          },
          {
            targets: 1,
            visible: true,
            orderable: false
          },
          {
            targets: 2,
            visible: true,
            orderable: false
          },
          {
            targets: 3,
            visible: true,
            orderable: false
          },
          {
            data: null,
            targets: -1,
            orderable: false
          }
        ],
      columns: [
        { width: '40%' },
        { width: '15%' },
        { width: '18%' },
        { width: '15%' },
        { width: '12%' }
      ]
    };

    $('#filer_input2').filer({
      limit: 3,
      maxSize: 3,
      extensions: ['pdf', 'html', 'txt'],
      changeInput: true,
      showThumbs: true,
      addMore: true
    });


    this.dtOptions = {
      pagingType: 'numbers',
      pageLength: 10,
      serverSide: true,
      processing: true,
      searching: true,
      responsive: true,
      ordering: true,
      paging: true,
      language: {
        processing: "",
        lengthMenu: "Mostrar  _MENU_  Registros",
        zeroRecords: "",
        info: "Mostrando de _START_ ate _END_ de _TOTAL_ registros",
        infoEmpty: "Mostrando de 0 ate 0 de 0 registros",
        infoFiltered: "",
        infoPostFix: "",
        search: "Pesquisar:",
        url: "",
        paginate: {
          first: "Primeiro",
          previous: "Anterior",
          next: "Proximo",
          last: "Ultimo"
        }
      },
      ajax: (dataTablesParameters: any, callback) => {
        this.loadingService.Show()

        let params = new HttpParams().set('id', this.id.toString())

        that.http
          .post<DataTablesResponse>(
            this.commonService.getBaseUrl() + '/empresa/GetFiliais/',
            dataTablesParameters, {
            params,
            headers: new HttpHeaders({
              "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
              "Content-Type": "application/json"
            })
          }
          ).subscribe(resp => {
            that.vinculacoes = resp.data;
            this.loadingService.Hide()
            callback({
              recordsTotal: resp.recordsTotal,
              recordsFiltered: resp.recordsFiltered,
              data: []
            });
          });
      },
      columnDefs:
        [
          {
            targets: 0,
            visible: true
          },
          {
            targets: 1,
            visible: true
          },
          {
            targets: 2,
            visible: true
          },
          {
            data: null,
            targets: -1,
            orderable: false
          }
        ],
      columns: [
        { width: '7%' },
        { width: '73%' },
        { width: '10%' },
        { data: null, width: '17%' }
      ]
    };
  }
}


