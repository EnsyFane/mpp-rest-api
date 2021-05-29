import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RestApiMatchesComponent } from './rest-api-matches.component';

describe('RestApiMatchesComponent', () => {
  let component: RestApiMatchesComponent;
  let fixture: ComponentFixture<RestApiMatchesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RestApiMatchesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RestApiMatchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
