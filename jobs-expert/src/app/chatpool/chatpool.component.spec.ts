import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatpoolComponent } from './chatpool.component';

describe('ChatpoolComponent', () => {
  let component: ChatpoolComponent;
  let fixture: ComponentFixture<ChatpoolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatpoolComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatpoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
