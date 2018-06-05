import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { UserOnlineComponent } from "./useronline.component";

describe("UserOnlineComponent", () => {
  let component: UserOnlineComponent;
  let fixture: ComponentFixture<UserOnlineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserOnlineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserOnlineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
