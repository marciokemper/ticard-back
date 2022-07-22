import { HttpClient, HttpHeaders, HttpEventType, HttpResponse, HttpRequest } from '@angular/common/http';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { CepService } from '../shared/cep.service';
import { UtilsService } from '../common/utils.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { IProfile } from '../../models/profile';
import { CommonService } from '../shared/common.service';
import { DatePipe } from '@angular/common';
import { LoadingService } from '../common/loading.service';
import { VariableAst } from '@angular/compiler';
import { DocumentoService } from './documento.service';
import { Documento } from '../../models/documento';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-documento-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./documento.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class EditComponent implements OnInit {
  @ViewChild('vencimento') input: ElementRef;
  public inEditMode: boolean = false;
  public currentNF: Documento;
  public buttonLabel: string = 'Create';
  public progress: number;
  public message: string;
  public lblnome: string;
  public lblrazaosocial: string;
  public lblcpfcnpj: string;
  public lblformat: string;
  public lblmask: string;
  public controlDisabled: boolean = false;
  public isAdm: boolean = false;
  public file: any;
  private id: number | null;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private authProfile: UsuarioProfile,
    private commonService: CommonService,
    private service: DocumentoService,
    private http: HttpClient,
    private loadingService: LoadingService,
    private cepService: CepService,
    private datePipe: DatePipe,
    private utilsService: UtilsService) {

    this.currentNF = new Documento();
    this.lblcpfcnpj = 'CNPJ';
    this.lblformat = '99.999.999/9999-99';
    this.lblmask = '00.000.000/0000-00';


  }

  public save(file): void {
    if ($('#formCliente').parsley().isValid()) {
      //debugger;
      // Se a nota está sendo criada OU nota sendo editada quando rejeitada, cria uma nova
      if (!this.inEditMode || (this.inEditMode && this.currentNF.situacaoDocumento === 2)) {

        if (!file || !file.files || file.files.length === 0) {
          return;
        }
        this.saveNewNF(file.files);
      } else {
        this.updateNF();
      }
    }
  }

  private FormatDate(texto: string): Date {
    var _split = texto.split('/');
    var datestring = _split[2] + "-" + _split[1] + "-" + _split[0] + "T00:00:00.000-03:00";
    return new Date(datestring);
  }

  private FormatValor(texto: string): Number {
    if(!texto)
      return 0;
    //var number = texto.replace('.','').replace(',','.');
    return new Number(texto);

    // if (vl.indexOf(',') > 0) {
    //   vl = texto.replace(',', '').replace('.', '');
    //   let cd = vl.substr(vl.length - 2, 2);
    //   let int = vl.substr(0, vl.length - 2);
    //   let ret = Number(`${int}.${cd}`);
    //   return ret;
    // }

    // return 0;

  }

  private saveNewNF(files): void {
    this.loadingService.Show();
    this.currentNF.caminhoAnexo = files[0].name;

    //this.currentNF.vencimento = this.FormatDate(this.input.nativeElement.value);

   // this.currentNF.valor = this.FormatValor(this.currentNF.valor.toString());

    const formData = new FormData();

    for (let file of files)
      formData.append(file.name, file);

    const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/notafiscal/UploadDocumentoFile', formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.Response) {
        this.currentNF.caminhoAnexo = event.body['fullPath'].toString();
        this.service.inserir(this.currentNF)
          .subscribe((res) => {
            if (res.success) {
              this.loadingService.Hide();
              this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
              this.router.navigate(['notafiscal']);
            }
            else {
              this.loadingService.Hide();
              this.utilsService.MessageWarningShow("Erro durante a inclusão")
            }
          });
      }
    });
  }

  private updateNF(): void {
    //debugger;
    this.loadingService.Show();
    //this.currentNF.vencimento = this.FormatDate(this.input.nativeElement.value);
    //this.currentNF.valor = this.FormatValor(this.currentNF.valor.toString());
    this.service.put(this.currentNF.id, this.currentNF)
        .subscribe((res) => {
          if (res.success) {
            this.loadingService.Hide();
            this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
            this.router.navigate(['notafiscal']);
          }
          else {
            this.loadingService.Hide();
            this.utilsService.MessageWarningShow("Erro durante a inclusão")
          }
    });
    

    // const formData = new FormData();

    // for (let file of files)
    //   formData.append(file.name, file);

    // const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/notafiscal/UploadDocumentoFile', formData, {
    //   reportProgress: true,
    // });

    // this.http.request(uploadReq).subscribe(event => {
    //   if (event.type === HttpEventType.Response) {
    //     this.currentNF.caminhoAnexo = event.body['fullPath'].toString();
    //     this.service.put(this.currentNF.id, this.currentNF)
    //       .subscribe((res) => {
    //         if (res.success) {
    //           this.loadingService.Hide();
    //           this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
    //           this.router.navigate(['notafiscal']);
    //         }
    //         else {
    //           this.loadingService.Hide();
    //           this.utilsService.MessageWarningShow("Erro durante a inclusão")
    //         }
    //       });
    //   }
    // });

    // this.currentNF.caminhoAnexo = files[0].name;
    // this.service.put(this.currentNF.id, this.currentNF)
    //   .subscribe((res) => {
    //     if (res.success) {
    //       const formData = new FormData();

    //       for (let file of files)
    //         formData.append(file.name, file);

    //       const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/arquivoColetado/UploadDocumentoFile', formData, {
    //         reportProgress: true,
    //       });

    //       this.http.request(uploadReq).subscribe(event => {

    //       });
    //       this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
    //       this.router.navigate(['notafiscal']);
    //     }
    //     else {
    //       this.utilsService.MessageWarningShow("Erro durante a alteração da nota fiscal")
    //     }
    //   })


  }


  public onChange(selectObject) {
    if (selectObject == 1) {
      this.lblnome = 'Nome Completo';
      this.lblrazaosocial = 'Apelido';
      this.lblcpfcnpj = 'CPF';
      this.lblformat = '999.999.999-99';
      this.lblmask = '000.000.000-00';

    }
    else {
      this.lblnome = 'Razão Social';
      this.lblrazaosocial = 'Razão Social';
      this.lblcpfcnpj = 'CNPJ';
      this.lblformat = '99.999.999/9999-99';
      this.lblmask = '00.000.000/0000-00';
    }
  }

  public getClienteById(id: number): void {
    this.service.getById(id)
      .subscribe((res) => {
        if (res.success) {
          this.currentNF = res.result;
          //this.input.nativeElement.value = this.datePipe.transform(this.currentNF.vencimento, 'dd/MM/yyyy');
          //if (this.currentNF.clientePessoa == 1) {
          //  this.lblnome = 'Nome Completo';
          //  this.lblrazaosocial = 'Apelido';
          //  this.lblcpfcnpj = 'CPF';
          //  this.lblformat = '999.999.999-99';
          //  this.lblmask = '000.000.000-00';
          //}
          //else {
          //  this.lblnome = 'Razão Social';
          //  this.lblrazaosocial = 'Razão Social';
          //  this.lblcpfcnpj = 'CNPJ';
          //  this.lblformat = '99.999.999/9999-99';
          //  this.lblmask = '00.000.000/0000-00';
          //}
        } else {
          console.error(res.errors);
        }
      });

  }

  public ngOnInit() {
    const that = this;

   // this.isAdm = this.authProfile.usuarioProfile.currentUsuario.fornecedor == null ? true : false;
    

    $('#datevencimento').datepicker({
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

    $(function () {
      $('#formCliente').parsley().on('field:validated', function () {
        var ok = $('.parsley-error').length === 0;
        $('.alert-info').toggleClass('hidden', !ok);
        $('.alert-warning').toggleClass('hidden', ok);
      })
        .on('form:submit', function () {
          return false; // Don't submit form for this demo
        });

    });

    this.route.params.subscribe((params) => {
      this.id = params['id'];
    })

    if (this.id != -1) {
      this.inEditMode = true;
      this.buttonLabel = 'Salvar';
      this.getClienteById(this.id);
      this.lblnome = 'Nome Completo';
      this.lblrazaosocial = 'Apelido';
      this.lblcpfcnpj = 'CPF';
      this.lblformat = '999.999.999-99';
      this.lblmask = '000.000.000-00';
    } else {
      this.currentNF = new Documento();
      //this.currentNF.clientePessoa = 2;
      this.lblnome = 'Razão Social';
      this.lblrazaosocial = 'Razão Social';
      this.lblcpfcnpj = 'CNPJ';
      this.lblformat = '99.999.999/9999-99';
      this.lblmask = '00.000.000/0000-00';
      this.buttonLabel = 'Incluir';
      this.inEditMode = false;
    }

  }
}
