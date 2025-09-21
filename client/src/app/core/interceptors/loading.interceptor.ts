import { Injectable } from '@angular/core'
import { HttpRequest,HttpEvent, HttpHandler,HttpInterceptor } from '@angular/common/http';
import { catchError, delay, finalize, Observable, throwError } from 'rxjs'; 
import { Router } from '@angular/router';
import { LoadingService } from '../services/loading.service';

@Injectable()

export class LoadingInterceptor implements HttpInterceptor{
  
  constructor(private loadingService: LoadingService){}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loadingService.loading();
    return next.handle(req).pipe(
      delay(6000),
      finalize(()=>{
        this.loadingService.idle();
      })
    );
  }  
};
