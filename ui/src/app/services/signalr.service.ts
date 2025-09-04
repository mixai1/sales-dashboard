import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { SaleModel } from '../Models/sale.model';

@Injectable()
export class SalesSignalRService {
  private hubConnection!: HubConnection;

  constructor(){
    this.startConnection();
  }

  public startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/salesHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('connected successfully'))
      .catch(err => console.error('connected error', err));
  }

  public onSalesUpdate(callback: (sales: SaleModel[]) => void): void {
    this.hubConnection.on('ReceiveSales', (sales) => {
      callback(sales);
    });
  }
}