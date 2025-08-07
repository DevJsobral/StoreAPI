import { OrdersService, OrderItem } from '../../services/orders.service';
import { InfoModalComponent } from '../info-modal/info-modal';
import { Component, OnInit, signal, ViewEncapsulation, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductsService, Product } from '../../services/products.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, InfoModalComponent],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css'],
  encapsulation: ViewEncapsulation.None
})
export class Cart implements OnInit {
  @ViewChild('infoModal') infoModal!: InfoModalComponent;
  cartItems: OrderItem[] = [];
  productsMap: Map<number, Product> = new Map();
  total = signal(0);

  constructor(
    private router: Router,
    private ordersService: OrdersService,
    private productsService: ProductsService
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart() {
    const cartJson = localStorage.getItem('cart');
    this.cartItems = cartJson ? JSON.parse(cartJson) : [];

    if (this.cartItems.length > 0) {
      const productIds = this.cartItems.map(item => item.id);
      this.productsService.getAll().subscribe(products => {
        products.forEach(p => {
          if (productIds.includes(p.id)) this.productsMap.set(p.id, p);
        });
        this.updateTotal();
      });
    } else {
      this.productsMap.clear();
      this.updateTotal();
    }
  }

  updateTotal() {
    let sum = 0;
    this.cartItems.forEach(item => {
      const product = this.productsMap.get(item.id);
      if (product) sum += product.price * item.quantity;
    });
    this.total.set(sum);
  }

  changeQuantity(item: OrderItem, event: Event) {
    const input = event.target as HTMLInputElement;
    const qty = input.valueAsNumber;

    if (qty <= 0) {
      this.removeItem(item.id);
    } else {
      const found = this.cartItems.find(ci => ci.id === item.id);
      if (found) {
        found.quantity = qty;
        this.saveCart();
        this.updateTotal();
      }
    }
  }

  removeItem(productId: number) {
    this.cartItems = this.cartItems.filter(item => item.id !== productId);
    this.saveCart();
    this.loadCart();
  }

  saveCart() {
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }

  placeOrder() {
    if (this.cartItems.length === 0) {
      alert('Your cart is empty!');
      return;
    }

    // ✅ transforma id -> productId
    const orderRequest = {
      items: this.cartItems.map(item => ({
        productId: item.id,
        quantity: item.quantity
      }))
    };

    this.ordersService.create(orderRequest).subscribe({
      next: () => {
        this.cartItems = [];
        this.productsMap.clear();
        this.saveCart();
        this.infoModal.show('Order placed successfully!');
      },
      error: (error) => {
        console.error('Failed to create order', error);
        alert('Failed to create order, please try again later.');
      }
    });
  }

  onModalClosed() {
    this.router.navigate(['/products']);
  }

  goToProducts() {
    this.router.navigate(['/products']);
  }

  isLoggedIn = false;

  goToAdmin() {
    if (!this.isLoggedIn) {
      alert('You need to be logged in as ADMIN!');
      return;
    }
    this.router.navigate(['/admin']);
  }
}
