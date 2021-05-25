import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { MatchesTableComponent } from './components/matches-table/matches-table.component';
import { MaterialModule } from './material-module';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { EventService } from './services/event-service/event.service';


@NgModule({
	declarations: [
		AppComponent,
		HeaderComponent,
		MatchesTableComponent,
		ToolbarComponent
	],
	imports: [
		AppRoutingModule,
		BrowserModule,
		BrowserAnimationsModule,
		HttpClientModule,
		MaterialModule,
		ReactiveFormsModule
	],
	providers: [EventService],
	bootstrap: [AppComponent]
})
export class AppModule { }
