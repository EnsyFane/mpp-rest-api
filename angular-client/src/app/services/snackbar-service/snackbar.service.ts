import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ErrorInfoComponent, SnackbarDetails } from 'src/app/components/error-info/error-info.component';

@Injectable({
	providedIn: 'root'
})
export class SnackbarService {

	constructor(private snackbar: MatSnackBar) { }

	displayErrorSnackbar(message: string, description: string = '', duration: number = 5000): void {
		const config = {
			data: new SnackbarDetails(message, description),
			duration,
			panelClass: ['default-snackbar']
		}

		this.snackbar.openFromComponent(ErrorInfoComponent, config)
	}

	dismissSnackbar(): void {
		this.snackbar.dismiss();
	}
}
