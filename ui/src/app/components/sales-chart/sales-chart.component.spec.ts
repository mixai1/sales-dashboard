import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ElementRef, NO_ERRORS_SCHEMA } from '@angular/core';
import { SalesSignalRService } from '../../services/signalr.service';
import { SaleChartComponent } from './sales-chart.component';

describe('SaleChartComponent', () => {
  let component: SaleChartComponent;
  let fixture: ComponentFixture<SaleChartComponent>;
  let salesSignalRServiceMock: jasmine.SpyObj<SalesSignalRService>;

  beforeEach(async () => {
    salesSignalRServiceMock = jasmine.createSpyObj(
      'SalesSignalRService',
      [
        'onSalesUpdate',
        'stopConnection'
      ]
    );

    await TestBed.configureTestingModule({
      imports: [SaleChartComponent],
      providers: [
        { provide: ElementRef, useValue: { nativeElement: document.createElement('div') } },
        { provide: SalesSignalRService, useValue: salesSignalRServiceMock }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(SaleChartComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should init chart and subscribe on ngOnInit', () => {
    component.ngOnInit();

    expect(salesSignalRServiceMock.onSalesUpdate).toHaveBeenCalled();
  });

  it('should stop connection on ngOnDestroy', () => {
    component.ngOnDestroy();

    expect(salesSignalRServiceMock.stopConnection).toHaveBeenCalled();
  });
});