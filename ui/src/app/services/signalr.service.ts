import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { SaleModel } from '../Models/sale.model';

@Injectable()
export class SalesSignalRService {
  private hubConnection!: HubConnection;

  private readonly urlHub: string = 'http://localhost:5000/salesHub';

  constructor(){
    this.startConnection();
  }

  public startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.urlHub)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('connected successfully'))
      .catch(err => console.error('connected error', err));
  }

  public stopConnection(): void {
    this.hubConnection.stop();
  }

  public onSalesUpdate(callback: (sales: SaleModel[]) => void): void {
    this.hubConnection.on('ReceiveSales', (sales) => {
      callback(sales);
    });
  }
}
