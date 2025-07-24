import { Component, EventEmitter, Output } from '@angular/core';
import { trigger, style, transition, animate } from '@angular/animations';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-info-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './info-modal.html',
  styleUrls: ['./info-modal.css'],
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
export class InfoModalComponent {
  message: string = '';
  visible: boolean = false;

  @Output() closed = new EventEmitter<void>();

  show(message: string) {
    this.message = message;
    this.visible = true;
  }

  close() {
    this.visible = false;
    this.closed.emit();
  }
}
