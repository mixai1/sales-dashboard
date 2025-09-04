import { Component } from '@angular/core';
import { SaleChartComponent } from './components/sales-chart/sales-chart.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.scss',
  imports: [SaleChartComponent]
})
export class App {}
