import { Component, ElementRef, OnDestroy, OnInit } from '@angular/core';
import { SalesSignalRService } from '../../services/signalr.service';
import { SaleModel } from '../../Models/sale.model';
import * as echarts from 'echarts';

@Component({
  selector: 'app-sale-chart',
  template: '<div class="chart" id="saleChart"></div>',
  styles: [`
    .chart {
      width: 100%;
      height: 500px;
    }
  `]
})
export class SaleChartComponent implements OnInit, OnDestroy {
  private chart!: echarts.ECharts;
  private times: string[] = [];
  private amounts: number[] = [];
  private counts: number[] = [];
  private maxPoints = 100;

  constructor(private elm: ElementRef, private salesSignalR: SalesSignalRService) {}

  ngOnInit() {
    this.chart = echarts.init(this.elm.nativeElement.querySelector('#saleChart'));
    this.initChart();

    this.salesSignalR.onSalesUpdate((newSales: SaleModel[]) => {
      this.addData(newSales);
    });
  }

  ngOnDestroy(): void {
    this.salesSignalR.stopConnection();
  }

  private initChart() {
    this.chart.setOption({
      tooltip: { trigger: 'axis' },
      legend: { top: 10, left: 'center', data: ['Sum', 'Sales'] },
      xAxis: { type: 'category', data: this.times },
      yAxis: [
        { type: 'value', name: 'Sum', position: 'left' },
        { type: 'value', name: 'Sales', position: 'right' }
      ],
      dataZoom: [
      {
        type: 'slider',
        show: true,
        xAxisIndex: 0,
        start: 0,
        end: 100,
        height: 30,
        bottom: 10
      }
    ],
      series: [
        {
          name: 'Sum',
          type: 'line',
          smooth: true,
          yAxisIndex: 0,
          showSymbol: true,
          symbol: 'circle',
          symbolSize: 6,
          itemStyle: { color: '#1334eeff' },
          lineStyle: { color: '#d8b415ff' },
          data: this.amounts,
        },
        {
          name: 'Sales',
          type: 'bar',
          yAxisIndex: 1,
          data: this.counts,
          itemStyle: { color: '#1287fcff', opacity: 0.3 }
        }
      ]
    });
  }

  private addData(newSales: SaleModel[]) {
    const grouped: Record<string, SaleModel[]> = {};

    newSales.forEach(s => {
      const time = new Date(s.dateTimeSale).toLocaleTimeString();
      if (!grouped[time]) grouped[time] = [];
      grouped[time].push(s);
    });

    Object.entries(grouped).forEach(([time, sales]) => {
      const sumAmount = Number(sales.reduce((sum, s) => sum + s.amount, 0).toFixed(2));
      const count = sales.length;

      this.times.push(time);
      this.amounts.push(sumAmount);
      this.counts.push(count);

      if (this.times.length > this.maxPoints) {
        this.times.shift();
        this.amounts.shift();
        this.counts.shift();
      }
    });

    this.chart.setOption({
      xAxis: { data: this.times },
      series: [
        { name: 'Sum', data: this.amounts },
        { name: 'Sales', data: this.counts }
      ]
    });
  }
}
