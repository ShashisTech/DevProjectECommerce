import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { ProductDetailComponent } from './catalog/product-detail/product-detail.component';
import { ShoppingComponent } from './shopping/shopping.component';

const routes: Routes = [
  { path: '', component: ShoppingComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'products/:id', component: ProductDetailComponent },
  { path: 'shopping', component: ShoppingComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
