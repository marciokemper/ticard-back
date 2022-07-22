import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from "./common/auth.guard";
import { ConfiguracaoComponent } from "./configuracao/configuracao.component";
import { GraficosComponent } from "./graficos/graficos.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { FornecedorComponent } from "./fornecedor/fornecedor.component";
import { FornecedorEditComponent } from "./fornecedor/fornecedoredit.component";
import { UsuarioComponent } from "./usuario/usuario.component";
import { UsuarioEditComponent } from "./usuario/usuarioedit.component";
import { DocumentoComponent } from "./documento/documento.component";
import { EditComponent } from "./documento/edit.component";
import { EmpresaComponent } from "./empresa/empresa.component";
import { EmpresaEditComponent } from "./empresa/empresaedit.component";
import { EmpresaVinculaComponent } from "./vincula/empresavincula.component";
import { EmpresaFornecedoresComponent } from "./empresafornecedores/empresafornecedores.component";
import { ArquivoComponent } from "./arquivo/arquivo.component";



const AppRoutes: Routes = [
  { path: 'notafiscal', component: DocumentoComponent, canActivate: [AuthGuard] },
  { path: 'arquivo', component: ArquivoComponent, canActivate: [AuthGuard] },
  { path: 'notafiscal/:id', component: EditComponent, canActivate: [AuthGuard] },
  { path: 'vincula/:id', component: EmpresaVinculaComponent, canActivate: [AuthGuard] },
  { path: 'empresafornecedores/:id', component: EmpresaFornecedoresComponent, canActivate: [AuthGuard] },
  { path: 'fornecedor', component: FornecedorComponent, canActivate: [AuthGuard] },
  { path: 'fornecedor/:id', component: FornecedorEditComponent, canActivate: [AuthGuard] },
  { path: 'empresa', component: EmpresaComponent, canActivate: [AuthGuard] },
  { path: 'empresa/:id', component: EmpresaEditComponent, canActivate: [AuthGuard] },
  { path: 'configuracao', component: ConfiguracaoComponent, canActivate: [AuthGuard] },
  { path: 'graficos', component: GraficosComponent, canActivate: [AuthGuard] },
  { path: 'usuario', component: UsuarioComponent, canActivate: [AuthGuard] },
  { path: 'usuario/:id', component: UsuarioEditComponent, canActivate: [AuthGuard] },
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
]

@NgModule({
  imports: [
    RouterModule.forRoot(AppRoutes)
  ],
  exports: [RouterModule]
})
export class AppRountingModule { }
