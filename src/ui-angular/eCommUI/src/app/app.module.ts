import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HeaderComponent } from './layout/header/header.component';
import { FooterComponent } from './layout/footer/footer.component';
import { CatalogModule } from './catalog/catalog.module';
import { ProductsComponent } from './products/products.component';
import { ShoppingComponent } from './shopping/shopping.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    ProductsComponent,
    ShoppingComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    AppRoutingModule,
    CatalogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }