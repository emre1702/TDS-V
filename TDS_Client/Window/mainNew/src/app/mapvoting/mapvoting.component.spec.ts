import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapVotingComponent } from './mapvoting.component';

describe('MapVotingComponent', () => {
  let component: MapVotingComponent;
  let fixture: ComponentFixture<MapVotingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapVotingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapVotingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
