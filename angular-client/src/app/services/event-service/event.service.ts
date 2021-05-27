import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { AppEvent, EventName } from 'src/app/models/event';

@Injectable({
	providedIn: 'root'
})
export class EventService {
	private subject = new Subject();
	private selectedGridElems: number[] = [];
	private selectedGridElements: BehaviorSubject<number[]>;

	constructor() {
		this.selectedGridElements = new BehaviorSubject(this.selectedGridElems)
	}

	emit(event: AppEvent): void {
		this.subject.next(event);
	}

	on(eventName: EventName): Observable<any> {
		return this.subject.pipe(
			filter((e: AppEvent) => eventName === e.name),
			map((e: AppEvent) => e.payload)
		);
	}

	changeSelectedElements(selections: number[]): void {
		this.selectedGridElements.next(selections);
	}

	onSelectedElementsChanged(): Observable<number[]> {
		return this.selectedGridElements;
	}
}
