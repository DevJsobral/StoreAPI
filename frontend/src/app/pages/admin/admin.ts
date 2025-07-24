import { Component, OnInit, ViewChild } from '@angular/core';
import { ProductsService, Product } from '../../services/products.service';
import { CategoriesService, Category } from '../../services/categories.service';
import { OrdersService, OrderResponseWithDetails } from '../../services/orders.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConfirmModalComponent } from '../confirm-modal/confirm-modal';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { EditCategoryModalComponent } from './modals/category/edit-category.modal';
import { NewCategoryModalComponent } from './modals/category/new-category.modal';
import { EditProductModalComponent } from './modals/product/edit-product.modal';
import { NewProductModalComponent } from './modals/product/new-product.modal';
import { PatchProductModalComponent } from './modals/product/patch-product.modal';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.html',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ConfirmModalComponent,
    RouterModule,
    EditCategoryModalComponent,
    NewCategoryModalComponent,
    EditProductModalComponent,
    NewProductModalComponent,
    PatchProductModalComponent
  ],
  styleUrls: ['./admin.css'],
})

export class AdminComponent implements OnInit {
  @ViewChild('confirmModal') confirmModal!: ConfirmModalComponent;

  activeTab: 'products' | 'categories' | 'orders' = 'products';
  editingCategory: Category | null = null;
  editingProduct: Product | null = null;
  patchingProduct: Product | null = null;
  creatingCategory: boolean = false;
  creatingProduct: boolean = false;

  products: Product[] = [];
  categories: Category[] = [];
  orders: OrderResponseWithDetails[] = [];

  constructor(
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private ordersService: OrdersService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  selectTab(tab: 'products' | 'categories' | 'orders') {
    this.activeTab = tab;
    this.loadData();
  }

  loadData() {
    if (this.activeTab === 'products') {
      this.productsService.getAll().subscribe({
        next: (res) => {
          this.products = res;
        },
        error: (err) => console.error('Error loading products', err),
      });
    } else if (this.activeTab === 'orders') {
      this.ordersService.getAll().subscribe({
        next: (res) => (this.orders = res),
        error: (err) => console.error('Error loading orders', err),
      });
    }
  }

  loadCategories() {
    this.categoriesService.getAll().subscribe({
      next: (cats) => {
        this.categories = cats;
        this.loadData();
      },
      error: (err) => console.error('Error loading categories', err),
    });
  }

  getProductName(productId: number): string {
    const product = this.products.find((p) => p.productId === productId);
    return product ? product.name : 'Unknown Product';
  }

  getCategoryName(categoryId: number | string): string {
    const cat = this.categories.find((c) => c.categoryId === +categoryId);
    return cat ? cat.name : 'Unknown';
  }

  openNewProductModal() {
    console.log('Opening New Product Modal');
    this.creatingProduct = true;
  }

  closeNewProductModal() {
    this.creatingProduct = false;
  }

  onProductCreated(newProduct: Product) {
    this.products.push(newProduct);
    this.closeNewProductModal();
  }
  openEditProductModal(product: Product) {
    this.editingProduct = { ...product };
  }

  closeEditProductModal() {
    this.editingProduct = null;
  }

  onProductSaved(updatedProduct: Product) {
    const index = this.products.findIndex(p => p.productId === updatedProduct.productId);
    if (index !== -1) {
      this.products[index] = updatedProduct;
    }
    this.closeEditProductModal();
  }


  openPatchProductModal(product: Product) {
    this.patchingProduct = product;
  }

  closePatchProductModal() {
    this.patchingProduct = null;
  }

  onProductPatched(updated: Product) {
    const index = this.products.findIndex(p => p.productId === updated.productId);
    if (index !== -1) {
      this.products[index] = updated;
    }
  }
  openNewCategoryModal() {
    this.creatingCategory = true;
  }

  openEditCategoryModal(category: Category) {
    this.editingCategory = category;
  }

  handleEditClose(updated: boolean) {
    this.editingCategory = null;
    if (updated) {
      this.loadCategories();
    }
  }

  handleNewCategoryClose(created: boolean) {
    this.creatingCategory = false;
    if (created) {
      this.loadCategories();
    }
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  goToCart() {
    this.router.navigate(['/cart']);
  }

  logout() {
    this.confirmModal.show('Do you wish to logout?', 'logout');
  }

  onConfirmed(event: { confirmed: boolean; context: string }) {
    const { confirmed, context } = event;
    if (context === 'logout') {
      if (confirmed) {
        this.authService.logout();
        this.router.navigate(['/products']);
      }
    }
  }

  deleteProduct(id: number) {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productsService.delete(id).subscribe({
        next: () => {
          this.products = this.products.filter((p) => p.productId !== id);
        },
        error: (err) => {
          console.error('Error deleting product', err);
          alert('Failed to delete product.');
        },
      });
    }
  }

  deleteCategory(id: number) {
    if (confirm('Are you sure you want to delete this category?')) {
      this.categoriesService.delete(id).subscribe({
        next: () => {
          this.categories = this.categories.filter((c) => c.categoryId !== id);
        },
        error: (err) => {
          console.error('Error deleting category', err);
          alert('Failed to delete category.');
        },
      });
    }
  }

  deleteOrder(id: number) {
    if (confirm('Are you sure you want to delete this order?')) {
      this.ordersService.delete(id).subscribe({
        next: () => {
          this.orders = this.orders.filter((o) => o.orderId !== id);
        },
        error: (err) => {
          console.error('Error deleting order', err);
          alert('Failed to delete order.');
        },
      });
    }
  }
}
