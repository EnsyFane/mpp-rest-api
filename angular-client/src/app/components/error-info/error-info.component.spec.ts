import { CommonModule } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

import { ErrorInfoComponent, SnackbarDetails } from './error-info.component';

fdescribe('ErrorInfoComponent', () => {
	let component: ErrorInfoComponent;
	let fixture: ComponentFixture<ErrorInfoComponent>;
	let mockSnackbarRef: jasmine.SpyObj<MatSnackBarRef<ErrorInfoComponent>>;

	async function configureTests(snacbarDetails: SnackbarDetails): Promise<void> {
		mockSnackbarRef = jasmine.createSpyObj(['dismiss']);

		const testBed = TestBed.configureTestingModule({
			declarations: [ErrorInfoComponent],
			providers: [
				{ provide: MatSnackBarRef, usevValue: mockSnackbarRef },
				{ provide: MAT_SNACK_BAR_DATA, useValue: snacbarDetails }
			],
			imports: [
				CommonModule,
				MatButtonModule,
				MatExpansionModule,
				MatIconModule,
				MatSnackBarModule
			]
		});
		await testBed.compileComponents();

		fixture = TestBed.createComponent(ErrorInfoComponent);
		fixture.detectChanges();
		component = fixture.componentInstance;
	}


	it('should create', async () => {
		await configureTests(new SnackbarDetails('main-text'));
		expect(component).toBeTruthy();
	});

	describe('Simple message with no details', () => {
		const message = new SnackbarDetails('main-text');

		beforeEach(async () => {
			await configureTests(message);
		});

		it('should have the correct main message', () => {

		})
	})
});
