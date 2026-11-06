import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { PurchaseOrdersService, SavedPurchaseOrder } from '../../core/services/purchase-orders.service';
import { Session } from '../../core/services/session';

interface BankAccount {
  name: string;
  system: string;
  updatedAgo: string;
  currency: string;
  amount: string;
  isNegative?: boolean;
  reviewed?: boolean;
}

interface ExpenseCategory {
  code: string;
  title: string;
  amount: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  // Live orders loaded directly from SQL Server Purchase_Orders & Purchase_Order_Items
  databaseOrders: SavedPurchaseOrder[] = [];
  loadingOrders: boolean = false;

  // Bank Accounts from design fix
  bankAccounts: BankAccount[] = [
    {
      name: 'Bank',
      system: 'In QuickBooks',
      updatedAgo: 'Updated 1176 days ago',
      currency: 'AED',
      amount: '-65,919.02',
      isNegative: true,
      reviewed: true
    },
    {
      name: '123.1234 PB_test',
      system: 'In QuickBooks',
      updatedAgo: 'Updated 300 days ago',
      currency: 'AED',
      amount: '4,567.00',
      isNegative: false,
      reviewed: false
    },
    {
      name: 'Cash and cash equivalents',
      system: 'In QuickBooks',
      updatedAgo: 'Updated 5 days ago',
      currency: 'AED',
      amount: '125,798.19',
      isNegative: false,
      reviewed: true
    },
    {
      name: 'Test Account',
      system: 'In QuickBooks',
      updatedAgo: 'Updated 100 days ago',
      currency: '€',
      amount: '200.00',
      isNegative: false,
      reviewed: false
    },
    {
      name: 'Master card 0011',
      system: 'In QuickBooks',
      updatedAgo: 'Updated 50 days ago',
      currency: 'AED',
      amount: '1,507.05',
      isNegative: false,
      reviewed: false
    }
  ];

  // Expenses breakdown from design
  totalExpenses = '$18,000.00';
  expenseCategories: ExpenseCategory[] = [
    {
      code: '6010',
      title: 'Online Marketing',
      amount: '$10,000.00',
      color: '#00a8cc'
    },
    {
      code: '6020',
      title: 'Subscriptions',
      amount: '$6,000.00',
      color: '#00508f'
    },
    {
      code: '6090',
      title: 'Depreciation',
      amount: '$1,100.00',
      color: '#1b2a47'
    },
    {
      code: '9090',
      title: 'Custom',
      amount: '$2,000.00',
      color: '#2ec4b6'
    }
  ];

  // Sidebar items
  sidebarNav = [
    { id: 'shortcuts', label: 'Shortcuts', icon: '⚡' },
    { id: 'core', label: 'Core', icon: '🧊' },
    { id: 'finance', label: 'Finance', icon: '💰' },
    { id: 'assets', label: 'Assets', icon: '📂' },
    { id: 'procurement', label: 'Procurement', icon: '🛒', action: 'purchase-bill', active: true },
    { id: 'inventory', label: 'Inventory', icon: '📦' },
    { id: 'manufacturing', label: 'Manufacturing', icon: '🏭' },
    { id: 'sales', label: 'Sales', icon: '🛍️' },
    { id: 'crm', label: 'CRM', icon: '👥' },
    { id: 'services', label: 'Services', icon: '🎧' },
    { id: 'docs', label: 'Docs', icon: '📄' },
    { id: 'analytics', label: 'Analytics', icon: '📈' },
    { id: 'admin', label: 'Admin', icon: '⚙️' }
  ];

  constructor(
    private router: Router,
    private purchaseOrdersService: PurchaseOrdersService,
    private session: Session
  ) { }

  ngOnInit(): void {
    this.loadDatabaseOrders();
  }

  loadDatabaseOrders(): void {
    this.loadingOrders = true;
    this.purchaseOrdersService.getPurchaseOrders().subscribe({
      next: (orders) => {
        this.databaseOrders = orders;
        this.loadingOrders = false;
        // If there are real database orders, calculate or update total expenses if available
        if (orders && orders.length > 0) {
          const totalFromDb = orders.reduce((sum, o) => sum + (o.netAmount || 0), 0);
          if (totalFromDb > 0) {
            this.totalExpenses = `$${totalFromDb.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
          }
        }
      },
      error: (err) => {
        console.error('Failed to load purchase orders from backend:', err);
        this.loadingOrders = false;
      }
    });
  }

  onSidebarClick(item: any): void {
    if (item.action === 'purchase-bill' || item.id === 'procurement' || item.id === 'shortcuts') {
      this.goToPurchaseBill();
    }
  }

  goToPurchaseBill(): void {
    this.router.navigate(['/purchase-bill']);
  }

  logout(): void {
    this.session.logout();
    this.router.navigate(['/']);
  }
}
