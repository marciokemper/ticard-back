import { Injectable, Inject } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { GenericResult, GenericSimpleResult } from '../../models/genericResult';
import { IProfile } from '../../models/profile';
import { Usuario } from '../../models/usuario';
import { contentHeaders } from '../common/headers';
import { CommonService } from '../shared/common.service';
import { UsuarioProfile } from './usuario.profile';
import { Router } from '@angular/router';
import { NgxCoolDialogsService } from 'ngx-cool-dialogs';

@Injectable()
export class UsuarioService {
  redirectUrl: string;
  errorMessage: string;
  private headersx: Headers;
  public _baseUrl: string;  

  constructor(@Inject('BASE_URL') baseUrl: string, private authProfile: UsuarioProfile, private commonService: CommonService, private http: Http, private coolDialogs: NgxCoolDialogsService, private router: Router) {
    this.headersx = new Headers({
      "Authorization": "Bearer " + this.authProfile.usuarioProfile.token,
      "Content-Type": "application/json"
    });
    this._baseUrl = baseUrl; 
  }

  public getById(id: number): Observable<GenericResult<Usuario>> {

    return this.http.get(this.commonService.getBaseUrl() + '/Usuario/' + id, { headers: this.headersx })
      .map(res => res.json());
  }

  public inserir(usuario: Usuario): Observable<GenericResult<Usuario>> {

    return this.http.post(this.commonService.getBaseUrl() + '/Usuario/Incluir', JSON.stringify(usuario), {
      headers: this.headersx
    }).map(res => res.json());
  }

  public put(id: number, usuario: Usuario): Observable<GenericSimpleResult> {

    return this.http.put(this.commonService.getBaseUrl() + '/Usuario/' + id, JSON.stringify(usuario), { headers: this.headersx })
      .map(res => res.json());

  }

  public delete(id: number): Observable<GenericSimpleResult> {
    return this.http.delete(this.commonService.getBaseUrl() + '/Usuario/' + id, { headers: this.headersx })
      .map(res => res.json());
  }

  isVisible() {
    let profile = this.authProfile.getProfile();
    var validToken = profile.token != "" && profile.token != null;
    var isTokenExpired = this.isTokenExpired(profile);
    return validToken && !isTokenExpired;

  }


  isAuthenticated() {
    var isTokenExpired = true;
    let profile = this.authProfile.getProfile();
    var validToken = profile.token != "" && profile.token != null;

    if (validToken) {
      isTokenExpired = this.isTokenExpired(profile);
      if (isTokenExpired) {
        this.authProfile.resetProfile();
        this.coolDialogs.alert('Desculpe, sua sessão expirou!');
        this.router.navigate(['/login']);
      }

    }

    return validToken && !isTokenExpired;
  }

  isAuthorized() {
    let profile = this.authProfile.getProfile();
    var validToken = profile.token != "" && profile.token != null;
    var isTokenExpired = this.isTokenExpired(profile);
    return validToken && !isTokenExpired;
  }

  isTokenExpired(profile: IProfile) {
    var expiration = new Date(profile.expiration)
    var currentDate = new Date();
    return currentDate > expiration ;
  }

  login(userName: string, password: string) {
    if (!userName || !password) {
      return;
    }
    let options = new RequestOptions(
      { headers: contentHeaders });

    var credentials = {
      grant_type: 'password',
      usuarioEmail: userName,
      usuarioSenha: password
    };
    let url = this.commonService.getBaseUrl() + '/auth/token';

    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http.post(url, JSON.stringify(credentials), { headers: headers })
      .map(res => {
        var userProfile: IProfile = res.json();
        this.authProfile.setProfile(userProfile);
        return res.json()
      }
      ).catch(this.commonService.handleFullError);

  }

  loginP(userName: string, password: string) {
    if (!userName || !password) {
      return;
    }
    let options = new RequestOptions(
      { headers: contentHeaders });

    var credentials = {
      grant_type: 'password',
      login: userName,
      senha: password
    };

    let url = this.commonService.getBaseUrl() + '/auth/login';

    console.log(url)

    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http.post(url, JSON.stringify(credentials), { headers: headers })
      .map(res => {
        var userProfile: IProfile = res.json();
        this.authProfile.setProfile(userProfile);
        return res.json()
      }
      ).catch(this.commonService.handleFullError);

  }

  loginReset(id: number,userName: string, password: string) {
    if (!userName || !password) {
      return;
    }
    let options = new RequestOptions(
      { headers: contentHeaders });

    var credentials = {
      grant_type: 'password',
      id: id,
      email: userName,
      senha: password
    };
    let url = this.commonService.getBaseUrl() + '/auth/loginreset';

    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http.post(url, JSON.stringify(credentials), { headers: headers })
      .map(res => {
        var userProfile: IProfile = res.json();
        this.authProfile.setProfile(userProfile);
        return res.json()
      }
      ).catch(this.commonService.handleFullError);

  }

  register(userName: string, password: string, confirmPassword: string) {
    if (!userName || !password) {
      return;
    }
    let options = new RequestOptions(
      { headers: contentHeaders });

    var credentials = {
      email: userName,
      password: password,
      confirmPassword: confirmPassword
    };
    let url = this.commonService.getBaseUrl() + '/auth/register';
    return this.http.post(url, credentials, options)
      .map((response: Response) => {
        return response.json();
      }).catch(this.commonService.handleFullError);
  }

  logout(): void {
    this.authProfile.resetProfile();
    this.router.navigate(['login']);
  }
}


