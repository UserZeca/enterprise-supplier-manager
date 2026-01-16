import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CompanyService } from '../../../core/services/company.service';
import { TableColumn } from '../../../shared/components/generic-table/generic-table.component';
import { CrudGridComponent } from '../../../shared/components/crud-grid/crud-grid-page/crud-grid-page.component';
import { GenericTableComponent } from '../../../shared/components/generic-table/generic-table.component';

@Component({
  selector: 'app-company-page',
  standalone: true,
  imports: [CrudGridComponent, GenericTableComponent],
  template: `
    <app-crud-grid 
      title="Gestão de Empresas"
      subtitle="Administre o cadastro centralizado de unidades de negócio."
      btnLabel="Nova Empresa"
      (btnClick)="onAdd()">
      <app-generic-table 
        [data]="companyService.companies()" 
        [columns]="columns"
        (edit)="onEdit($event)"
        (delete)="onDelete($event)">
      </app-generic-table>
    </app-crud-grid>
  `
})
export class CompanyPageComponent implements OnInit {
  protected readonly companyService = inject(CompanyService);
  private readonly router = inject(Router);

  protected readonly columns: TableColumn[] = [
    { key: 'name', label: 'Empresa' },
    { key: 'document', label: 'CNPJ', type: 'document' },
    { key: 'email', label: 'E-mail' },
  ];

  ngOnInit() { this.companyService.getAll().subscribe(); }

  onAdd() { this.router.navigate(['/companies/new']); }
  onEdit(company: any) { this.router.navigate(['/companies/edit', company.id]); }
  onDelete(company: any) {
    if (confirm('Excluir empresa?')) this.companyService.delete(company.id).subscribe();
  }
}