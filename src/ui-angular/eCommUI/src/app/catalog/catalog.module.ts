import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CatalogListComponent } from './catalog-list/catalog-list.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';

@NgModule({
  declarations: [
    CatalogListComponent,
    ProductDetailComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    CatalogListComponent,
    ProductDetailComponent
  ]
})
export class CatalogModule { }
