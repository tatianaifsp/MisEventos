import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { routing } from './sesion.routing';
import { SesionComponent } from './sesion.component';

import { RolesUsuarioComponent } from '../rol/rolesUsuario.component';
import { DataTableModule } from "angular2-datatable";
import { ToastrModule } from 'ngx-toastr';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { LoginComponent } from '../login/login.component';
const PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};


export const routes = [
  { path: '', component: SesionComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    PerfectScrollbarModule.forRoot(PERFECT_SCROLLBAR_CONFIG),
    ToastrModule.forRoot(),
    routing,
    FormsModule,
    ReactiveFormsModule,DataTableModule,
    RouterModule.forChild(routes),
  ],
  declarations: [SesionComponent, RolesUsuarioComponent, LoginComponent ]
})

export class SesionModule { }
