import { Component, OnInit } from '@angular/core';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { IProfile } from '../../models/profile';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html'
})


export class NavMenuComponent implements OnInit {
  Profile: IProfile;
  isAdmin: boolean;
  public balance: string = '';
  constructor(private authProfile: UsuarioProfile,
  ) {
    this.balance = '';
  }

  public ngOnInit() {
    this.Profile = this.authProfile.usuarioProfile;

    this.isAdmin = false;
    if (this.Profile.currentUsuario.empresaFornecedorId == 0)
      this.isAdmin = true;
   
  }
}
