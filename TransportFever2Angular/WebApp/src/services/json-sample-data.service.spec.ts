import { TestBed } from '@angular/core/testing';

import { JsonSampleDataService } from './json-sample-data.service';

describe('JsonSampleDataService', () => {
  let service: JsonSampleDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JsonSampleDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
