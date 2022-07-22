import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IProfile } from '../models/profile';
import { UsuarioProfile } from './usuario/usuario.profile';
import { UsuarioService } from './usuario/usuario.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app works!';
  apiUrl: string = environment.apiUrl;

  loading: boolean = true;
  profile: IProfile;

  constructor(public authService: UsuarioService,private authProfile: UsuarioProfile,private router: Router) {
  }

  public ngOnInit() {

    this.profile = this.authProfile.usuarioProfile;    
  }

  logOut(): void {
   // this.authService.logout();

//    this.router.navigateByUrl('/login');
    this.router.navigate(['login']);
  }

}
