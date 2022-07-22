import { HttpClient, HttpHeaders, HttpParams, HttpRequest, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataTablesResponse } from '../../models/DataTablesResponse';
import { Fornecedor } from '../../models/fornecedor';
import { DialogConfirmService } from '../common/dialogconfirm.service';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { NgxCoolDialogsService, NgxCoolDialogResult } from 'ngx-cool-dialogs';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
import { Empresa } from '../../models/empresa';
import { EmpresaService } from '../empresa/empresa.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-arquivo',
  templateUrl: './arquivo.component.html',
  styleUrls: ['./arquivo.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class ArquivoComponent implements OnInit {

  constructor(private http: HttpClient,
              private authProfile: UsuarioProfile,
              private commonService: CommonService,
              private service: EmpresaService,
              private dialogconfirmService: DialogConfirmService,
              private coolDialogs: NgxCoolDialogsService,
              private loadingService: LoadingService,
              private utilsService: UtilsService,
  ) {
  }

  public insertManual(files): void {


    if (!$('#formUploadManual').parsley().isValid())
      return;

    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    this.loadingService.Show();

    for (let file of files) {
      formData.append('empresa_fornecedor', this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId.toString());
      formData.append(file.name, file);
    }


    //let params = new HttpParams().set('id', "1")

    const uploadReq = new HttpRequest('POST', this.commonService.getBaseUrl() + '/arquivo/UploadFile', formData, {
      reportProgress: true
    });

    console.log(this.commonService.getBaseUrl() + '/arquivo/UploadFile')

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.Response) {
        this.loadingService.Hide();
        this.utilsService.MessageSucessoShow("Carga de Cliente(s) realizada com sucesso");
        jQuery("#frmUploadManual").modal("hide");
      }
    });



  }
    

  public updatedadosAcesso(fornecedorId: number): void {
   // this.lstDadosAcesso = [];
    //this.serviceDadosAcesso.getByFornecedor(fornecedorId)
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.lstDadosAcesso = res.result;
    //    }
    //  });
  }

  upload(files) {

    //if (!this.validaCamposUpload(files))
   //   return;

    const formData = new FormData();
    for (let file of files)
      formData.append('files', file);

    this.http.post(this.commonService.getBaseUrl() + '/fornecedor/UploadFile', formData).subscribe(event => {
      //jQuery("#frmFornecedorTemplate").modal("hide");
    });

  }

  public edit(empresa: Empresa): void {
    //this.currentEmpresa = empresa;
    //this.inEditMode = true;
    //this.buttonLabel = 'Salvar';
  }

  public create(): void {
    //this.currentEmpresa = new Empresa();
    //this.currentFornecedor.fkTipoFornecedorId = new TipoFornecedor();
    //this.currentFornecedor.fkTipoFornecedorId.tipoFornecedorId = 0;
    //this.inEditMode = false;
   // this.buttonLabel = 'Incluir';
  }
   
  public ngOnInit() {
    const that = this;

    $('#my_multi_select3').multiSelect({
      selectableHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='pesquisar...'>",
      selectionHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='pesquisar...'>",
      afterInit: function (ms) {
        var that = this,
          $selectableSearch = that.$selectableUl.prev(),
          $selectionSearch = that.$selectionUl.prev(),
          selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
          selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
          .on('keydown', function (e) {
            if (e.which === 40) {
              that.$selectableUl.focus();
              return false;
            }
          });

        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
          .on('keydown', function (e) {
            if (e.which == 40) {
              that.$selectionUl.focus();
              return false;
            }
          });
      },
      afterSelect: function () {
        this.qs1.cache();
        this.qs2.cache();
      },
      afterDeselect: function () {
        this.qs1.cache();
        this.qs2.cache();
      }
    });

  }
}

