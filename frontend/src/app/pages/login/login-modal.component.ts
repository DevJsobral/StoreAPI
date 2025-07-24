import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
<div class="modal-backdrop" *ngIf="isVisible">
  <div class="modal-content p-4">
    <h4 class="mb-3 fw-bold">Login</h4>

    <div class="mb-3">
      <label for="email" class="form-label fw-bold">Username</label>
      <input id="email" type="email" class="form-control" [(ngModel)]="username" />
      <div *ngIf="errors.email" class="text-danger mt-1">{{ errors.email }}</div>
    </div>

    <div class="mb-3">
      <label for="password" class="form-label fw-bold">Password</label>
      <input id="password" type="password" class="form-control" [(ngModel)]="password" />
      <div *ngIf="errors.password" class="text-danger mt-1">{{ errors.password }}</div>
    </div>

    <div *ngIf="errors.general" class="text-danger mb-3 fw-bold">{{ errors.general }}</div>

    <div class="d-flex justify-content-end gap-2">
      <button class="btn btn-danger btn-sm" (click)="close()">Cancel</button>
      <button class="btn btn-success btn-sm" (click)="login()">Login</button>
    </div>
  </div>
</div>
  `,
  styles: [`
    .modal-backdrop {
      position: fixed;
      inset: 0;
      background: rgba(0,0,0,0.5);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1050;
    }
    .modal-content {
      background: white;
      border-radius: 0.3rem;
      max-width: 400px;
      width: 100%;
    }
  `]
})
export class LoginModalComponent {
  isVisible = false;
  username = '';
  password = '';
  errors: any = {};

  @Output() loggedIn = new EventEmitter<boolean>();
  @Output() closed = new EventEmitter<void>();

  constructor(private authService: AuthService) { }

  show() {
    this.isVisible = true;
    this.username = '';
    this.password = '';
    this.errors = {};
  }

  close() {
    this.isVisible = false;
    this.closed.emit();
  }

  login() {
    this.errors = {};

    if (!this.username) {
      this.errors.username = 'Username is required';
    }
    if (!this.password) {
      this.errors.password = 'Password is required';
    }
    if (Object.keys(this.errors).length > 0) return;

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.isVisible = false;
        this.loggedIn.emit();
      },
      error: (err) => {
        if (err.status === 401) {
          this.errors.general = 'Invalid username or password';
        } else {
          this.errors.general = 'Login failed. Try again later.';
        }
      }
    });
  }
}
