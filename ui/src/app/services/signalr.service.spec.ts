import { TestBed } from '@angular/core/testing';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { SaleModel } from '../Models/sale.model';
import { SalesSignalRService } from './signalr.service';

describe('SalesSignalRService', () => {
  let service: SalesSignalRService;
  let hubConnectionMock: jasmine.SpyObj<HubConnection>;
  let builderMockInstance: any;

  beforeEach(() => {
    hubConnectionMock = jasmine.createSpyObj<HubConnection>('HubConnection', ['start', 'stop', 'on']);
    hubConnectionMock.start.and.returnValue(Promise.resolve());
    hubConnectionMock.stop.and.returnValue(Promise.resolve());

    builderMockInstance = {
      withUrl: jasmine.createSpy('withUrl').and.callFake(() => builderMockInstance),
      withAutomaticReconnect: jasmine.createSpy('withAutomaticReconnect').and.callFake(() => builderMockInstance),
      build: jasmine.createSpy('build').and.returnValue(hubConnectionMock)
    };

    spyOn(HubConnectionBuilder.prototype, 'withUrl').and.callFake(builderMockInstance.withUrl);
    spyOn(HubConnectionBuilder.prototype, 'withAutomaticReconnect').and.callFake(builderMockInstance.withAutomaticReconnect);
    spyOn(HubConnectionBuilder.prototype, 'build').and.callFake(builderMockInstance.build);

    TestBed.configureTestingModule({
      providers: [SalesSignalRService]
    });

    service = TestBed.inject(SalesSignalRService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should start connection on creation', () => {
    expect(builderMockInstance.withUrl).toHaveBeenCalledWith('http://localhost:5000/salesHub');
    expect(builderMockInstance.withAutomaticReconnect).toHaveBeenCalled();
    expect(builderMockInstance.build).toHaveBeenCalled();
    expect(hubConnectionMock.start).toHaveBeenCalled();
  });

  it('should call stop on hubConnection when stopConnection is called', () => {
    service.stopConnection();

    expect(hubConnectionMock.stop).toHaveBeenCalled();
  });

  it('should register onSalesUpdate callback', () => {
    const cb = jasmine.createSpy('callback');

    service.onSalesUpdate(cb);

    expect(hubConnectionMock.on).toHaveBeenCalledWith('ReceiveSales', jasmine.any(Function));
    const handler = hubConnectionMock.on.calls.argsFor(0)[1];
    const sales: SaleModel[] = [{ dateTimeSale: new Date(), amount: 100 }];
    handler(sales);
    expect(cb).toHaveBeenCalledWith(sales);
  });
});
