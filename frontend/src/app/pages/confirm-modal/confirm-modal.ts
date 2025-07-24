import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirm-modal.html',
  styleUrls: ['./confirm-modal.css'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.9)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'scale(1)' })),
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0, transform: 'scale(0.9)' })),
      ]),
    ]),
  ],
})
export class ConfirmModalComponent {
 @Output() confirm = new EventEmitter<{ confirmed: boolean, context: string }>();

  visible = false;
  message = '';
  context = '';

  show(msg: string, ctx: string = '') {
  this.message = msg;
  this.context = ctx;
  this.visible = true;
}

onConfirm(): void {
  this.confirm.emit({ confirmed: true, context: this.context });
  this.visible = false;
}

onCancel(): void {
  this.confirm.emit({ confirmed: false, context: this.context });
  this.visible = false;
}
}
