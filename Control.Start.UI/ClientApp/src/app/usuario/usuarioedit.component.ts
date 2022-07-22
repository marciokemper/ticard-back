import { Component, OnInit, ViewChild} from '@angular/core';
import { HttpClient, HttpHeaders, HttpEventType, HttpResponse, HttpRequest } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Usuario } from '../../models/usuario';
import { UsuarioService } from './usuario.service';
import { UtilsService } from '../common/utils.service';
import { forEach } from '@angular/router/src/utils/collection';
import { UsuarioProfile } from './usuario.profile';
import { Fornecedor } from '../../models/fornecedor';
import { FornecedorService } from '../fornecedor/fornecedor.service';
import { Empresa } from '../../models/empresa';
import { EmpresaService } from '../empresa/empresa.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-usuario-edit',
  templateUrl: './usuarioedit.component.html',
  styleUrls: ['./usuario.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class UsuarioEditComponent implements OnInit {

  public fornecedores: Fornecedor[] = [];
  public empresas: Empresa[] = [];
  public inEditMode: boolean = false;
  public currentUsuario: Usuario;
  public buttonLabel: string = 'Create';
  private id: number | null;
  public progress: number;
  public message: string;
  public isAdm: boolean;
  public isAdmGeral: boolean;
  public fornecedorSelected: string;
  public empresaSelected: string;

  constructor(private http: HttpClient,
              private router: Router,
              private authProfile: UsuarioProfile,
              private route: ActivatedRoute,
              public service: UsuarioService,
    private fornecedorService: FornecedorService,
    private empresaService: EmpresaService,
              private utilsService: UtilsService) {

    this.currentUsuario = new Usuario();
   // this.currentUsuario.fornecedor = new Fornecedor();
    this.fornecedorSelected = '';
    //t//his.isAdm = this.authProfile.usuarioProfile.currentUsuario.fornecedor == null ? true : false;
    //if (this.authProfile.usuarioProfile.currentUsuario.tipoUsuario == null || this.authProfile.usuarioProfile.currentUsuario.tipoUsuario == 1)
      this.isAdmGeral = true;
    //else
     // this.isAdmGeral = false;

  }

  public getFornecedores(): void {

    this.fornecedores = [];
    this.fornecedorService.getAllByAtivo()
      .subscribe((res) => {
        if (res.success) {
          this.fornecedores = res.result;
        } else {
          console.error(res.errors);
        }
     });

  }

  public getEmpresas(): void {

    this.empresas = [];
    this.empresaService.getAll()
      .subscribe((res) => {
        if (res.success) {
          this.empresas = res.result;
        } else {
          console.error(res.errors);
        }
      });

  }

  public save(): void {


    if ($('#formUsuario').parsley().isValid()) {

      //if (this.fornecedorSelected != '') {
      //  this.currentUsuario.fornecedor = new Fornecedor();
      //  this.currentUsuario.fornecedor.id = Number(this.fornecedorSelected);

      //}

      //if (this.empresaSelected != '') {
      // /* this.currentUsuario.empresa = new Empresa();*/
      //  this.currentUsuario.empresa.id = Number(this.empresaSelected);

      //}
      //else {

      //  this.currentUsuario.empresa = new Empresa()
      //  this.currentUsuario.empresa.id = this.authProfile.usuarioProfile.currentUsuario.empresaId
      //}
      if (!this.inEditMode) {
        this.saveNewUsuario();
      } else {
        this.updateUsuario();
      }

    }

  }


  private saveNewUsuario(): void {
    this.service.inserir(this.currentUsuario)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
          this.router.navigate(['usuario']);
        }
        else {
          this.utilsService.MessageWarningShow("Erro durante a inclusão do usuário")
        }
      });
  }

  private updateUsuario(): void {
    this.service.put(this.currentUsuario.id, this.currentUsuario)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
          this.router.navigate(['usuario']);
          this.currentUsuario = new Usuario();
          this.inEditMode = false;
          this.buttonLabel = 'Incluir';
        }
        else {
          this.utilsService.MessageWarningShow("Erro durante a alteração do usuário")
        }
      })


  }

  public getUsuarioById(id: number): void {
    this.service.getById(id)
      .subscribe((res) => {
        if (res.success) {
          this.currentUsuario = res.result;
          //this.fornecedorSelected = this.currentUsuario.fornecedor == null ? '' : this.currentUsuario.fornecedor.id.toString();
          //this.empresaSelected = this.currentUsuario.empresa == null ? '' : this.currentUsuario.empresa.id.toString();
        } else {
          console.error(res.errors);
        }
      });
  }

  public ngOnInit() {
    const that = this;
    this.getFornecedores();
    this.getEmpresas();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
    })

    if (this.id != -1) {
      this.inEditMode = true;
      this.buttonLabel = 'Salvar';
      this.getUsuarioById(this.id);
    } else {
      this.currentUsuario = new Usuario();
      //this.currentUsuario.fornecedor = new Fornecedor();
      //this.currentUsuario.fornecedor.id = 0;
      this.fornecedorSelected = '';
      this.empresaSelected = '';
      this.buttonLabel = 'Incluir';
      this.inEditMode = false;
    }

    $(function () {
      $('#formUsuario').parsley().on('field:validated', function () {
        var ok = $('.parsley-error').length === 0;
        $('.alert-info').toggleClass('hidden', !ok);
        $('.alert-warning').toggleClass('hidden', ok);
      })
        .on('form:submit', function () {
          return false; // Don't submit form for this demo
        });
    });

  }
}
