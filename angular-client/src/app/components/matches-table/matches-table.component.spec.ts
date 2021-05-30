import { HttpClient } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FakeHttpClient } from 'src/app/testing/fakes/fake-http-client';

import { MatchesTableComponent } from './matches-table.component';

describe('MatchesTableComponent', () => {
	let component: MatchesTableComponent;
	let fixture: ComponentFixture<MatchesTableComponent>;
	const fakeHttpClient: FakeHttpClient = new FakeHttpClient();

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			declarations: [MatchesTableComponent],
			providers: [
				{ provide: HttpClient, useValue: fakeHttpClient }
			],
			imports: [
				MatSnackBarModule
			]
		}).compileComponents();
	});

	beforeEach(() => {
		fixture = TestBed.createComponent(MatchesTableComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
