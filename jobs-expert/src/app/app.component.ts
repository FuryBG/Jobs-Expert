import { Component, OnDestroy } from '@angular/core';
import { ChatServiceService } from './chat-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'jobs-expert';

  constructor(private chatService: ChatServiceService) { 
    chatService.connect();
    window.onbeforeunload = function(e) {
      console.log("destroyed");
      chatService._hubConnection?.stop();
    };
  }


}