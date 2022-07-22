import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Fornecedor } from '../../models/fornecedor';
import { EmpresaService } from './empresa.service';
import { CepService } from '../shared/cep.service';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
import { Empresa } from '../../models/empresa';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-empresa-edit',
  templateUrl: './empresaedit.component.html',
  styleUrls: ['./empresa.component.css',
    '../../assets/css/bootstrap.min.css',
    '../../assets/css/core.css',
    '../../assets/css/components.css',
    '../../assets/css/icons.css',
    '../../assets/css/pages.css',
    '../../assets/css/menu.css',
    '../../assets/css/responsive.css'
  ]
})


export class EmpresaEditComponent implements OnInit {
  public inEditMode: boolean = false;
  public currentEmpresa: Empresa;
  public buttonLabel: string = 'Create';
  private id: number | null;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private service: EmpresaService,
    private cepService: CepService,
    private loadingService: LoadingService,
    private utilsService: UtilsService) {
    this.currentEmpresa = new Empresa();
  }


  public save(): void {
    if ($('#formFornecedor').parsley().isValid()) {
      if (!this.inEditMode) {
        this.saveNewEmpresa();
      } else {
        this.updateEmpresa();
      }
    }
  }

  private saveNewEmpresa(): void {

    this.loadingService.Show();

    this.service.inserir(this.currentEmpresa)
      .subscribe((res) => {
        if (res.success) {
          this.loadingService.Hide();
          this.utilsService.MessageSucessoShow("Inclusão Realizada com sucesso")
          this.router.navigate(['empresa']);
        }
        else {
          this.loadingService.Hide();
          this.utilsService.MessageWarningShow("Erro durante a inclusão do empresa")
        }
      });
  }

  private updateEmpresa(): void {
    this.loadingService.Show();
    this.service.put(this.currentEmpresa.id, this.currentEmpresa)
      .subscribe((res) => {
        if (res.success) {
          this.loadingService.Hide();
          this.utilsService.MessageSucessoShow("Alteração Realizada com sucesso")
          this.router.navigate(['empresa']);
        }
        else {
          this.loadingService.Hide();
          this.utilsService.MessageWarningShow("Erro durante a alteração do empresa");
        }
      })


  }

  public getEmpresaById(id: number): void {
    this.service.getById(id)
      .subscribe((res) => {
        if (res.success) {
          this.currentEmpresa = res.result;
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
      this.getEmpresaById(this.id);
    } else {
      this.currentEmpresa = new Empresa();
      this.buttonLabel = 'Incluir';
      this.inEditMode = false;
    }


  }
}
