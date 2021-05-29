import { EventEmitter, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Component, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDrawer } from '@angular/material/sidenav';
import { Observable, Subscription } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { AppEvent, EventName } from 'src/app/models/event';
import { Match, MatchType } from 'src/app/models/match-model';
import { EventService } from 'src/app/services/event-service/event.service';
import { MatchService } from 'src/app/services/match-service/match.service';

@Component({
	selector: 'app-sidenav-wrapper',
	templateUrl: './sidenav-wrapper.component.html',
	styleUrls: ['./sidenav-wrapper.component.scss']
})
export class SidenavWrapperComponent implements OnInit, OnDestroy {
	@Output() onPrimaryAction: EventEmitter<Match>;
	@Output() onSecondaryAction: EventEmitter<Match>;

	sidenavTitle: string;
	sidenavPrimaryAction: string;
	sidenavSecondaryAction: string;

	@ViewChild('drawer') sidenav: MatDrawer;

	matchFormGroup = new FormGroup({
		homeTeam: new FormControl('', [Validators.required]),
		guestTeam: new FormControl('', [Validators.required]),
		matchType: new FormControl('', [Validators.required, this.validateMatchType()]),
		availableSeats: new FormControl('', [Validators.required, , Validators.min(0)]),
		ticketPrice: new FormControl('', [Validators.required, Validators.min(0)]),
	});

	matchTypes: string[] = Object.keys(MatchType);
	filteredMatchTypes: Observable<string[]>;
	matchDetails: Match;
	respondWith: EventName;

	subscriptions: Subscription[] = [];

	constructor(
		private eventService: EventService,
		private matchService: MatchService) {
		this.subscriptions.push(eventService.on(EventName.OpenSidenav).subscribe((payload: SidenavDetails) => {
			this.sidenavTitle = payload.title;
			this.sidenavPrimaryAction = payload.primaryAction;
			this.sidenavSecondaryAction = payload.secondaryAction;
			this.matchDetails = payload.match;
			this.respondWith = payload.respondWith;
			if (this.matchDetails) {
				if (this.matchDetails.id) {
					this.subscriptions.push(matchService.getMatchById(this.matchDetails.id).subscribe((match) => {
						if (match) {
							this.matchDetails = match;
							this.populateForm(this.matchDetails);
						}
					}));
				} else {
					this.populateForm(this.matchDetails);
				}
			} else {
				this.clearForm();
			}

			this.sidenav.open();
		}));
		this.subscriptions.push(eventService.on(EventName.CloseSidenav).subscribe(() => {
			this.clearForm();
			this.sidenav.close()
		}));

		this.matchFormGroup
	}

	ngOnInit(): void {
		this.filteredMatchTypes = this.matchFormGroup.get('matchType').valueChanges
			.pipe(
				startWith(''),
				map(value => this.filterMatchTypeOptions(value))
			);

		// setTimeout(() => {
		// 	const sidenavDetails = new SidenavDetails("Add match", "Add", "Cancel", EventName.AddMatch);
		// 	let appEvent = new AppEvent(EventName.OpenSidenav, sidenavDetails);
		// 	this.eventService.emit(appEvent);
		// }, 50);
	}

	ngOnDestroy(): void {
		for (const subscription of this.subscriptions) {
			subscription.unsubscribe();
		}
	}

	primaryAction(): void {
		const match = this.generateMatchFromForm();
		const event = new AppEvent(this.respondWith, match);
		this.eventService.emit(event)
	}

	secondaryAction(): void {
		const match = this.generateMatchFromForm();
		const event = new AppEvent(EventName.SidenavSecondaryAction, match);
		this.eventService.emit(event)
	}

	private generateMatchFromForm(): Match {
		const match = new Match();
		if (this.matchDetails?.id) {
			match.id = this.matchDetails.id;
		}
		match.homeTeam = this.matchFormGroup.get('homeTeam').value;
		match.guestTeam = this.matchFormGroup.get('guestTeam').value;
		match.matchType = this.matchFormGroup.get('matchType').value;
		match.availableSeats = this.matchFormGroup.get('availableSeats').value;
		match.ticketPrice = this.matchFormGroup.get('ticketPrice').value;

		return match;
	}

	private populateForm(match: Match): void {
		this.matchFormGroup.get('homeTeam').setValue(match.homeTeam);
		this.matchFormGroup.get('homeTeam').updateValueAndValidity();

		this.matchFormGroup.get('guestTeam').setValue(match.guestTeam);
		this.matchFormGroup.get('guestTeam').updateValueAndValidity();

		this.matchFormGroup.get('matchType').setValue(match.matchType);
		this.matchFormGroup.get('matchType').updateValueAndValidity();

		this.matchFormGroup.get('availableSeats').setValue(match.availableSeats);
		this.matchFormGroup.get('availableSeats').updateValueAndValidity();

		this.matchFormGroup.get('ticketPrice').setValue(match.ticketPrice);
		this.matchFormGroup.get('ticketPrice').updateValueAndValidity();
	}

	private clearForm(): void {
		this.matchFormGroup.get('homeTeam').reset();
		this.matchFormGroup.get('guestTeam').reset();
		this.matchFormGroup.get('matchType').reset();
		this.matchFormGroup.get('availableSeats').reset();
		this.matchFormGroup.get('ticketPrice').reset();
	}

	private filterMatchTypeOptions(value: string): string[] {
		if (!value) {
			return;
		}
		const filterValue = value.toLowerCase();

		return this.matchTypes.filter(option => option.toLowerCase().includes(filterValue));
	}

	private validateMatchType(): ValidatorFn {
		return (control: AbstractControl): ValidationErrors | null => {
			let invalid = true;
			if (!this.matchTypes) {
				return null;
			}
			for (const val of this.matchTypes) {
				if (val.toLowerCase().includes(control.value?.toString().toLowerCase())) {
					invalid = false
					break;
				}
			}
			return invalid ? { invalidValue: { value: control.value } } : null;
		};
	}
}

export class SidenavDetails {
	title: string;
	primaryAction: string;
	secondaryAction: string;
	respondWith: EventName;
	match?: Match;

	constructor(title: string, primaryAction: string, secondaryAction: string, respondWith: EventName, match?: Match) {
		this.title = title;
		this.primaryAction = primaryAction;
		this.secondaryAction = secondaryAction;
		this.respondWith = respondWith;
		this.match = match;
	}
}
