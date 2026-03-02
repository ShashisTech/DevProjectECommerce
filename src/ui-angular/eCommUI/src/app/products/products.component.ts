import { Component, OnInit } from '@angular/core';
import { CatalogService } from '../catalog/catalog.service';
import { NgIf ,NgFor,CurrencyPipe} from '@angular/common';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [NgIf, NgFor,CurrencyPipe],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  loading = false;
  error: string | null = null;

  constructor(private catalogService: CatalogService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    debugger;
    this.loading = true;
    this.error = null;
    this.catalogService.getAllProducts().subscribe({
      next: data => {
        this.products = data;
        this.loading = false;
      },
      error: err => {
        this.error = 'Failed to load products.';
        console.error(err);
        this.loading = false;
      }
    });
  }
}
