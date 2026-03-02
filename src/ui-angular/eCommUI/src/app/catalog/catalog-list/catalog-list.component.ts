import { Component, OnInit } from '@angular/core';
import { CatalogService } from '../catalog.service';
import { NgFor,CurrencyPipe } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
@Component({
  selector: 'app-catalog-list',
  standalone:true,
  imports: [NgFor,CurrencyPipe,RouterLink,RouterLinkActive],
  templateUrl: './catalog-list.component.html',
  styleUrls: ['./catalog-list.component.css']
})
export class CatalogListComponent implements OnInit {
  categories: any[] = [];
  products: any[] = [];
  selectedCategoryId: number | null = null;

  constructor(private catalogService: CatalogService) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadCategories() {
    this.catalogService.getCategories().subscribe(data => {
      this.categories = data;
    });
  }

  loadProducts(categoryId?: number) {
    this.catalogService.getProducts(categoryId).subscribe(data => {
      this.products = data;
    });
  }

  onCategorySelect(category: any) {
    this.selectedCategoryId = category.id;
    this.loadProducts(category.id);
  }
}
