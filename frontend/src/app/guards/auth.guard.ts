import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    const isAuth = this.authService.isAuthenticated();

    if (isAuth) {
      return true;
    } else {
      alert('You need to be logged in to access this page!');
      this.router.navigate(['/products']);
      return false;
    }
  }
}
