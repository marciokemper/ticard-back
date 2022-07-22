import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxCoolDialogsService } from 'ngx-cool-dialogs';
import { Usuario } from '../../models/usuario';
import { UsuarioService } from '../usuario/usuario.service';
import { LoadingService } from '../common/loading.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { UsuarioLogin } from '../../models/usuariologin';
declare var $: any;
@Component({
  templateUrl: './login.component.html',
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
export class LoginComponent implements OnInit {
  errorMessage: string;
  pageTitle = 'Entrar';
  public usuario: UsuarioLogin = new UsuarioLogin();

  constructor(private authService: UsuarioService,
    private router: Router,
    private authProfile: UsuarioProfile,
    private coolDialogs: NgxCoolDialogsService,
    private loadingService: LoadingService) {

  }


  public login(): void {
    let userName = this.usuario.login;
    let password = this.usuario.senha;

    if (userName == '' || password == '' || password == undefined)
      return;

    this.loadingService.Show();

    this.authService.loginP(userName, password)
      .subscribe(
        response => {
          this.loadingService.Hide();
          this.router.navigate(['/dashboard']);
        },
        error => {
          this.loadingService.Hide();
          console.log(`teste - ${error}`)
          this.coolDialogs.alert('Usuário/Senha Inválidos!', {
            theme: 'material',
            okButtonText: 'Sair',
            color: 'red',
            title: 'Erro'
          });
        }
      );

  }

  public ngOnInit() {



  }
}


