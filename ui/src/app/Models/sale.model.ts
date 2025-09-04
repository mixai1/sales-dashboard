export class SaleModel {
  public dateTimeSale: Date;
  public amount: number;

  public constructor(
    fields?: Partial<SaleModel>) {
    if (fields) {
      Object.assign(this, fields);
    }
  }
}