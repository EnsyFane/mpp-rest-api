import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { SnackbarService } from '../snackbar-service/snackbar.service';
import { catchError } from 'rxjs/operators'
import { Match } from 'src/app/models/match-model';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Injectable({
	providedIn: 'root'
})
export class MatchService {
	private hubConnection: HubConnection;

	constructor(
		private http: HttpClient,
		private snackbarService: SnackbarService
	) { }

	subscribe(): Observable<any> {
		this.hubConnection = new HubConnectionBuilder()
			.withUrl("https://localhost:5001/basketball")
			.build();

		this.hubConnection.start()
			.then(() => console.log('Connection started'))
			.catch(err => console.error('Error starting connection: ' + err));

		this.hubConnection.on('transferdata', (data) => {
			console.log('d' + data);
		});

		return this.http.get(`basketball/matches/subscribe`)
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to get match by id failed with error code ${error.status}.`, null);
				})
			);
	}

	getMatchById(id: number): Observable<any> {
		return this.http.get(`basketball/matches/${id}`)
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to get match by id failed with error code ${error.status}.`, null);
				})
			);
	}

	getMatches(): Observable<any> {
		return this.http.get('basketball/matches')
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to get matches failed with error code ${error.status}.`, []);
				})
			);
	}

	addMatch(match: Match): Observable<any> {
		const body = {
			homeTeam: match.homeTeam,
			guestTeam: match.guestTeam,
			matchType: match.matchType,
			availableSeats: match.availableSeats,
			ticketPrice: match.ticketPrice,
		};
		return this.http.post('basketball/matches', body)
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to add match failed with error code ${error.status}.`, null);
				})
			);
	}

	updateMatch(id: number, match: Match): Observable<any> {
		const body = {
			homeTeam: match.homeTeam,
			guestTeam: match.guestTeam,
			matchType: match.matchType,
			availableSeats: match.availableSeats,
			ticketPrice: match.ticketPrice,
		};
		return this.http.put(`basketball/matches/${id}`, body)
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to update match failed with error code ${error.status}.`, null);
				})
			);
	}

	deleteMatch(id: number): Observable<any> {
		return this.http.delete(`basketball/matches/${id}`)
			.pipe(
				catchError((error: any) => {
					return this.handleHttpError(`The request to delete match failed with error code ${error.status}.`, null);
				})
			);
	}

	private handleHttpError(error: string, toReturn: any): Observable<any> {
		this.snackbarService.displayErrorSnackbar(error);
		return of(toReturn);
	}
}
