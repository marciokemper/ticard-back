import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UsuarioService } from '../usuario/usuario.service';
import { UsuarioProfile } from '../usuario/usuario.profile';
import { IProfile } from '../../models/profile';


@Injectable()
export class AuthGuard implements CanActivate {

  profile: IProfile;
  constructor(private router: Router, private authService: UsuarioService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isAuthorized()) {
      return true;
    }

    this.authService.redirectUrl = state.url;
    this.router.navigate(['/login']);
    return false;
  }
}
