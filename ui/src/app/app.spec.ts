import { TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { App } from './app';
import { SalesSignalRService } from './services/signalr.service';

describe('App', () => {
  let salesSignalRServiceMock: jasmine.SpyObj<SalesSignalRService>;

  beforeEach(async () => {
    salesSignalRServiceMock = jasmine.createSpyObj('SalesSignalRService', [
      'onSalesUpdate',
      'stopConnection'
    ]);

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [{ provide: SalesSignalRService, useValue: salesSignalRServiceMock }],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
