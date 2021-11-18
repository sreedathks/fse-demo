import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { ProductEndpoint } from './product-endpoint.service';
import { AuthService } from './auth.service';
import {Product} from '../models/product.model';

@Injectable()
export class ProductService {

    constructor(private router: Router, private http: HttpClient, private authService: AuthService,
        private productEndpoint: ProductEndpoint) {
    
      }

      getAllProducts() {
        return this.productEndpoint.getAllproductsEndpoint<Product[]>();
      }

      getProductDetails(id) {
        return this.productEndpoint.getProductEndpoint<Product>(id);
      }
}