import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ChatpoolComponent } from './chatpool/chatpool.component';
import { SearchComponent } from './search/search.component';

const routes: Routes = [
  {path: "app", component: AppComponent},
  {path: "search", component: SearchComponent},
  {path: "chat", component: ChatpoolComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
