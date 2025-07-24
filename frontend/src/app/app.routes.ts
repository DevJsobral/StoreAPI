import { Routes } from '@angular/router';
import { Products } from './pages/products/products';
import { Cart } from './pages/cart/cart';
import { AdminComponent } from './pages/admin/admin';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'products', component: Products },
  { path: 'cart', component: Cart },
  { path: 'admin', component: AdminComponent,canActivate: [AuthGuard] },
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  { path: '**', redirectTo: 'products' }
];
