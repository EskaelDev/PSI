import { TestBed, inject } from '@angular/core/testing';
import { AuthenticationService } from './authentication.service';
import { CommonTestModule } from 'src/app/shared/common-test/common-test.module';

describe('Service: Authentication', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CommonTestModule],
      providers: [AuthenticationService],
    });
  });

  it('should be created', inject(
    [AuthenticationService],
    (service: AuthenticationService) => {
      expect(service).toBeTruthy();
    }
  ));
});
