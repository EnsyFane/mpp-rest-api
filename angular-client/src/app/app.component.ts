import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, HostBinding, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { EventName } from './models/event';
import { EventService } from './services/event-service/event.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy {

	@HostBinding('class') className = '';

	subscription: Subscription;
	title = 'angular-client';

	readonly darkModeClass = 'dark-mode';

	constructor(
		private eventService: EventService,
		private overlay: OverlayContainer) {
		this.subscription = eventService.on(EventName.ThemeChange).subscribe((lightMode: boolean) => {
			this.className = lightMode ? '' : this.darkModeClass;
			if (lightMode) {
				overlay.getContainerElement().classList.remove(this.darkModeClass);
			} else {
				overlay.getContainerElement().classList.add(this.darkModeClass);
			}
		});
	}

	ngOnDestroy(): void {
		this.subscription.unsubscribe();
	}
}
