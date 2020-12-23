import { TestBed, inject } from '@angular/core/testing';
import { MessageHubService } from './message-hub.service';

describe('Service: MessageHub', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MessageHubService]
    });
  });

  it('should be created', inject([MessageHubService], (service: MessageHubService) => {
    expect(service).toBeTruthy();
  }));
});
