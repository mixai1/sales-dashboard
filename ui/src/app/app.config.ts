import { ApplicationConfig, importProvidersFrom, inject, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { SalesSignalRService } from './services/signalr.service';
import { SaleChartComponent } from './components/sales-chart/sales-chart.component';
import { NgxEchartsModule } from 'ngx-echarts';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(
      NgxEchartsModule.forRoot({
        echarts: () => import('echarts')
      })
    ),
    SalesSignalRService
  ]
};
