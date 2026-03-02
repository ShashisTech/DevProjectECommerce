import { Component, OnInit } from '@angular/core';
import { CatalogService } from '../catalog/catalog.service';
import { NgIf,NgFor,CurrencyPipe } from '@angular/common';

interface CategoryGroup {
  categoryName: string;
  products: any[];
}

@Component({
  selector: 'app-shopping',
  standalone: true,
  imports: [NgIf,NgFor,CurrencyPipe],
  templateUrl: './shopping.component.html',
  styleUrls: ['./shopping.component.css']
})
export class ShoppingComponent implements OnInit {
  groups: CategoryGroup[] = [];
  loading = false;
  error: string | null = null;

  constructor(private catalogService: CatalogService) {}

  ngOnInit(): void { debugger;
    this.loadData();
  }

  loadData(): void {
    debugger;
    this.loading = true;
    this.error = null;

    this.catalogService.getAllProducts().subscribe({
      next: products => {
        const byCategory: { [key: string]: any[] } = {};
        for (const p of products) {
          const name = p.categoryName || p.category?.name || 'Other';
          if (!byCategory[name]) {
            byCategory[name] = [];
          }
          byCategory[name].push(p);
        }
        this.groups = Object.keys(byCategory).map(name => ({
          categoryName: name,
          products: byCategory[name]
        }));
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load products.';
        this.loading = false;
      }
    });
  }
}
