import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { PlayerOnlineComponent } from "./playeronline.component";

describe("PlayerOnlineComponent", () => {
  let component: PlayerOnlineComponent;
  let fixture: ComponentFixture<PlayerOnlineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerOnlineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerOnlineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
