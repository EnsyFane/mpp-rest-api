import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { startWith, switchMap, tap } from 'rxjs/operators';
import { Match, MatchType } from 'src/app/models/match-model';
import { EventService } from 'src/app/services/event-service/event.service';
import { MatchService } from 'src/app/services/match-service/match.service';

@Component({
	selector: 'matches-table',
	templateUrl: './matches-table.component.html',
	styleUrls: ['./matches-table.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class MatchesTableComponent implements OnInit, OnDestroy {
	displayedColumns: string[] = ['select', 'homeTeam', 'guestTeam', 'matchType', 'availableSeats', 'ticketPrice'];
	selection = new SelectionModel<number>(true, []);

	matches: Match[] = [];
	dataSource = new MatTableDataSource<Match>();
	pollingInterval = 5000;

	subscriptions: Map<string, Subscription> = new Map<string, Subscription>();

	constructor(
		private eventService: EventService,
		private matchService: MatchService) { }

	ngOnInit(): void {
		const initSubscription = this.matchService.getMatches()
			.pipe(
				tap((matches: Match[]) => {
					this.matches = matches;
					this.dataSource.data = this.matches;
				}),
				tap(() => {
					this.cleanSubscription('initial-sub');
					this.refreshPolling();
				})
			).subscribe()

		this.subscriptions.set('initial-sub', initSubscription);
	}

	ngOnDestroy(): void {
		this.cleanSubscription('initial-sub');
		this.cleanSubscription('polling');
	}

	isAllSelected(): boolean {
		const numSelected = this.selection.selected.length;
		const numRows = this.dataSource.data.length;
		return numSelected === numRows;
	}

	masterToggle() {
		if (this.isAllSelected()) {
			this.selection.clear();
			return
		}

		this.selection.select(...this.dataSource.data.map(m => m.id))
	}

	private cleanSubscription(name: string): void {
		const oldSub = this.subscriptions.get(name);
		if (oldSub) {
			oldSub.unsubscribe();
		}
		this.subscriptions.delete(name);
	}

	private refreshPolling(): void {
		const pollingSub = interval(this.pollingInterval)
			.pipe(
				startWith(0)
			).pipe(
				switchMap(() => {
					return this.matchService.getMatches();
				}),
				tap((matches: Match[]) => {
					this.matches = matches;
					this.dataSource.data = this.matches;
				})
			).subscribe();

		this.cleanSubscription('polling');
		this.subscriptions.set('polling', pollingSub);
	}
}
