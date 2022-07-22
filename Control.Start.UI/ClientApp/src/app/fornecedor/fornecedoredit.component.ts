import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Fornecedor } from '../../models/fornecedor';
import { FornecedorService } from './fornecedor.service';
import { CepService } from '../shared/cep.service';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { Empresa } from '../../models/empresa';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-fornecedor-edit',
  templateUrl: './fornecedoredit.component.html',
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


export class FornecedorEditComponent implements OnInit {
  public inEditMode: boolean = false;
  public currentFornecedor: Fornecedor;
  public buttonLabel: string = 'Create';
  private id: number | null;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private service: FornecedorService,
    private authProfile: UsuarioProfile,
    private loadingService: LoadingService,
    private utilsService: UtilsService) {
    this.currentFornecedor = new Fornecedor();
  }


    public save(): void {
    if ($('#formFornecedor').parsley().isValid()) {
      if (!this.inEditMode) {
        this.saveNewCliente();
      } else {
        this.updateFornecedor();
      }
    }
  }

  private saveNewCliente(): void {

    this.loadingService.Show();    

    //this.currentFornecedor.empresa = new Empresa()
    //this.currentFornecedor.empresa.id = this.authProfile.usuarioProfile.currentUsuario.empresaId

    this.service.inserir(this.currentFornecedor)
      .subscribe((res) => {
        if (res.success) {
          this.loadingService.Hide();
          this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
          this.router.navigate(['fornecedor']);
        }
        else {
          this.loadingService.Hide();
          this.utilsService.MessageWarningShow("Erro durante a inclusão do fornecedor")
        }
      });
  }

  private updateFornecedor(): void {
    this.loadingService.Show();

    //this.currentFornecedor.empresa = new Empresa()
    //this.currentFornecedor.empresa.id = this.authProfile.usuarioProfile.currentUsuario.empresaId

    this.service.put(this.currentFornecedor.id, this.currentFornecedor)
      .subscribe((res) => {
        if (res.success) {
          this.loadingService.Hide();
          this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
          this.router.navigate(['fornecedor']);
        }
        else {
          this.loadingService.Hide();
          this.utilsService.MessageWarningShow("Erro durante a alteração do fornecedor");
        }
      })


  }

  public getFornecedorById(id: number): void {
    this.service.getById(id)
      .subscribe((res) => {
        if (res.success) {
          this.currentFornecedor = res.result;
        } else {
          console.error(res.errors);
        }
      });
  }


  public ngOnInit() {
    const that = this;

    $(function () {
      $('#formFornecedor').parsley().on('field:validated', function () {
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
      this.getFornecedorById(this.id);
    } else {
      this.currentFornecedor = new Fornecedor();
      this.buttonLabel = 'Incluir';
      this.inEditMode = false;
    }


  }
}
