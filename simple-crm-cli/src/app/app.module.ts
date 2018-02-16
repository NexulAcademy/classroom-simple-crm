import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { SharedImportsModule } from './shared/shared-imports.module';
import { CustomerModule } from './customer/customer.module';
import { AppIconsService } from './app-icons.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedImportsModule,
    CustomerModule
  ],
  providers: [
    AppIconsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  // simply passing in the icon service, instantiates it and registers its icons
  constructor(iconService: AppIconsService) {}
 }
