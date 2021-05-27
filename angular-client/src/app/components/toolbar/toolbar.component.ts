import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AppEvent, EventName } from 'src/app/models/event';
import { Match } from 'src/app/models/match-model';
import { EventService } from 'src/app/services/event-service/event.service';
import { MatchService } from 'src/app/services/match-service/match.service';
import { DeleteMatchDialogComponent } from '../delete-match-dialog/delete-match-dialog.component';
import { SidenavDetails } from '../sidenav/sidenav-wrapper/sidenav-wrapper.component';

@Component({
	selector: 'matches-toolbar',
	templateUrl: './toolbar.component.html',
	styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnDestroy {

	isSingleElementSelected: boolean = false;
	selectedElementId: number;

	subscriptions: Subscription[] = [];

	constructor(
		private eventService: EventService,
		private dialog: MatDialog) {
		this.subscriptions.push(eventService.onSelectedElementsChanged().subscribe((selectedElements) => {
			this.selectedElementId = selectedElements[0] ?? -1;
			this.isSingleElementSelected = selectedElements.length === 1 && this.selectedElementId !== -1;
		}));

		this.subscriptions.push(eventService.on(EventName.SidenavSecondaryAction).subscribe(() => {
			eventService.emit(new AppEvent(EventName.CloseSidenav));
		}));
	}

	ngOnDestroy(): void {
		for (const subscription of this.subscriptions) {
			subscription.unsubscribe();
		}
	}

	addButtonClicked(event): void {
		event.currentTarget.blur();
		const sidenavDetails = new SidenavDetails("Add match", "Add", "Cancel", EventName.AddMatch);
		let appEvent = new AppEvent(EventName.OpenSidenav, sidenavDetails);
		this.eventService.emit(appEvent);
	}

	editButtonClicked(event): void {
		event.currentTarget.blur();
		const placeholderMatch = new Match();
		placeholderMatch.id = this.selectedElementId;
		const sidenavDetails = new SidenavDetails("Edit match", "Edit", "Cancel", EventName.EditMatch, placeholderMatch);
		let appEvent = new AppEvent(EventName.OpenSidenav, sidenavDetails);
		this.eventService.emit(appEvent);
	}

	deleteButtonClicked(event): void {
		event.currentTarget.blur();
		const dialogRef = this.dialog.open(DeleteMatchDialogComponent, {
			width: '450px'
		});
		this.subscriptions.push(dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.eventService.emit(new AppEvent(EventName.DeleteMatch, this.selectedElementId));
			}
		}))
	}
}
