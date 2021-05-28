import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
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

	constructor(private eventService: EventService) {
		this.subscription = eventService.on(EventName.ThemeChange).subscribe((lightMode: boolean) => {
			this.className = lightMode ? '' : 'dark-mode';
		});
	}

	ngOnDestroy(): void {
		this.subscription.unsubscribe();
	}
}
