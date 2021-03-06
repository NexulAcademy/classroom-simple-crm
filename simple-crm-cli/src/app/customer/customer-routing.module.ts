import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from '../app.component';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';

const routes: Routes = [
  {
    path: 'customers',
    pathMatch: 'full',
    component: CustomerListPageComponent
  },
  {
    path: 'customer/:id',
    pathMatch: 'full',
    component: CustomerDetailComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule {}
