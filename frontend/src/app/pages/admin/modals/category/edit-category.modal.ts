import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CategoriesService } from '../../../../services/categories.service';
import { ImageUploadService } from '../../../../services/image-upload.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-category-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-category-modal.html',
  styleUrls: ['./edit-category-modal.css']
})

export class EditCategoryModalComponent {
  @Input() categoryId!: number;
  @Input() categoryName!: string;
  @Input() categoryImageUrl!: string;
  @Output() editClose = new EventEmitter<boolean>();


  name!: string;
  imageFile?: File;
  imagePreview?: string;
  uploading = false;
  errorMsg = '';

  constructor(
    private categoriesService: CategoriesService,
    private imageUploadService: ImageUploadService
  ) { }

  ngOnInit() {
    this.name = this.categoryName;
    this.imagePreview = this.categoryImageUrl;
  }

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

    if (this.uploading) return;

    this.uploading = true;

    if (this.imageFile) {
      this.imageUploadService.uploadImage(this.imageFile).subscribe({
        next: (imageUrl) => {
          this.updateCategory(imageUrl);
        },
        error: () => {
          this.uploading = false;
          this.errorMsg = 'Erro ao enviar imagem.';
        }
      });
    } else {
      this.updateCategory(this.imagePreview!);
    }
  }

  private updateCategory(imageUrl: string) {
    const categoryToUpdate = {
      id: this.categoryId,
      name: this.name,
      imageURL: imageUrl
    };

    this.categoriesService.update(this.categoryId, categoryToUpdate)
      .subscribe({
        next: () => {
          this.uploading = false;
          this.editClose.emit(true);
        },
        error: (err) => {
          this.uploading = false;
          if (err.status === 400 && err.error?.errors) {
            const errors = err.error.errors as { [key: string]: string[] };
            this.errorMsg = Object.values(errors)
              .map((msgs) => (msgs as string[]).join(', '))
              .join('; ');
          } else {
            this.errorMsg = 'Erro ao atualizar categoria.';
          }
        }
      });
  }


  close() {
    this.editClose.emit(false);
  }
}
