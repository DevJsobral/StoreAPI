import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Product, ProductsService } from '../../../../services/products.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-patch-product-modal',
  templateUrl: './patch-product.html',
  styleUrls: ['../category/edit-category-modal.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class PatchProductModalComponent {
  @Input() product!: Product;
  @Output() onClose = new EventEmitter<void>();
  @Output() onPatched = new EventEmitter<Product>();

  newPrice: number | null = null;
  newStock: number | null = null;
  errorMessage: string | null = null;

  updating = false;

  constructor(private productsService: ProductsService) { }

  ngOnInit() {
    this.newPrice = this.product.price;
    this.newStock = this.product.stock;
  }

  save() {
    this.errorMessage = '';

    if (this.newPrice == null || this.newPrice < 0) {
      this.errorMessage = 'Price must be a valid number.';
      return;
    }
    if (this.newStock == null || this.newStock < 0) {
      this.errorMessage = 'Stock must be a valid number.';
      return;
    }

    this.updating = true;

    this.productsService
      .patchPriceAndStock(this.product.id, this.newPrice, this.newStock)
      .subscribe({
        next: (updatedProduct) => {
          this.updating = false;
          this.onPatched.emit(updatedProduct);
          this.close();
        },
        error: (err) => {
          this.updating = false;

          if (err.status === 400 && err.error?.errors) {
            const errors = err.error.errors as { [key: string]: string[] };
            this.errorMessage = Object.values(errors)
              .map(msgs => msgs.join(', '))
              .join('; ');
          } else {
            this.errorMessage = err.error?.message || 'Error updating product.';
          }
        }
      });
  }


  close() {
    this.onClose.emit();
  }
}
