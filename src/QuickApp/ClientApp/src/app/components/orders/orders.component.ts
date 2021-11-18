// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { Component, Inject, OnInit } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import {Product} from '../../models/product.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ProductService } from 'src/app/services/product-service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  animations: [fadeInOut]
})
export class OrdersComponent implements OnInit{
  public products : Product[];
  public search_product: Product;
  public sel_prodoct: Product;
  _productService: ProductService;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, prodcutservice: ProductService) {
    this._productService = prodcutservice;
    prodcutservice.getAllProducts().subscribe( results => {

      this.products = results;    
      this.sel_prodoct =  results[0];  
    });
      
  }
  ngOnInit() {
    this.fetchProducts();
  }

  GetProduct() {
    //this.search_product = this.products.find(p=> (p.id===this.sel_prodoct.id));
    this._productService.getProductDetails(this.sel_prodoct.id).subscribe ( results =>
      this.search_product = results
      );
  }

  ChangingValue(value) {

    this.sel_prodoct = value;

  }

  fetchProducts()
  {
    //const prducts[]: Product[];
  }
}
