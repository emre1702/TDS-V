/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FreeroamComponent } from './freeroam.component';

describe('FreeroamComponent', () => {
  let component: FreeroamComponent;
  let fixture: ComponentFixture<FreeroamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FreeroamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FreeroamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
