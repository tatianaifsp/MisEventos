import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
const PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};
import { ToastrModule } from 'ngx-toastr';
import { DirectivesModule } from '../theme/directives/directives.module';
import { PipesModule } from '../theme/pipes/pipes.module';
import { SharedModule } from './shared/shared.module';

import { routing } from './pages.routing';
import { PagesComponent } from './pages.component';
import { BlankComponent } from './blank/blank.component';
import { MenuComponent } from '../theme/components/menu/menu.component';
import { SidebarComponent } from '../theme/components/sidebar/sidebar.component';
import { NavbarComponent } from '../theme/components/navbar/navbar.component';
import { MessagesComponent } from '../theme/components/messages/messages.component';
import { BreadcrumbComponent } from '../theme/components/breadcrumb/breadcrumb.component';
import { BackTopComponent } from '../theme/components/back-top/back-top.component';
import { SearchComponent } from './search/search.component';
import { SociosComponent } from './socios/socios.component';
import { DataTableModule } from "angular2-datatable";
import { EspaciosComponent } from './espacios/espacios.component';
import { TipoEspacioComponent } from './tipoEspacio/tipoEspacio.component';
import { DomicilioComponent } from './domicilio/domicilio.component';
import { ContactoComponent } from './contacto/contacto.component';
import { EncargadosComponent } from './encargados/encargados.component';
import { ContactoModelComponent } from './contacto-model/contacto-model.component';
import { RolComponent } from './rol/rol.component';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { UsuarioComponent } from './usuarios/usuarios.component';
import { MatriculaComponent } from './matricula/matricula.component';
import { NuevaMatriculaComponent } from './matricula/nueva-matricula/nueva-matricula.component';
import { CobroMatriculaComponent } from './cobro-matricula/cobro-matricula.component';
import { MatriculasPagadasComponent } from './matricula/matriculas-pagadas/matriculas-pagadas.component';

@NgModule({
  imports: [
    CommonModule,
    PerfectScrollbarModule.forChild(),
    ToastrModule.forRoot(),
    DirectivesModule,
    PipesModule,
    routing,
    FormsModule,
    DataTableModule,
    NguiAutoCompleteModule,
    SharedModule
  ],
  declarations: [
    PagesComponent,
    BlankComponent,
    MenuComponent,
    SidebarComponent,
    NavbarComponent,
    MessagesComponent,
    BreadcrumbComponent,
    BackTopComponent,
    SearchComponent,
    SociosComponent,
    EspaciosComponent,
    TipoEspacioComponent,
    DomicilioComponent,
    ContactoComponent,
    EncargadosComponent,
    RolComponent,
    ContactoModelComponent,
    UsuarioComponent,
    MatriculaComponent,
    NuevaMatriculaComponent,
    CobroMatriculaComponent,
    MatriculasPagadasComponent
  ]
  , exports: [
    BreadcrumbComponent,
    BackTopComponent,
    SociosComponent,
    EspaciosComponent,
    TipoEspacioComponent,
    DomicilioComponent,
    ContactoComponent,
    RolComponent,
    EncargadosComponent,
    ContactoModelComponent
  ]
})

export class PagesModule { }
