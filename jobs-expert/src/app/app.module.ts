import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchComponent } from './search/search.component';
import { HttpClientModule } from '@angular/common/http';
import { ChatpoolComponent } from './chatpool/chatpool.component';
import { ChatServiceService } from './chat-service.service';
import { UserService } from './user.service';

@NgModule({
  declarations: [
    AppComponent,
    SearchComponent,
    ChatpoolComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [ChatServiceService, UserService, HttpClientModule],
  bootstrap: [AppComponent]
})
export class AppModule { }
