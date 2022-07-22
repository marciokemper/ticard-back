import { HttpClient, HttpHeaders, HttpParams, HttpRequest, HttpEventType } from '@angular/common/http';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgxCoolDialogResult, NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { DataTablesResponse } from '../../models/DataTablesResponse';
import { DialogConfirmService } from '../common/dialogconfirm.service';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { Fornecedor } from '../../models/fornecedor';
import { FornecedorService } from '../fornecedor/fornecedor.service';
import { Documento } from '../../models/documento';
import { DocumentoService } from './documento.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-documento',
  templateUrl: './documento.component.html',
  styleUrls: [
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class DocumentoComponent implements OnInit {
  @ViewChild('vencimento') input: ElementRef;
  notas: Documento[];
  //public matrizes: Cliente[] = [];
  filterPalavra: string;
  dtOptions: DataTables.Settings = {};
  public inEditMode: boolean = false;
  public currentNF: Documento;
  public buttonLabel: string = 'Create';
  filterTexto: string;
  public emissores: Fornecedor[] = [];
  _emissor = [];
  private _ids = '';

  tipoRelatorio: number;

  public isAdm: boolean = false;

  constructor(private http: HttpClient,
    private authProfile: UsuarioProfile,
    private commonService: CommonService,
    private service: DocumentoService,
    private fornecedorService: FornecedorService,
    private dialogconfirmService: DialogConfirmService,
    private coolDialogs: NgxCoolDialogsService,
    private loadingService: LoadingService,
    private utilsService: UtilsService) {
    this.currentNF = new Documento();
  }

  public remove(id: number): void {
    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Deseja remover esse cliente?', {
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
              this.utilsService.MessageSucessoShow("Cliente deletado com sucesso");
              jQuery("#grdListaCliente").DataTable().ajax.reload();
            }
            else {
              this.utilsService.MessageWarningShow("Cliente não pode ser excluído");
              console.error(res.errors);
            }
          });
      }
    });

  }

  public editSituacao(): void {

    //this._ids = '';
    //if (this.arquivosColetados.length == 0) {

    //  this.coolDialogs.alert('Nenhum arquivo Selecionado', {
    //    theme: 'material',
    //    okButtonText: 'Sair',
    //    color: 'red',
    //    title: 'Atenção'
    //  });
    //  return;
    //}

    //for (var i = 0; i < this.arquivosColetados.length; i++) {

    //  if ($('#' + this.arquivosColetados[i].arquivoColetadoId.toString()).is(":checked")) {
    //    if (this._ids == '')
    //      this._ids = this.arquivosColetados[i].arquivoColetadoId.toString();
    //    else
    //      this._ids = this._ids + ',' + this.arquivosColetados[i].arquivoColetadoId.toString();
    //  }
    //}

    //if (this._ids == '') {

    //  this.coolDialogs.alert('Nenhum arquivo Selecionado', {
    //    theme: 'material',
    //    okButtonText: 'Sair',
    //    color: 'red',
    //    title: 'Atenção'
    //  });
    //  return;
    //}

    //this.currentArquivoColetado = new ArquivoColetado();
    //this.radioSelected == "";

  }

  public edit(nf: Documento): void {
    this.currentNF = nf;
    this.inEditMode = true;
    this.buttonLabel = 'Salvar';
  }

  public create(): void {
    this.currentNF = new Documento();
    this.inEditMode = false;
    this.buttonLabel = 'Incluir';
  }

  public enviarJvl(): void {

    this._ids = '';
    if (this.notas.length == 0) {

      this.coolDialogs.alert('Nenhuma nota fiscal Selecionada', {
        theme: 'material',
        okButtonText: 'Sair',
        color: 'red',
        title: 'Atenção'
      });
      return;
    }

    for (var i = 0; i < this.notas.length; i++) {

      if ($('#' + this.notas[i].id.toString()).is(":checked")) {
        if (this._ids == '')
          this._ids = this.notas[i].id.toString();
        //else
        //  this._ids = this._ids + ',' + this.notas[i].id.toString();

        this.currentNF = new Documento();
       //this.currentNF.obsAdmin = '';
       // this.currentNF.pedidoCompra = '';
       // this.currentNF.situacaoOrcamento = 3;
        this.currentNF.id = this.notas[i].id;

        this.service.aprovarDocumento(this.currentNF)
          .subscribe((res) => {
            
          })



      }
    }

    if (this._ids=='')
    {
      this.coolDialogs.alert('Nenhuma nota fiscal Selecionada', {
        theme: 'material',
        okButtonText: 'Sair',
        color: 'red',
        title: 'Atenção'
      });
    }
    else
      this.utilsService.MessageSucessoShow("Notas fiscais enviadas com sucesso!")

  }

  public createRelatorio() {
    if (!this.validarCamposRelatorio()) {
      return;
    }

    let dataInicialDia = parseInt(($('#dataInicial').val()).substring(0, 2))
    let dataInicialMes = parseInt($('#dataInicial').val().substring(3, 5))
    let dataInicialAno = parseInt($('#dataInicial').val().substring(6, 10))
    let dataFinalDia = parseInt($('#dataFinal').val().substring(0, 2))
    let dataFinalMes = parseInt($('#dataFinal').val().substring(3, 5))
    let dataFinalAno = parseInt($('#dataFinal').val().substring(6, 10))

    this.service.GetRelatorio(dataInicialDia, dataInicialMes, dataInicialAno, dataFinalDia, dataFinalMes, dataFinalAno, this.tipoRelatorio)
      .subscribe((res) => {
        const url = window.URL
          .createObjectURL(new Blob([res._body]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'relatorio.csv');
        document.body.appendChild(link);
        link.click();
      });
  }

  validarCamposRelatorio() {
    if ($('#dataInicial').val() === '' || $('#dataFinal').val() === '' || this.tipoRelatorio === undefined) {
      this.utilsService.MessageWarningShow("Preencha todos os campos para gerar o relatório.");
      return false;
    }
    return true;
  }

  public selecionarTipoRelatorio(tipoRelatorio) {
    this.tipoRelatorio = tipoRelatorio;
  }

  public viewPdf(pdf: string): void {


    var left = (screen.width / 2) - (800 / 2);
    var top = (screen.height / 2) - (700 / 2);
    window.open(pdf, ``, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + 800 + ', height=' + 700 + ', top=' + top + ', left=' + left);
  }

  searchGrid(filterTexto: string) {

    this.filterPalavra = filterTexto
    jQuery("#grdListaCliente").DataTable().ajax.reload();

  }

  public parseNfStatus(status): string {
    switch (status) {
      case 1:
        return 'Pendente';
      case 2:
        return 'Reprovada';
      case 3:
        return 'Aprovada';
      case 4:
        return 'Validada';
    }
  }

  public insertManual(files): void {


    if (!$('#formUploadManual').parsley().isValid())
      return;

    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    this.loadingService.Show();

    for (let file of files)
      formData.append(file.name, file);

    let params = new HttpParams().set('loggedclienteId', this.authProfile.usuarioProfile.currentUsuario.id == null ? "" : this.authProfile.usuarioProfile.currentUsuario.id.toString())

    const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/cliente/UploadClienteFile', formData, {
      reportProgress: true, params
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.Response) {
        this.loadingService.Hide();
        this.utilsService.MessageSucessoShow("Carga de Cliente(s) realizada com sucesso");
        jQuery("#frmUploadManual").modal("hide");
      }
    });



  }

  private CheckAll() {
    $('#allchecked').click(function (event) {
      if (this.checked) {
        $(':checkbox').each(function () {
          this.checked = true;
        });
      }
      else {
        $(':checkbox').each(function () {
          this.checked = false;
        });
      }
    });
  }

  public ngOnInit() {

    //this.isAdm = this.authProfile.usuarioProfile.currentUsuario.fornecedor == null ? true : false;

    const that = this;


    this.CheckAll();

    $('#dataInicial').datepicker({
      clowseText: 'Fechar',
      prevText: '<Anterior',
      nextText: 'Próximo>',
      currentText: 'Hoje',
      monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
      monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
      dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
      dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
      dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
      weekHeader: 'Sm',
      dateFormat: 'dd/mm/yyyy',
      firstDay: 0,
      isRTL: false,
      orientation: 'bottom',
      showMonthAfterYear: false,
      yearSuffix: '',
      format: "dd/mm/yyyy",
      language: "pt-BR",
      autoclose: true,
      todayHighlight: true,
    });

    $('#dataFinal').datepicker({
      closeText: 'Fechar',
      prevText: '<Anterior',
      nextText: 'Próximo>',
      currentText: 'Hoje',
      monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
      monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
      dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
      dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
      dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
      weekHeader: 'Sm',
      dateFormat: 'dd/mm/yyyy',
      firstDay: 0,
      isRTL: false,
      orientation: 'bottom',
      showMonthAfterYear: false,
      yearSuffix: '',
      format: "dd/mm/yyyy",
      language: "pt-BR",
      autoclose: true,
      todayHighlight: true,
    });

    $('#tooltip-html').tooltipster({
      content: $('<p style="text-align:left;">Salvar o arquivo em Formato csv(separado por ;)</p><p style="text-align:left;"><ul><li>Nome Fantasia</li><li>Razao Social</li><li>Telefone</li><li>CNPJ</li><li>CNPJ MATRIZ</li><li>CEP</li><li>Estado</li><li>Cidade</li><li>Endereço</li><li>Número</li><li>Bairro</li><li>Complemento</li></ul></p>'),
      minWidth: 550,
      maxWidth: 550,
      position: 'left'
    });

    this.dtOptions = {
      pagingType: 'full_numbers',
      dom: 'Btp',
      pageLength: 100,
      serverSide: true,
      responsive: true,
      processing: true,
      searching: true,
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

        let params = new HttpParams().set('loggedFornecedorId', this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId.toString())
          .set('palavra', this.filterPalavra);


        that.http
          .post<DataTablesResponse>(
            this.commonService.getBaseUrl() + '/notafiscal/',
            dataTablesParameters, {
            params,
            headers: new HttpHeaders({
              "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
              "Content-Type": "application/json"
            })
          }
          ).subscribe(resp => {
            that.notas = resp.data;
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
            orderable: false,
            className: "select-checkbox",
            targets: 0
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
            targets: 3,
            visible: true
          },
          {
            targets: 4,
            visible: true
          },
          {
            targets: 5,
            visible: true
          },
          {
            targets: 6,
            visible: true
          },
          {
            targets: 7,
            visible: true
          },
          {
            data: null,
            targets: -1,
            orderable: false
          }
        ],
      columns: [
        { width: '5%' },
        { width: '7%' },
        { width: '23%' },
        { width: '23%' },
        { width: '12%' },
        { width: '12%' },
        { width: '10%' },
        { width: '10%' },
        { data: null, width: '10%' }
      ]
    };
  }
}
