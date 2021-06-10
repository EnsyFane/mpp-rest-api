import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { EventName } from './models/event';
import { EventService } from './services/event-service/event.service';
import { MatchService } from './services/match-service/match.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

	@HostBinding('class') className = '';

	subscription: Subscription;
	title = 'angular-client';

	readonly darkModeClass = 'dark-mode';

	constructor(
		private eventService: EventService,
		private overlay: OverlayContainer,
		private matchService: MatchService) {
		this.subscription = eventService.on(EventName.ThemeChange).subscribe((lightMode: boolean) => {
			this.className = lightMode ? '' : this.darkModeClass;
			if (lightMode) {
				overlay.getContainerElement().classList.remove(this.darkModeClass);
			} else {
				overlay.getContainerElement().classList.add(this.darkModeClass);
			}
		});
	}

	ngOnInit(): void {
		this.matchService.subscribe().subscribe(res => {
			console.log(res);
		});
	}

	ngOnDestroy(): void {
		this.subscription.unsubscribe();
	}
}
