import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';

@Injectable()
export class ProductEndpoint extends EndpointFactory {


    private readonly _getallproductsUrl: string = '/api/product/getallproducts';
  private readonly _getproductUrl: string = '/api/product/getproduct';

  get allproductsUrl() { return this.configurations.apimUrl + this._getallproductsUrl; }
  get getproductUrl() { return this.configurations.apimUrl + this._getproductUrl; }

  constructor(http: HttpClient, configurations: ConfigurationService, injector: Injector) {

    super(http, configurations, injector);
  }

  getAllproductsEndpoint<T>(): Observable<T> {
    const endpointUrl = this.allproductsUrl;
    return this.http.get<T>(endpointUrl, this.getRequestHeaders()).pipe<T>(
      catchError(error => {
        return this.handleError(error, () => this.getAllproductsEndpoint());
      }));
    // return this.http.get<T>(endpointUrl, this.getRequestHeaders_apim()).pipe<T>(
    //     catchError(error => {
    //       return this.handleError(error, () => this.getAllproductsEndpoint());
    //     }));
  }

  getProductEndpoint<T>(id: number): Observable<T> {
    const endpointUrl = `${this.getproductUrl}/${id}`;
    return this.http.get<T>(endpointUrl, this.getRequestHeaders()).pipe<T>(
      catchError(error => {
        return this.handleError(error, () => this.getProductEndpoint(id));
      }));
    }
}