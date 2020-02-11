import { Component, OnInit, OnDestroy, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { UserpanelService } from '../services/userpanel.service';

@Component({
  selector: 'app-userpanel-faqs',
  templateUrl: './userpanel-faqs.component.html',
  styleUrls: ['./userpanel-faqs.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelFAQsComponent implements OnInit, OnDestroy {

  constructor(public userpanelService: UserpanelService, private changeDetector: ChangeDetectorRef) { }

  ngOnInit() {
    this.userpanelService.faqsLoaded.on(null, this.detectChanges.bind(this));
  }

  ngOnDestroy(): void {
    this.userpanelService.faqsLoaded.off(null, this.detectChanges.bind(this));
  }

  private detectChanges() {
    this.changeDetector.detectChanges();
  }

}
