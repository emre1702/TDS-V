import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { ReportAdminComponent } from "./reportadmin.component";
import { ReportUserComponent } from "./reportuser.component";

describe("ReportComponent", () => {
  let component: ReportAdminComponent;
  let fixture: ComponentFixture<ReportAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});

describe("ReportComponent", () => {
    let component: ReportUserComponent;
    let fixture: ComponentFixture<ReportUserComponent>;

    beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [ ReportUserComponent ]
      })
      .compileComponents();
    }));

    beforeEach(() => {
      fixture = TestBed.createComponent(ReportUserComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
    });

    it("should create", () => {
      expect(component).toBeTruthy();
    });
  });

