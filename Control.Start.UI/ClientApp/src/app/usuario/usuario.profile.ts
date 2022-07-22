import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import { IProfile } from '../../models/profile';

@Injectable()
export class UsuarioProfile {
  usuarioProfile: IProfile = {
    token: "",
    expiration: "",
    currentUsuario: { id: null, nome: '', login: '', email: '', senha: '', confirmaSenha: '', ativo: true, empresaFornecedor: null, empresaFornecedorId: null},
    claims: null
  };


  constructor(private router: Router) {
  }

  setProfile(profile: IProfile): void {
    var nameid = profile.claims.filter(p => p.type == 'nameid')[0].value;
    var name = profile.claims.filter(p => p.type == 'sub')[0].value;
    var userName = profile.claims.filter(p => p.type == 'email')[0].value;
    var fornecedor = profile.claims.filter(p => p.type == 'acr')[0].value;

    sessionStorage.setItem('access_token', profile.token);
    sessionStorage.setItem('expires_in', profile.expiration);
    sessionStorage.setItem('nameid', nameid);
    sessionStorage.setItem('userName', userName);
    sessionStorage.setItem('nome', name);
    sessionStorage.setItem('fornecedor', fornecedor);
  }

  getProfile(authorizationOnly: boolean = false): IProfile {
    var accessToken = sessionStorage.getItem('access_token');

    if (accessToken) {
      this.usuarioProfile.token = accessToken;
      this.usuarioProfile.expiration = sessionStorage.getItem('expires_in');
      if (this.usuarioProfile.currentUsuario == null) {
        this.usuarioProfile.currentUsuario.empresaFornecedorId = Number(sessionStorage.getItem('fornecedor'));
        this.usuarioProfile.currentUsuario = { id: null, nome: '', login: '', email: '', senha: '', confirmaSenha: '', ativo: true, empresaFornecedor: null, empresaFornecedorId: null }
      }
      this.usuarioProfile.currentUsuario.id = Number(sessionStorage.getItem('nameid'));

      this.usuarioProfile.currentUsuario.email = sessionStorage.getItem('userName');
      this.usuarioProfile.currentUsuario.nome = sessionStorage.getItem('nome');
      this.usuarioProfile.currentUsuario.empresaFornecedorId = Number(sessionStorage.getItem('fornecedor'));

    }

    return this.usuarioProfile;
  }

  resetProfile(): IProfile {
    sessionStorage.removeItem('access_token');
    sessionStorage.removeItem('expires_in');
    this.usuarioProfile = {
      token: "",
      expiration: "",
      currentUsuario: null,
      claims: null
    };
    return this.usuarioProfile;
  }
}


