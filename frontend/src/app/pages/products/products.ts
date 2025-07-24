import { Component, OnInit, signal, computed, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { ProductsService, Product } from '../../services/products.service';
import { CategoriesService, Category } from '../../services/categories.service';
import { ConfirmModalComponent } from '../confirm-modal/confirm-modal';
import { OrderItem } from '../../services/orders.service';
import { LoginModalComponent } from '../login/login-modal.component';
import { AuthService } from '../../services/auth.service';

declare var bootstrap: any;

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, FormsModule, ConfirmModalComponent, LoginModalComponent, RouterModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})

export class Products implements OnInit {
  @ViewChild('confirmModal') confirmModal!: ConfirmModalComponent;
  @ViewChild('loginModal') loginModal!: LoginModalComponent;
  @ViewChild('imageModal') imageModal!: ElementRef;

  modalImageUrl: string = '';
  username = '';
  selectedProduct: Product | undefined;

  products: Product[] = [];
  categories: Category[] = [];
  selectedCategoryId: number | undefined = undefined;
  searchName: string = '';

  selectedCategoryName: string = 'All Products';
  selectedCategoryImage: string = 'https://i.ibb.co/Sw1Npj18/All-Products.jpg';

  constructor(
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private router: Router,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadProducts(): void {
    const trimmedSearch = this.searchName.trim();
    const category = this.categories.find(cat => cat.categoryId === this.selectedCategoryId);

    if (category) {
      this.selectedCategoryName = category.name;
      this.selectedCategoryImage = category.imageURL;
    } else {
      this.selectedCategoryName = 'All Products';
      this.selectedCategoryImage = 'https://i.ibb.co/Sw1Npj18/All-Products.jpg';
    }

    this.productsService.getAll(trimmedSearch, this.selectedCategoryId)
      .subscribe({
        next: (data) => {
          this.products = data;
          console.log('Produtos carregados:', this.products);
        },
        error: (err) => {
          console.error('Failed to load products:', err);
          this.products = [];
        }
      });
  }

  loadCategories(): void {
    this.categoriesService.getAll()
      .subscribe((data) => this.categories = data);
  }

  openImage(url: string): void {
  this.modalImageUrl = url;
  const modal = new bootstrap.Modal(document.getElementById('imageModal')!);
  modal.show();
}

  filteredProducts: Product[] = [];
  searchTerm: string = '';
  selectedCategory: number = 0;
  showLoginSuccess = false;

  showLogin() {
    this.loginModal.show();
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  onLoggedIn(success: boolean) {
    if (success) {
      this.showLoginSuccess = true;
    }
    setTimeout(() => {
      this.showLoginSuccess = false;
    }, 2000);
  }

  logout() {
    this.confirmModal.show('Do you wish to logout?', 'logout');
  }

  confirmLogout() {
    this.confirmModal.show(`Really want to logout?`);
  }

  SelectedCategoryName(): string {
    if (!this.selectedCategory) {
      return 'All Products';
    }
    const cat = this.categories.find(c => c.categoryId == this.selectedCategory);
    return cat ? cat.name : 'All Products';
  }

  filterProducts() {
    this.productsService.getAll(this.searchTerm, this.selectedCategoryId ?? undefined).subscribe({
      next: (data) => this.products = data,
      error: (err) => {
        console.error(err);
        this.products = [];
      }
    });
  }

  goToAdmin() {
    this.router.navigate(['/admin']);
  }

  goToCart() {
    this.router.navigate(['/cart']);
  }

  addToCart(product: Product): void {
    this.selectedProduct = product;
    this.confirmModal.show(`Add ${product.name} to cart?`, 'cart');
  }

  onConfirmed(event: { confirmed: boolean, context: string }) {
    const { confirmed, context } = event;

    if (context === 'cart') {
      if (confirmed && this.selectedProduct) {
        const stored = localStorage.getItem('cart');
        const cart: OrderItem[] = stored ? JSON.parse(stored) : [];

        const existing = cart.find(item => item.productId === this.selectedProduct!.productId);

        if (existing) {
          existing.quantity += 1;
        } else {
          cart.push({ productId: this.selectedProduct.productId, quantity: 1 });
        }

        localStorage.setItem('cart', JSON.stringify(cart));
        this.selectedProduct = undefined!;
      }
    }

    if (context === 'logout') {
      if (confirmed) {
        this.authService.logout();
        this.router.navigate(['/products']);
      }
    }
  }

  onCancel(): void { }
}
