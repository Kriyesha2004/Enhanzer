import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface PurchaseOrderItemInput {
  item: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

export interface CreatePurchaseOrderPayload {
  orderDate?: string;
  netAmount?: number;
  items: PurchaseOrderItemInput[];
}

export interface SavedOrderItem {
  purchaseOrderItemId: number;
  purchaseOrderId: number;
  itemName: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  quantity: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

export interface SavedPurchaseOrder {
  purchaseOrderId: number;
  orderDate: string;
  netAmount: number;
  totalItems: number;
  totalQuantity: number;
  createdAt: string;
  items: SavedOrderItem[];
}

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrdersService {
  private apiUrl = `${environment.apiUrl}/PurchaseOrders`;

  constructor(private http: HttpClient) { }

  getPurchaseOrders(): Observable<SavedPurchaseOrder[]> {
    return this.http.get<SavedPurchaseOrder[]>(this.apiUrl);
  }

  savePurchaseOrder(payload: CreatePurchaseOrderPayload): Observable<SavedPurchaseOrder> {
    return this.http.post<SavedPurchaseOrder>(this.apiUrl, payload);
  }
}

