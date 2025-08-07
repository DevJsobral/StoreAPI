import { Component, Output, EventEmitter } from '@angular/core';
import { CategoriesService, Category } from '../../../../services/categories.service';
import { ImageUploadService } from '../../../../services/image-upload.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-new-category-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './new-category-modal.html',
  styleUrls: ['./edit-category-modal.css']
})
export class NewCategoryModalComponent {
  @Output() createClose = new EventEmitter<boolean>();

  name = '';
  imageFile?: File;
  imagePreview?: string;
  uploading = false;
  errorMsg = '';

  constructor(
    private categoriesService: CategoriesService,
    private imageUploadService: ImageUploadService
  ) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.imageFile = file;
      const reader = new FileReader();
      reader.onload = e => this.imagePreview = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  save() {
    this.errorMsg = '';
    if (!this.name.trim()) {
      this.errorMsg = 'Name is required.';
      return;
    }

    if (!this.imageFile) {
      this.errorMsg = 'Image file is required.';
      return;
    }

    this.uploading = true;

    this.imageUploadService.uploadImage(this.imageFile).subscribe({
      next: (imageUrl) => {
        const newCategory: Category = {
          id: 0,
          name: this.name,
          imageURL: imageUrl
        };

        this.categoriesService.create(newCategory).subscribe({
          next: () => {
            this.uploading = false;
            this.createClose.emit(true);
          },
          error: (err) => {
            this.uploading = false;
            if (err.status === 400 && err.error?.errors) {
              const errors = err.error.errors as { [key: string]: string[] };
              this.errorMsg = Object.values(errors)
                .map(msgs => (msgs as string[]).join(', '))
                .join('; ');
            } else {
              this.errorMsg = 'Error creating category.';
            }
          }
        });
      },
      error: () => {
        this.uploading = false;
        this.errorMsg = 'Error uploading image.';
      }
    });
  }

  close() {
    this.createClose.emit(false);
  }
}
