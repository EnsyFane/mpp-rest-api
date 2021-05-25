import { Component, OnInit } from '@angular/core';

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

	constructor() {
		// TODO
	 }

	ngOnInit(): void {
		// TODO
	}

}
