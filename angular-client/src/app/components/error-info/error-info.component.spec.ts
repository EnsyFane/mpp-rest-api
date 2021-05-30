import { CommonModule } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ErrorInfoPageObject } from '../../testing/page-objects/error-info.pageobject';

import { ErrorInfoComponent, SnackbarDetails } from './error-info.component';

describe('ErrorInfoComponent', () => {
	let component: ErrorInfoComponent;
	let fixture: ComponentFixture<ErrorInfoComponent>;
	let mockSnackbarRef: jasmine.SpyObj<MatSnackBarRef<ErrorInfoComponent>>;
	let page: ErrorInfoPageObject;

	async function configureTests(snackbarDetails: SnackbarDetails): Promise<void> {
		mockSnackbarRef = jasmine.createSpyObj(['dismiss']);

		const testBed = TestBed.configureTestingModule({
			declarations: [ErrorInfoComponent],
			providers: [
				{ provide: MatSnackBarRef, useValue: mockSnackbarRef },
				{ provide: MAT_SNACK_BAR_DATA, useValue: snackbarDetails }
			],
			imports: [
				CommonModule,
				MatButtonModule,
				MatExpansionModule,
				MatIconModule,
				MatSnackBarModule,
				NoopAnimationsModule
			]
		});
		await testBed.compileComponents();

		fixture = TestBed.createComponent(ErrorInfoComponent);
		fixture.detectChanges();
		component = fixture.componentInstance;
		page = new ErrorInfoPageObject(fixture.nativeElement);
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
			expect(page.getMainMessageText().trim()).toBe(message.main);
		});

		it('should call dismiss on close', () => {
			page.clickDismiss();
			expect(mockSnackbarRef.dismiss).toHaveBeenCalledTimes(1);
		});
	});

	describe('Message with details', () => {
		const message = new SnackbarDetails('main-text', 'details');

		beforeEach(async () => {
			await configureTests(message);
		});

		it('should have the correct main message and details', () => {
			expect(page.getMainMessageText().trim()).toBe(message.main);
			expect(page.getDescriptionMessageText().trim()).toBe(message.details);
		});

		it('should expand/minimize details panel', () => {
			expect(component.panelOpenState).toBeFalse();

			page.clickExpansionPanelHeader();
			fixture.detectChanges();

			expect(component.panelOpenState).toBeTrue();

			page.clickExpansionPanelHeader();
			fixture.detectChanges();

			expect(component.panelOpenState).toBeFalse();
		});
	});
});
