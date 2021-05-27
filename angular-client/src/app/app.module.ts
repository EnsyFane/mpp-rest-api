import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { MatchesTableComponent } from './components/matches-table/matches-table.component';
import { MaterialModule } from './material-module';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { EventService } from './services/event-service/event.service';
import { MatchService } from './services/match-service/match.service';
import { ErrorInfoComponent } from './components/error-info/error-info.component';
import { SnackbarService } from './services/snackbar-service/snackbar.service';
import { RestApiMatchesComponent } from './components/rest-api-matches/rest-api-matches.component';
import { SidenavWrapperComponent } from './components/sidenav/sidenav-wrapper/sidenav-wrapper.component';
import { DeleteMatchDialogComponent } from './components/delete-match-dialog/delete-match-dialog.component';

@NgModule({
	declarations: [
		AppComponent,
		HeaderComponent,
		MatchesTableComponent,
		ToolbarComponent,
		ErrorInfoComponent,
		RestApiMatchesComponent,
		SidenavWrapperComponent,
  DeleteMatchDialogComponent
	],
	imports: [
		AppRoutingModule,
		BrowserModule,
		BrowserAnimationsModule,
		HttpClientModule,
		MaterialModule,
		ReactiveFormsModule
	],
	providers: [
		EventService,
		MatchService,
		SnackbarService
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
