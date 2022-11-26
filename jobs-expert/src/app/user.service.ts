import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  
  userInfo: any = null;

  constructor(private httpClient: HttpClient) {
    this.GetUserForLocalStorage().subscribe({
      next: (value) => {
        localStorage.setItem("user", JSON.stringify(value))
        this.userInfo = value
      }
    })
   }

  GetUserForLocalStorage() {
    let result = this.httpClient.get("https://localhost:7285/userlocalinfo");
    return result;
  }
}
