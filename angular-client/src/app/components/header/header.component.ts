import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AppEvent, EventName } from 'src/app/models/event';
import { EventService } from 'src/app/services/event-service/event.service';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
	lightMode: boolean = true;

	constructor(private eventService: EventService) { }

	themeSlideChanged() {
		this.eventService.emit(new AppEvent(EventName.ThemeChange, this.lightMode));
	}
}
