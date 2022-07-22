import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Configuracao } from '../../models/configuracao';
import { ConfiguracaoService } from './configuracao.service';
import { LoadingService } from '../common/loading.service';
import { UtilsService } from '../common/utils.service';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-configuracao',
  templateUrl: './configuracao.component.html',
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


export class ConfiguracaoComponent implements OnInit {
  public currentConfiguracao: Configuracao;

  constructor(private http: HttpClient,
    private service: ConfiguracaoService,
    private loadingService: LoadingService,
    private utilsService: UtilsService) {
    this.currentConfiguracao = new Configuracao();
  }

  public getConfiguracao(): void {
    
    this.currentConfiguracao = new Configuracao();
    this.service.get()
      .subscribe((res) => {
        if (res.success) {
          this.currentConfiguracao = res.result;
        } else {
          console.error(res.errors);
        }
      });
  }
    

  public save(): void {
    if ($('#formConfiguracao').parsley().isValid()) {
      this.updateCliente();
    }
  }

  private updateCliente(): void {
    this.service.put(this.currentConfiguracao.id, this.currentConfiguracao)
      .subscribe((res) => {
        if (res.success) {
          this.utilsService.MessageSucessoShow("Configuração Alterada com sucesso")
        }
        else {
          this.utilsService.MessageWarningShow("Erro durante a alteração das configurações")
        }
      });
  }


  public ngOnInit() {

    $(function () {
      $('#formConfiguracao').parsley().on('field:validated', function () {
        var ok = $('.parsley-error').length === 0;
        $('.alert-info').toggleClass('hidden', !ok);
        $('.alert-warning').toggleClass('hidden', ok);
      })
        .on('form:submit', function () {
          return false; 
        });
    });

    const that = this;
    this.getConfiguracao();    
  }
}
