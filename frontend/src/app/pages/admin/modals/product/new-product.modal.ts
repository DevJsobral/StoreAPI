import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ProductsService, Product } from '../../../../services/products.service';
import { Category } from '../../../../services/categories.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ImageUploadService } from '../../../../services/image-upload.service';

@Component({
  selector: 'app-new-product-modal',
  templateUrl: './new-product.html',
  styleUrls: ['../category/edit-category-modal.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class NewProductModalComponent {
  @Output() onClose = new EventEmitter<void>();
  @Output() onCreate = new EventEmitter<Product>();
  @Input() categories: Category[] = [];

  name = '';
  description = '';
  price: number | null = null;
  stock: number | null = null;
  categoryId: number | null = null;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;

  uploading = false;
  errorMsg = '';

  constructor(private productsService: ProductsService,
    private imageUploadService: ImageUploadService
  ) { }

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
    this.errorMsg = '';
    if (this.uploading) return;

    if (!this.name.trim()) {
      this.errorMsg = 'Product name is required.';
      return;
    }
    if (this.price == null || this.price < 0) {
      this.errorMsg = 'Price must be a positive number.';
      return;
    }
    if (this.stock == null || this.stock < 0) {
      this.errorMsg = 'Stock must be a positive number.';
      return;
    }
    if (!this.categoryId) {
      this.errorMsg = 'Category must be selected.';
      return;
    }

    this.uploading = true;

    if (this.selectedFile) {
      this.imageUploadService.uploadImage(this.selectedFile).subscribe({
        next: (imageUrl: string) => {
          this.createProduct(imageUrl);
        },
        error: () => {
          this.uploading = false;
          this.errorMsg = 'Error uploading image.';
        }
      });
    } else {
      this.createProduct('');
    }
  }

  private createProduct(imageUrl: string) {
    const newProduct: Product = {
      productId: 0,
      name: this.name.trim(),
      description: this.description || '',
      price: this.price!,
      stock: this.stock!,
      categoryId: this.categoryId!,
      imageURL: imageUrl
    };

    this.productsService.create(newProduct).subscribe({
      next: createdProduct => {
        this.uploading = false;
        this.onCreate.emit(createdProduct);
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
          this.errorMsg = err.error?.message || 'Error creating product.';
        }
      }
    });
  }


  close() {
    this.onClose.emit();
  }
}
