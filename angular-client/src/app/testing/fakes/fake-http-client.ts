import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export class FakeHttpClient {
    private matches: any;

    constructor() {
        this.matches = [];
    }

    get(url: string): Observable<any> {
        if (this.matchesValue(/basketball\/matches$/i, url)) {
            return of(this.matches);
        }

        return of(this.notFoundResponse());
    }

    private matchesValue(regex: RegExp, url: string): false | RegExpMatchArray {
        const match = url.match(regex);
        if (!match) {
            return false;
        }
        return match;
    }

    private notFoundResponse(): HttpErrorResponse {
        return new HttpErrorResponse({
            status: 404
        });
    }
}