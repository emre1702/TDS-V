import { TestBed, inject } from "@angular/core/testing";

import { AuthService } from "./auth.service";
import { AuthGuardService } from "./auth-guard.service";

describe("AuthService", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService]
    });
  });

  it("should be created", inject([AuthService], (service: AuthService) => {
    expect(service).toBeTruthy();
  }));
});

describe("AuthGuardService", () => {
    beforeEach(() => {
      TestBed.configureTestingModule({
        providers: [AuthGuardService]
      });
    });

    it("should be created", inject([AuthGuardService], (service: AuthGuardService) => {
      expect(service).toBeTruthy();
    }));
  });
