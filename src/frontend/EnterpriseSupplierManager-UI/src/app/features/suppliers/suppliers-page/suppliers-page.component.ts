import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrudGridComponent } from '../../../shared/components/crud-grid/crud-grid-page/crud-grid-page.component';
import { Supplier } from '../../../core/models/supplier.model';
import { GenericTableComponent, TableColumn } from '../../../shared/components/generic-table/generic-table.component';
import { Router } from '@angular/router';

// Importação da lógica de negócio
import { SupplierService } from '../../../core/services/supplier.service';

@Component({
  selector: 'app-suppliers-page',
  standalone: true,
  imports: [CommonModule, CrudGridComponent, GenericTableComponent],
  templateUrl: './suppliers-page.component.html'
})
export class SuppliersPageComponent implements OnInit {
  
  private readonly router = inject(Router);
  protected readonly supplierService = inject(SupplierService);

  /**
   * Definição das colunas específica para o domínio de Fornecedores.

   */
  protected readonly columns: TableColumn[] = [
    { key: 'name', label: 'Fornecedor' },
    { key: 'document', label: 'Documento', type: 'document' },
    { key: 'email', label: 'E-mail de Contato' },
    { 
      key: 'type', // ID Único para evitar o erro de duplicidade
      dataKey: 'document', // Aponta para a propriedade 'document' do objeto
      label: 'Tipo', 
      type: 'type' 
  } 
  ];

  ngOnInit(): void {
    // Dispara a busca inicial de dados no backend .NET
    this.supplierService.getAll().subscribe();
  }

  onAddSupplier(): void {
    this.router.navigate(['/suppliers/new']);
  }
  // Handlers para os eventos emitidos pela tabela genérica
  
  onViewSupplier(supplier: Supplier): void {
    console.log('Visualizando fornecedor:', supplier);
    // Aqui você implementaria a navegação para a tela de detalhes
  }

  onEditSupplier(supplier: Supplier): void {
    this.router.navigate(['/suppliers/edit', supplier.id]);
  }

  onDeleteSupplier(supplier: Supplier): void {
    if (!supplier.id) {
      console.error('Não é possível excluir: Fornecedor sem ID.');
      return;
    }

    if (confirm(`Deseja realmente excluir o fornecedor ${supplier.name}?`)) {
      this.supplierService.delete(supplier.id).subscribe({
        next: () => console.log('Excluído com sucesso'),
        error: (err) => console.error('Erro ao excluir', err)
      });
    }
  }
}