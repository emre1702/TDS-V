/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RageConnectorService } from './rage-connector.service';

describe('Service: RageConnector', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RageConnectorService]
    });
  });

  it('should ...', inject([RageConnectorService], (service: RageConnectorService) => {
    expect(service).toBeTruthy();
  }));
});
