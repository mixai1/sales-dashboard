export class Sale {
  public dateTimeSale: Date;
  public amount: number;

  public constructor(
    fields?: Partial<Sale>) {
    if (fields) {
      Object.assign(this, fields);
    }
  }
}