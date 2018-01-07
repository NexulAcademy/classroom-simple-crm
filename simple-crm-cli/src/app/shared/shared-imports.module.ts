import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatToolbarModule,
  MatButtonModule,
  MatIconModule,
  MatSidenavModule,
  MatListModule
} from '@angular/material';

export const SHARED_MATERIAL_MODULES = [
  MatToolbarModule,
  MatButtonModule,
  MatIconModule,
  MatSidenavModule,
  MatListModule
];

@NgModule({
  imports: [
    CommonModule,
    ...SHARED_MATERIAL_MODULES
  ],
  exports: [
    ...SHARED_MATERIAL_MODULES
  ],
  declarations: []
})
export class SharedImportsModule { }
