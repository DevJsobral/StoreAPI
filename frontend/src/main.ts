import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthGuard } from './app/guards/auth.guard';

bootstrapApplication(App, {
  providers: [
   provideHttpClient(),
    AuthGuard,
    provideRouter(routes),
  ]
}).catch(err => console.error(err));
