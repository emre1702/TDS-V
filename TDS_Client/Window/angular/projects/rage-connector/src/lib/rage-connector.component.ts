import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'lib-rage-connector',
  template: `
    <p>
      rage-connector works!
    </p>
  `,
  styles: [],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RageConnectorComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
