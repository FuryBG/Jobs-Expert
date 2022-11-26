import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ChatServiceService  {

  participants: any = {};
  participants$ = new BehaviorSubject(this.participants);

  constructor(private httpClient: HttpClient, private userService: UserService) {

    this.GetUser().subscribe({
      next: (value) => {
        console.log(value);
        this.participants = value;
      }
    });
  }

  public _hubConnection?: HubConnection;


  public async connect(): Promise<void> {
    this._hubConnection = new HubConnectionBuilder().withUrl("https://localhost:7285/chat").build();

    this._hubConnection.on('MessageReceived', (message) => {
      this.participants?.conversationinfo.push({userId: message.userId, text: message.text, roomId: message.roomId, userName: message.userName, created: message.created});
      console.log(this.participants.conversationinfo);
      
    });

    await this._hubConnection.start()
    console.log('connection started');
  }

  GetUser() {
    let result = this.httpClient.get("https://localhost:7285/rooms");
    return result;
  }

  
}
