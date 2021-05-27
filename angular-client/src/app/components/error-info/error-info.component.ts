import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
	selector: 'app-error-info',
	templateUrl: './error-info.component.html',
	styleUrls: ['./error-info.component.scss']
})
export class ErrorInfoComponent {
	mainMessage: string;
	details?: string;

	panelOpenState = false;

	constructor(
		private snacbarRef: MatSnackBarRef<ErrorInfoComponent>,
		@Inject(MAT_SNACK_BAR_DATA) data: any) {
		this.mainMessage = data.main;
		this.details = data.details;
	}

	dismiss(): void {
		this.snacbarRef.dismiss();
	}
}
