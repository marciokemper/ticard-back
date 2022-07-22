import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { NgxCoolDialogResult, NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { UtilsService } from '../common/utils.service';
import { GraficosService } from '../graficos/graficos.service';
import { Documento } from '../../models/documento';
import { DocumentoService } from '../documento/documento.service';
import { IProfile } from '../../models/profile';
declare var google: any;
declare var $: any;
declare var Morris: any;
declare var c3: any;


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css',
    '../../assets/plugins/switchery/switchery.min.css'
  ]
})


export class DashboardComponent implements OnInit {
  public pageTitle: string = 'Principal';
  public status = [];
  public data1: any[];
  public documentos = [];
  public currentDocto: Documento;

  // Gráficos
  public statusGraficos = [];
  public dataGraficos: any[];
  public qtdAPagar: number = 0;
  public vlrAPagar: number = 0;
  public anoCorrente: string = '';
  public anoAnterior: string = '';
  public mesCorrente: string = '';
  public mesAnterior: string = '';
  public qtddocumento: string = '0';
  public qtNfServicoAprovadas: string = '0';
  public qtNfProdutoAprovadas: string = '0';
  public vlrmes: string = '0';

  public loggedclienteId: string = ' ';
  public ultimosColetados = [];
  public valorPorMesProduto = [];
  public valorPorMesServico = [];
  public valorPorTipo = [];
  public valorTipoMes = [];

  Profile: IProfile;
  isAdmin: boolean;


  constructor(
    private service: DocumentoService,
    private http: HttpClient,
    private coolDialogs: NgxCoolDialogsService,
    private authProfile: UsuarioProfile,
    private utilsService: UtilsService,
    private commonService: CommonService) {

    this.getDocumentos();


  }

  public aprovacao(id: number): void {
    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Confirma a aprovação?', {
      theme: 'default',
      okButtonText: 'Aprovar',
      cancelButtonText: 'Sair',
      color: '#3a3d42',
      title: 'Atenção'
    });

    dialog.subscribe(res => {
      if (res == true) {

        this.service.delete(id)
          .subscribe((res) => {
            if (res.success) {
              this.utilsService.MessageSucessoShow("Nota Fiscal aprovada com sucesso");
              jQuery("#grdListaCliente").DataTable().ajax.reload();
            }
            else {
              this.utilsService.MessageWarningShow("Erro durante a aprovação da nota fiscal");
              console.error(res.errors);
            }
          });
      }
    });

  }

  public updateaprovar(): void {

    this.service.aprovarDocumento(this.currentDocto)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Documento aprovado com sucesso!")
          jQuery("#frmAprovar").modal("hide");
          this.getDocumentos();
        }
        else {
          this.utilsService.MessageWarningShow("Documento não aprovado")
          jQuery("#frmAprovar").modal("hide");
        }
      })


  }
  public updatevalidar(): void {


    // if (this.currentDocto.pedidoCompra == null) {
    //   this.utilsService.MessageSucessoShow("Número pedido de compra obrigatório!");
    //   return;
    // }
    //console.log(this.currentDocto);

    this.service.validarNF(this.currentDocto)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Nota Fiscal validada com sucesso!")
          // jQuery("#frmValidar").modal("hide");
          this.getDocumentos();
        }
        else {
          this.utilsService.MessageWarningShow("Nota Fiscal não pode ser validada pro problemas técnicos")
          // jQuery("#frmValidar").modal("hide");
        }
      })


  }

  public updaterejeitar(): void {

    this.service.rejeitarDocumento(this.currentDocto)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Documento rejeitado com sucesso!")
          jQuery("#frmRejeitar").modal("hide");
          this.getDocumentos();
        }
        else {
          this.utilsService.MessageWarningShow("Documento não rejeitado")
          jQuery("#frmRejeitar").modal("hide");
        }
      })


  }

  public exitpopup(): void {
    jQuery("#grdListaArquivos").DataTable().ajax.reload();
  }

  public aprovar(dc: Documento): void {
    this.currentDocto = dc;
    //this.currentDocto.obsAdmin = '';
  }
  public validar(dc: Documento): void {
    this.currentDocto = dc;
    //this.currentDocto.obsAdmin = '';
    this.updatevalidar();
  }
  public rejeitar(nf: Documento): void {
    this.currentDocto = nf;
    //this.currentNF.obsAdmin = '';
  }

  public viewPdf(pdf: string): void {


    var left = (screen.width / 2) - (800 / 2);
    var top = (screen.height / 2) - (700 / 2);
    window.open(pdf, ``, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + 800 + ', height=' + 700 + ', top=' + top + ', left=' + left);
  }

  public getDocumentos(): void {
    this.documentos = [];

    this.service.GetAllByFilter('', '', '', '')
      .subscribe((res) => {
        if (res.success) {
          this.documentos = res.result;

        }
      });



  }

  public ngOnInit() {
    this.Profile = this.authProfile.usuarioProfile;

    this.isAdmin = false;
    if (this.Profile.currentUsuario.empresaFornecedorId == 0)
      this.isAdmin = true;

  }


  public getMonthCurrent(): string {
    var _month = new Date().getMonth();

    if (_month == 0)
      return 'Janeiro';
    else if (_month == 1)
      return 'Fevereiro';
    else if (_month == 2)
      return 'Março';
    else if (_month == 3)
      return 'Abril';
    else if (_month == 4)
      return 'Maio';
    else if (_month == 5)
      return 'Junho';
    else if (_month == 6)
      return 'Julho';
    else if (_month == 7)
      return 'Agosto';
    else if (_month == 8)
      return 'Setembro';
    else if (_month == 9)
      return 'Outubro';
    else if (_month == 10)
      return 'Novembro';
    else
      return 'Dezembro';
  }

  public getMonthPrevius(): string {
    var date = new Date();
    date.setMonth(date.getMonth() - 1);
    var _month = date.getMonth();

    if (_month == 0)
      return 'Janeiro';
    else if (_month == 1)
      return 'Fevereiro';
    else if (_month == 2)
      return 'Março';
    else if (_month == 3)
      return 'Abril';
    else if (_month == 4)
      return 'Maio';
    else if (_month == 5)
      return 'Junho';
    else if (_month == 6)
      return 'Julho';
    else if (_month == 7)
      return 'Agosto';
    else if (_month == 8)
      return 'Setembro';
    else if (_month == 9)
      return 'Outubro';
    else if (_month == 10)
      return 'Novembro';
    else
      return 'Dezembro';
  }

}

