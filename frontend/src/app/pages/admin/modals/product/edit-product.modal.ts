import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { ProductsService, Product } from '../../../../services/products.service';
import { Category } from '../../../../services/categories.service';
import { ImageUploadService } from '../../../../services/image-upload.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-product-modal',
  standalone: true,
  templateUrl: './edit-product.html',
  imports: [CommonModule, FormsModule],
  styleUrls: ['../category/edit-category-modal.css']
})
export class EditProductModalComponent implements OnInit {
  @Input() product!: Product;
  @Input() categories: Category[] = [];
  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<Product>();

  uploading = false;
  errorMsg = '';
  imagePreview: string | ArrayBuffer | null = null;
  selectedFile: File | null = null;

  constructor(
    private productsService: ProductsService,
    private imageUploadService: ImageUploadService
  ) { }

  ngOnInit() {
    if (this.product.imageURL) {
      this.imagePreview = this.product.imageURL;
    }
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;

      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreview = (e.target?.result) as string | ArrayBuffer | null;
      };
      reader.readAsDataURL(file);
    }
  }


  save() {
    if (this.uploading) return;

    if (!this.product.name.trim()) {
      this.errorMsg = 'Product name is required.';
      return;
    }
    if (this.product.price == null || this.product.price < 0) {
      this.errorMsg = 'Price must be a positive number.';
      return;
    }
    if (this.product.stock == null || this.product.stock < 0) {
      this.errorMsg = 'Stock must be a positive number.';
      return;
    }
    if (!this.product.categoryId) {
      this.errorMsg = 'Category must be selected.';
      return;
    }

    this.errorMsg = '';
    this.uploading = true;

    if (this.selectedFile) {
      this.imageUploadService.uploadImage(this.selectedFile).subscribe({
        next: (imageUrl) => {
          this.updateProduct(imageUrl);
        },
        error: () => {
          this.errorMsg = 'Error uploading image.';
          this.uploading = false;
        }
      });
    } else {
      this.updateProduct(this.product.imageURL || '');
    }
  }


  private updateProduct(imageUrl: string) {
    const productToUpdate: Product = {
      ...this.product,
      imageURL: imageUrl
    };

    this.productsService.update(this.product.productId, productToUpdate).subscribe({
      next: updatedProduct => {
        this.uploading = false;
        this.onSave.emit(updatedProduct);
        this.close();
      },
      error: err => {
        this.uploading = false;
        if (err.status === 400 && err.error?.errors) {
          const errors = err.error.errors as { [key: string]: string[] };
          this.errorMsg = Object.values(errors)
            .map(msgs => msgs.join(', '))
            .join('; ');
        } else {
          this.errorMsg = err.error?.message || 'Error updating product.';
        }
      }
    });
  }

  close() {
    this.onClose.emit();
  }
}
