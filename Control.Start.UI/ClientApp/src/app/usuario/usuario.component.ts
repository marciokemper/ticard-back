import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataTablesResponse } from '../../models/DataTablesResponse';
import { Usuario } from '../../models/usuario';
import { DialogConfirmService } from '../common/dialogconfirm.service';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from './usuario.profile';
import { UsuarioService } from './usuario.service';
import { NgxCoolDialogResult, NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
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


export class UsuarioComponent implements OnInit {
  usuarios: Usuario[];
  dtOptions: DataTables.Settings = {};
  public inEditMode: boolean = false;
  public currentUsuario: Usuario;
  public buttonLabel: string = 'Create';

  constructor(private http: HttpClient,
    private authProfile: UsuarioProfile,
    private commonService: CommonService,
    private dialogconfirmService: DialogConfirmService,
    private service: UsuarioService,
    private coolDialogs: NgxCoolDialogsService,
    private loadingService: LoadingService,
    private utilsService: UtilsService) {

    this.currentUsuario = new Usuario();
    
  }

  public remove(id: number): void {
    let dialog: NgxCoolDialogResult;
    dialog = this.coolDialogs.confirm('Deseja remover esse usuário?', {
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
              this.utilsService.MessageSucessoShow("Usuário deletado com sucesso")
              jQuery("#grdListaUsuario").DataTable().ajax.reload();
            }
            else {
              this.utilsService.MessageWarningShow("Usuário não pode ser excluído")
              console.error(res.errors);
            }
          });
      }
    });

  

  }

  public edit(usuario: Usuario): void {
    this.currentUsuario = usuario;
    this.inEditMode = true;
    this.buttonLabel = 'Salvar';
  }

  public create(): void {
    
    this.inEditMode = false;
    this.buttonLabel = 'Incluir';
  }


  public ngOnInit() {
    const that = this;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      responsive:true,
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
        this.loadingService.Show()

        let params = new HttpParams().set('loggedEmpresaId', this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId == null ? "" : this.authProfile.usuarioProfile.currentUsuario.empresaFornecedorId.toString())

        that.http
          .post<DataTablesResponse>(
            this.commonService.getBaseUrl() + '/usuario',
            dataTablesParameters, {
              params,
              headers: new HttpHeaders({
                "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
                "Content-Type": "application/json"
              })
            }
          ).subscribe(resp => {
            that.usuarios = resp.data;
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
            targets: 4,
            visible: true
          },
          {
            targets: 5,
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
        { width: '25%' },
        { width: '16%' },
        { width: '16%' },
        { width: '10%' },
        { width: '10%' },
        { width: '7%' }
      ]
    };
  }
}
