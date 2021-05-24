import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { MatchesTableComponent } from './components/matches-table/matches-table.component';
import { MaterialModule } from './material-module';


@NgModule({
	declarations: [
		AppComponent,
		HeaderComponent,
		MatchesTableComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		MaterialModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
