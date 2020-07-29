import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GangWindowVehiclesComponent } from './gang-window-vehicles.component';

describe('GangWindowVehiclesComponent', () => {
  let component: GangWindowVehiclesComponent;
  let fixture: ComponentFixture<GangWindowVehiclesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GangWindowVehiclesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GangWindowVehiclesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
