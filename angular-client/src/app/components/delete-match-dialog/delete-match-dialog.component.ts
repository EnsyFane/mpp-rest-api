import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
	selector: 'app-delete-match-dialog',
	templateUrl: './delete-match-dialog.component.html',
	styleUrls: ['./delete-match-dialog.component.scss']
})
export class DeleteMatchDialogComponent {

	constructor(
		public dialogRef: MatDialogRef<DeleteMatchDialogComponent>
	) { }

	onCancel(): void {
		this.dialogRef.close();
	}
}
