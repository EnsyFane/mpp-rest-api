import { Component, OnInit } from '@angular/core';
import { EventService } from 'src/app/services/event-service/event.service';

@Component({
	selector: 'matches-table',
	templateUrl: './matches-table.component.html',
	styleUrls: ['./matches-table.component.scss']
})
export class MatchesTableComponent implements OnInit {
	displayedColumns: string[] = ['name'];

	dataSource: any[] = [
		{ name: 'asd' },
		{ name: 'sdaasda' }
	];

	constructor(private eventService: EventService) {
		// TODO
	}

	ngOnInit(): void {
		// TODO
	}

}
