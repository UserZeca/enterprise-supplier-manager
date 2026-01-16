import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { GlassCardComponent } from '../../glass-card/glass-card.component';

@Component({
  selector: 'app-crud-grid',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule, GlassCardComponent],
  templateUrl: './crud-grid-page.component.html',
  styleUrl: './crud-grid-page.component.scss'
})
export class CrudGridComponent {
  @Input({ required: true }) title: string = '';
  @Input() subtitle: string = '';
  @Input() btnLabel: string = '';
  @Input() btnIcon: string = 'add';

  @Output() btnClick = new EventEmitter<void>();
}