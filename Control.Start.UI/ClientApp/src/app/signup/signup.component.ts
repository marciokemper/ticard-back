import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { UsuarioService } from '../usuario/usuario.service';
declare var jQuery: any; 
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
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


export class SignupComponent {
  errorMessage: string;
  pageTitle = 'signup';

  constructor(private authService: UsuarioService,
    private router: Router) { }

  register(signupForm: NgForm) {
    if (signupForm && signupForm.valid) {
      let userName = signupForm.form.value.userName;
      let password = signupForm.form.value.password;
      let confirmPassword = signupForm.form.value.confirmPassword;
      var result = this.authService.register(userName, password, confirmPassword)
        .subscribe(
          response => {
            if (this.authService.redirectUrl) {
              this.router.navigateByUrl(this.authService.redirectUrl);
            } else {
              this.router.navigate(['/']);
            }
          },
          error => {
            var results = error['_body'];
            this.errorMessage = error.statusText + ' ' +

              error.text();
          }
        );
    } else {
      this.errorMessage = 'Por favor insira um nome de usuário e senha.';
    };
  }
}
