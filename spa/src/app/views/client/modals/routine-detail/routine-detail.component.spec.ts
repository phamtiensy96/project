import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoutineDetailComponent } from './routine-detail.component';

describe('RoutineDetailComponent', () => {
  let component: RoutineDetailComponent;
  let fixture: ComponentFixture<RoutineDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoutineDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoutineDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
