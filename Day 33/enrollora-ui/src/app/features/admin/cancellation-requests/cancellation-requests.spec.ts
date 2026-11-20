import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CancellationRequests } from './cancellation-requests';

describe('CancellationRequests', () => {
  let component: CancellationRequests;
  let fixture: ComponentFixture<CancellationRequests>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CancellationRequests],
    }).compileComponents();

    fixture = TestBed.createComponent(CancellationRequests);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
