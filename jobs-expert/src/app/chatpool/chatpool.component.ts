import { Component } from '@angular/core';
import { ChatServiceService } from '../chat-service.service';
import { UserService } from '../user.service';

@Component({
  selector: 'app-chatpool',
  templateUrl: './chatpool.component.html',
  styleUrls: ['./chatpool.component.scss']
})
export class  ChatpoolComponent {

  userInfo: any = null;
  participants: any;
  conversation: any = null;

  constructor(private chatService: ChatServiceService, private userService: UserService) {
    this.participants = chatService.participants;
    userService.GetUserForLocalStorage().subscribe({
      next: (value) => {
        this.userInfo = value;
        this.conversation = this.participants[0].conversationinfo;
        console.log(this.participants);
        
      }
    });
   }

   
  sendMessage(valueElement: HTMLInputElement) {
    if(valueElement.value != "") {
      this.conversation.push({roomId: this.participants.roominfo[0].roomId, userId: this.userInfo.userId, text: valueElement.value, userName: this.userInfo.firstName});
      this.chatService._hubConnection?.invoke("SendMessage", {"RoomId": this.conversation[0].roomId, "UserId": this.participants.roominfo[0].userId, "userName": this.userInfo.firstName, "Message": valueElement.value});
    }
    valueElement.value = "";
  }

  SelectChat(roomId: any) {
    console.log(roomId);
    this.participants.forEach((x: any) => {
      if(x.roominfo.roomId == roomId) {
        this.conversation = x.conversationinfo;
      }
    });
  }

}
