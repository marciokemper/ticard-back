import { Component, OnInit } from '@angular/core';
import { IProfile } from '../../models/profile';
import { UsuarioService } from '../usuario/usuario.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu-extras',
  templateUrl: './menu-extras.component.html'
})


export class MenuExtrasComponent implements OnInit {
  Profile: IProfile;
  public notifs = [];
  public notificacoes: number = 0;
  public qtdRequisicoes: number = 0;
  public qtdFaturasAtrasadas: number = 0;
  public qtdFaturasHoje: number = 0;
  public qtdErroContrato: number = 0;
  public qtdRobos: number = 0;
  isAdministradorBosch: boolean;

  constructor(private authService: UsuarioService,
    //private service: ContaColetadaService,
    private authProfile: UsuarioProfile, private router: Router) {
  }

  public getNotificacoes(): void {
    this.notifs = [];
    //this.service.getNotificacoes()
    //  .subscribe((res) => {
    //    if (res.success) {
    //      this.notifs = res.result;
    //      this.qtdRequisicoes = this.notifs[0].QtdRequisicoes;
    //      this.qtdFaturasAtrasadas = this.notifs[0].QtdFaturasAtrasadas;
    //      this.qtdFaturasHoje = this.notifs[0].QtdFaturasHoje;
    //      this.qtdRobos = this.notifs[0].QtdRobos;          
    //      this.qtdErroContrato = this.notifs[0].QtdErroContrato;
    //      this.notificacoes = this.qtdRequisicoes + this.qtdFaturasAtrasadas + this.qtdFaturasHoje + this.qtdErroContrato;
    //    }
    //  });
  }

  public requisicoes(e): void {
    e.preventDefault();
    this.router.navigate(['requisicao', { situacao: '3,4' }]);
  }

  public logins(e): void {
    e.preventDefault();
    this.router.navigate(['log', { login: '2' }]);
  }

  public atrasadas(e): void {
    e.preventDefault();

    this.router.routeReuseStrategy.shouldReuseRoute = function () { return false; };

    let currentUrl = this.router.url + '?';

    this.router.navigateByUrl(currentUrl)
      .then(() => {
        this.router.navigated = false;
        this.router.navigate(['contacoletada', { situacao: '1' }]);
      });

  }

  public hoje(e): void {
    e.preventDefault();

    this.router.routeReuseStrategy.shouldReuseRoute = function () { return false; };

    let currentUrl = this.router.url + '?';

    this.router.navigateByUrl(currentUrl)
      .then(() => {
        this.router.navigated = false;
        this.router.navigate(['contacoletada', { situacao: '2' }]);
      });

  }

  public problemas(e): void {
    e.preventDefault();
    this.router.navigate(['contacoletada', { situacao: '3' }]);
  }

  ngOnInit(): void {

    this.Profile = this.authProfile.usuarioProfile;
   // if (this.Profile.currentUsuario.empresaId != null && this.Profile.currentUsuario.empresaId == 1)
   //   this.isAdministradorBosch = true;
   // else
    //  this.isAdministradorBosch = false;

   // this.getNotificacoes();
  }

  logOut(): void {
    this.authService.logout();
  }

}

