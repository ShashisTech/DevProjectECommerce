import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {CatalogService} from '../catalog.service';
import { NgFor,CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product-detail',
   standalone:true,
  imports:  [NgFor,CurrencyPipe],
   templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  product: any;

  constructor(
    private route: ActivatedRoute,
    private catalogService: CatalogService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.catalogService.getProductById(id).subscribe(p => this.product = p);
    }
  }
}
