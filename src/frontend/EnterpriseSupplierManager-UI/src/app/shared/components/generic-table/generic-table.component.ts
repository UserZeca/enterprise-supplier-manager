import { Component, Input, Output, EventEmitter, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

import { DocumentMaskPipe } from '../../pipes/document-mask.pipe';
import { PersonTypePipe } from '../../pipes/type.pipe';
/**
 * Interface para definir a configuração das colunas da tabela.
 */
export interface TableColumn {
  key: string;     
  label: string;    
  type?: 'text' | 'badge' | 'date'| 'document' | 'type'; 
  dataKey?: string;
}

@Component({
  selector: 'app-generic-table',
  standalone: true,
 
  imports: [
    CommonModule, 
    MatTableModule, 
    MatButtonModule, 
    MatIconModule,
    MatTooltipModule,
    DocumentMaskPipe,
    PersonTypePipe
  ],
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.scss'
})
export class GenericTableComponent<T> {
  /**
   * Dados brutos que vêm da página.
   */
  @Input({ required: true }) data: T[] = [];

  /**
   * Configuração das colunas que define o que será exibido e como.
   */
  @Input({ required: true }) columns: TableColumn[] = [];

  /**
   * Eventos de saída para que a página decida o que fazer (abrir modal, deletar, etc).
   */
  @Output() edit = new EventEmitter<T>();
  @Output() delete = new EventEmitter<T>();
  @Output() view = new EventEmitter<T>();

  /**
   * Lógica dinâmica para definir a ordem das colunas.
   */
  get displayedColumns(): string[] {
    const columnKeys = this.columns.map(col => col.key);
    return [...columnKeys, 'actions'];
  }

  // Métodos auxiliares para emissão de eventos
  onEdit(element: T): void {
    this.edit.emit(element);
  }

  onDelete(element: T): void {
    this.delete.emit(element);
  }

  onView(element: T): void {
    this.view.emit(element);
  }
}