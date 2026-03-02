
import { Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { ProductDetailComponent } from './catalog/product-detail/product-detail.component';
import { CatalogListComponent } from './catalog/catalog-list/catalog-list.component';
import { ShoppingComponent } from './shopping/shopping.component';
export const routes: Routes = [
  { path: '', component: ProductsComponent },
  { path: 'Category', component: CatalogListComponent },
  { path: 'Product/:id', component: ProductDetailComponent },
  { path: 'Shopping', component: ShoppingComponent }
  // Wildcard route for a 404 page
  //{ path: '**', component: NotFoundComponent }
];