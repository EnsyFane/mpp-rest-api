import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { startWith, switchMap, tap } from 'rxjs/operators';
import { AppEvent, EventName } from 'src/app/models/event';
import { Match } from 'src/app/models/match-model';
import { EventService } from 'src/app/services/event-service/event.service';
import { MatchService } from 'src/app/services/match-service/match.service';

@Component({
	selector: 'matches-table',
	templateUrl: './matches-table.component.html',
	styleUrls: ['./matches-table.component.scss']
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
		private matchService: MatchService) {
		this.subscriptions.set('add-match', eventService.on(EventName.AddMatch).subscribe((matchDetails: Match) => {
			this.subscriptions.set('match-added', matchService.addMatch(matchDetails).subscribe(() => {
				this.refreshPolling();
			}));
			eventService.emit(new AppEvent(EventName.CloseSidenav));
		}));

		this.subscriptions.set('edit-match', eventService.on(EventName.EditMatch).subscribe((matchDetails: Match) => {
			this.subscriptions.set('match-edited', matchService.updateMatch(matchDetails.id, matchDetails).subscribe(() => {
				this.refreshPolling();
			}));
			eventService.emit(new AppEvent(EventName.CloseSidenav));
		}));

		this.subscriptions.set('delete-match', eventService.on(EventName.DeleteMatch).subscribe((matchId: number) => {
			this.subscriptions.set('match-deleted', matchService.deleteMatch(matchId).subscribe(() => {
				this.refreshPolling();
				this.selection.toggle(matchId);
			}));
			eventService.emit(new AppEvent(EventName.CloseSidenav));
		}));
	}

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

	masterToggle(): void {
		if (this.isAllSelected()) {
			this.selection.clear();
			this.eventService.changeSelectedElements(this.selection.selected);
			return;
		}

		this.selection.select(...this.dataSource.data.map(m => m.id))
		this.eventService.changeSelectedElements(this.selection.selected);

	}

	toggleSelection(id: number): void {
		this.selection.toggle(id);
		this.eventService.changeSelectedElements(this.selection.selected);
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
