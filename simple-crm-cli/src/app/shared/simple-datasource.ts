import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable } from 'rxjs/Observable';

export class SimpleDataSource<T> extends DataSource<T> {
  constructor(private rows$: Observable<T[]>) { super(); }
  connect(collectionViewer: CollectionViewer): Observable<T[]> { return this.rows$; }
  disconnect(collectionViewer: CollectionViewer): void {}
}
