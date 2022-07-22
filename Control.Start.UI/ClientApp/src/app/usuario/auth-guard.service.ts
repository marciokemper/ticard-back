import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, CanLoad, Router, Route } from '@angular/router';
import { UsuarioService } from './usuario.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authService: UsuarioService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.checkLoggedIn(state.url);
  }

  //canLoad(route: Route): boolean {
  //  return this.checkLoggedIn(route.path);
  //}

  checkLoggedIn(url: string): boolean {
    /*if (this.authService .isLoggedIn()) {
        return true;
    }
    this.authService._redirectUrl = url;
    this.router.navigate(['/login']);
    return false;*/
    return true;
  }
}
