import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataTablesResponse } from '../../models/DataTablesResponse';
import { Fornecedor } from '../../models/fornecedor';
import { DialogConfirmService } from '../common/dialogconfirm.service';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { FornecedorService } from './fornecedor.service';
import { NgxCoolDialogsService, NgxCoolDialogResult } from 'ngx-cool-dialogs';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-fornecedor',
  templateUrl: './fornecedor.component.html',
  styleUrls: ['./fornecedor.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class FornecedorComponent implements OnInit {
  fornecedores: Fornecedor[];
  dtOptions: DataTables.Settings = {};
  public inEditMode: boolean = false;
  public currentFornecedor: Fornecedor;
  public buttonLabel: string = 'Create';

  constructor(private http: HttpClient,
              private authProfile: UsuarioProfile,
              private commonService: CommonService,
              private service: FornecedorService,
              private dialogconfirmService: DialogConfirmService,
              private coolDialogs: NgxCoolDialogsService,
              private loadingService: LoadingService,
              private utilsService: UtilsService,
  ) {
    this.currentFornecedor = new Fornecedor();
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

    if (!this.validaCamposUpload(files))
      return;

    const formData = new FormData();
    for (let file of files)
      formData.append('files', file);

    this.http.post(this.commonService.getBaseUrl() + '/fornecedor/UploadFile', formData).subscribe(event => {
      //jQuery("#frmFornecedorTemplate").modal("hide");
    });

  }

  private validaCamposUpload(files): boolean {


    if (files.length === 0) {

      this.coolDialogs.alert('Nenhum arquivo selecionado!', {
        theme: 'material',
        okButtonText: 'Sair',
        color: 'red',
        title: 'Atenção'
      });
      return false;
    }

    return true;
  }

  public remove(id: number): void {


    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Deseja remover esse Fornecedor?', {
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
            this.utilsService.MessageSucessoShow("Fornecedor deletado com sucesso")
            jQuery("#grdListaFornecedor").DataTable().ajax.reload();
          }
          else {
            this.utilsService.MessageWarningShow("Fornecedor não pode ser excluído")
            console.error(res.errors);
          }
        });
      }
    });
    
  }

  public createTemplate(fornecedor: Fornecedor): void {
    //this.currentVinculacao = new fornecedorCliente();
    //this.currentVinculacao.fkFornecedorId = new Fornecedor();
    //this.currentVinculacao.fkClienteId = new Cliente();

    //this.inEditMode = false;
    //this.buttonLabel = 'Vincular';
  }

  public edit(fornecedor: Fornecedor): void {
    this.currentFornecedor = fornecedor;
    this.inEditMode = true;
    this.buttonLabel = 'Salvar';
  }

  public create(): void {
    this.currentFornecedor = new Fornecedor();
    //this.currentFornecedor.fkTipoFornecedorId = new TipoFornecedor();
    //this.currentFornecedor.fkTipoFornecedorId.tipoFornecedorId = 0;
    this.inEditMode = false;
    this.buttonLabel = 'Incluir';
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



    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      responsive: true,
      serverSide: true,
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
        this.loadingService.Show

        let params = new HttpParams().set('loggedFornecedorId', this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId.toString())

        that.http
          .post<DataTablesResponse>(
          this.commonService.getBaseUrl() + '/fornecedor',
            dataTablesParameters, {
             params,
            headers: new HttpHeaders({
              "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
              "Content-Type": "application/json"
            })
          }
        ).subscribe(resp => {
          that.fornecedores = resp.data;
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
            targets: 3,
            visible: true
          },
          {
            data: null,
            targets: -1,
            orderable:false
          }
        ],
      columns: [
        { width: '7%' },
        { width: '8%' },
        { width: '63%' },
        { width: '15%' },
        { data: null, width: '7%'}
      ]
    };
  }
}

