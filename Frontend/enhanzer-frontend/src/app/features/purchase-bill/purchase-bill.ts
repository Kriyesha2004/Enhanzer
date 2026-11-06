
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AppLocation, Locations } from '../../core/services/locations';
import { Router } from '@angular/router';
import { PurchaseOrdersService, CreatePurchaseOrderPayload } from '../../core/services/purchase-orders.service';

interface BillItem {
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

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css'
})
export class PurchaseBill implements OnInit {

  // Item form fields
  item = '';
  batch = '';

  standardCost = 100;
  standardPrice = 150;
  quantity = 1;
  freeQty = 0;
  discount = 0;

  // Available locations
  locations: AppLocation[] = [];

  // Added bill items
  billItems: BillItem[] = [];

  // Payment & Save status
  savingOrder = false;
  orderSuccessMessage = '';
  orderErrorMessage = '';

  constructor(
    private locationsService: Locations,
    private purchaseOrdersService: PurchaseOrdersService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadLocations();
  }

  goToLogin(): void {
    this.router.navigate(['/']);
  }

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  // Load batch names from backend
  loadLocations(): void {
    this.locationsService.getLocations().subscribe({
      next: (response) => {
        this.locations = response;
        console.log('Locations loaded:', response);
      },
      error: (error) => {
        console.error('Failed to load locations:', error);
      }
    });
  }

  // Calculate total quantity
  get totalQuantity(): number {
    return this.billItems.reduce(
      (total, current) => total + current.quantity,
      0
    );
  }

  // Calculate total cost
  get totalCost(): number {
    return this.billItems.reduce(
      (total, current) => total + current.totalCost,
      0
    );
  }

  // Calculate total selling
  get totalSelling(): number {
    return this.billItems.reduce(
      (total, current) => total + current.totalSelling,
      0
    );
  }

  // Add item to purchase bill
  addItem(): void {

    if (
      !this.item.trim() ||
      !this.batch ||
      this.quantity <= 0 ||
      this.standardCost < 0 ||
      this.standardPrice < 0 ||
      this.discount < 0 ||
      this.discount > 100
    ) {
      alert('Please enter valid item details.');
      return;
    }

    const grossCost = this.standardCost * this.quantity;

    const discountAmount = grossCost * (this.discount / 100);

    const calculatedTotalCost = grossCost - discountAmount;

    const calculatedTotalSelling =
      this.standardPrice * this.quantity;

    const newItem: BillItem = {
      item: this.item,
      batch: this.batch,
      standardCost: this.standardCost,
      standardPrice: this.standardPrice,
      quantity: this.quantity,
      freeQty: this.freeQty,
      discount: this.discount,
      totalCost: calculatedTotalCost,
      totalSelling: calculatedTotalSelling
    };

    this.billItems.push(newItem);

    // Reset form
    this.item = '';
    this.batch = '';
    this.standardCost = 100;
    this.standardPrice = 150;
    this.quantity = 1;
    this.freeQty = 0;
    this.discount = 0;
  }

  // Save bill items to Purchase_Orders and Purchase_Order_Items tables in SQL Server
  payAndSaveBill(): void {
    if (this.billItems.length === 0) {
      alert('Please add at least one item before saving.');
      return;
    }

    this.savingOrder = true;
    this.orderSuccessMessage = '';
    this.orderErrorMessage = '';

    const payload: CreatePurchaseOrderPayload = {
      orderDate: new Date().toISOString(),
      netAmount: this.totalCost,
      items: this.billItems.map(item => ({
        item: item.item,
        batch: item.batch,
        standardCost: item.standardCost,
        standardPrice: item.standardPrice,
        quantity: item.quantity,
        freeQty: item.freeQty,
        discount: item.discount,
        totalCost: item.totalCost,
        totalSelling: item.totalSelling
      }))
    };

    this.purchaseOrdersService.savePurchaseOrder(payload).subscribe({
      next: (order) => {
        this.savingOrder = false;
        this.orderSuccessMessage = `Order #${order.purchaseOrderId} saved successfully! Total: $${order.netAmount.toFixed(2)}`;
        this.billItems = [];

        // Redirect to eZuite Dashboard after short delay
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
        }, 1500);
      },
      error: (err) => {
        this.savingOrder = false;
        console.error('Failed to save purchase bill:', err);
        this.orderErrorMessage = err.error?.message || 'Failed to save purchase order to database.';
      }
    });
  }
}
