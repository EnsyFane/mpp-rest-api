import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
	selector: 'app-delete-match-dialog',
	templateUrl: './delete-match-dialog.component.html',
	styleUrls: ['./delete-match-dialog.component.scss']
})
export class DeleteMatchDialogComponent {

	constructor(
		public dialogRef: MatDialogRef<DeleteMatchDialogComponent>
	) { }

	onCancel() {
		this.dialogRef.close();
	}
}
