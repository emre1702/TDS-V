import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DamageTestMenuComponent } from './damage-test-menu.component';

describe('DamageTestMenuComponent', () => {
  let component: DamageTestMenuComponent;
  let fixture: ComponentFixture<DamageTestMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DamageTestMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DamageTestMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
