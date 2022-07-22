import { HttpClientModule } from '@angular/common/http';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxCoolDialogsModule } from 'ngx-cool-dialogs';
import { AppComponent } from './app.component';
import { AppRountingModule } from './app.routing.module';
import { DialogConfirmService } from './common/dialogconfirm.service';
import { ConfiguracaoComponent } from './configuracao/configuracao.component';
import { ConfiguracaoService } from './configuracao/configuracao.service';
import { GraficosComponent } from './graficos/graficos.component';
import { GraficosService } from './graficos/graficos.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FornecedorComponent } from './fornecedor/fornecedor.component';
import { FornecedorService } from './fornecedor/fornecedor.service';
import { FornecedorEditComponent } from './fornecedor/fornecedoredit.component';
import { MenuExtrasComponent } from './menu-extras/menu-extras.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CommonService } from './shared/common.service';
import { UsuarioModule } from './usuario/usuario.module';
import { UsuarioEditComponent } from './usuario/usuarioedit.component';
import { UsuarioComponent } from './usuario/usuario.component';
import { DataTablesModule } from 'angular-datatables';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { CepService } from './shared/cep.service';
import { LoadingService } from './common/loading.service';
import { UtilsService } from './common/utils.service';
import { NgxMaskModule, IConfig  } from 'ngx-mask'
import { SelectModule } from 'ng-select';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import { DropDownListAllModule } from '@syncfusion/ej2-angular-dropdowns';
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { FullCalendarModule } from '@fullcalendar/angular'
import { DatePipe } from '@angular/common';
import { TooltipModule } from 'ng2-tooltip-directive';
import { DocumentoComponent } from "./documento/documento.component";
import { EmpresaEditComponent } from './empresa/empresaedit.component';
import { EmpresaComponent } from './empresa/empresa.component';
import { EmpresaService } from './empresa/empresa.service';
import { EditComponent } from "./documento/edit.component";
import { DocumentoService } from "./documento/documento.service";
import { EmpresaVinculaComponent } from './vincula/empresavincula.component';
import { EmpresavinculaService } from './vincula/empresavincula.service';
import { EmpresaFornecedoresComponent } from './empresafornecedores/empresafornecedores.component';
import { EmpresaFornecedoresService } from './empresafornecedores/empresafornecedores.service';
import { ArquivoComponent } from './arquivo/arquivo.component';
import { ArquivoService } from './arquivo/arquivo.service';

registerLocaleData(localePt);

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DocumentoComponent,
    EmpresaVinculaComponent,
    EditComponent,
    EmpresaFornecedoresComponent,
    DashboardComponent,
    MenuExtrasComponent,
    FornecedorEditComponent,
    EmpresaEditComponent,
    UsuarioEditComponent,
    FornecedorComponent,
    EmpresaComponent,
    ConfiguracaoComponent,
    GraficosComponent,
    UsuarioComponent,
    UsuarioEditComponent,
    ArquivoComponent
  ],
  imports: [
    UsuarioModule,
    BrowserAnimationsModule,
    NgxCoolDialogsModule.forRoot(),
    NgxMaskModule.forRoot(maskConfig),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    DataTablesModule,
    AppRountingModule,
    SelectModule,
    DropDownListAllModule,
    DialogModule,
    GridAllModule,
    TooltipModule,
    FullCalendarModule
  ],
  providers: [
    DocumentoService,
    EmpresavinculaService,
    ConfiguracaoService,
    EmpresaFornecedoresService,
    GraficosService,
    ArquivoService,
    FornecedorService,
    EmpresaService,
    CommonService,
    CepService,
    DialogConfirmService,
    LoadingService,
    UtilsService,
    DatePipe,
      { provide: LocationStrategy, useClass: HashLocationStrategy, useValue: 'pt-BR' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
