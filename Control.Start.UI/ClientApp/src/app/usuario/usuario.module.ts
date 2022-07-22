import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../common/auth.guard';
import { LoginComponent } from '../login/login.component';
import { SharedModule } from '../shared/shared.module';
import { SignupComponent } from '../signup/signup.component';
import { UsuarioProfile } from './usuario.profile';
import { UsuarioService } from './usuario.service';

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent },
    ])
  ],
  declarations: [
    LoginComponent,
    SignupComponent
  ],
  providers: [
    UsuarioService,
    AuthGuard,
    UsuarioProfile
  ]
})
export class UsuarioModule { }
