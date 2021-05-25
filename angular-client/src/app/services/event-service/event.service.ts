import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class EventService {

	selectedGridElems = 0;
	selectedGridElements: BehaviorSubject<number>;

	constructor() {
		this.selectedGridElements = new BehaviorSubject(this.selectedGridElems)
	}

	changeSelectedElementsCount(newCount: number): void {
		this.selectedGridElements.next(newCount);
	}
}
