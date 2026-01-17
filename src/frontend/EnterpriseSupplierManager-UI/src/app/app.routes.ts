import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'suppliers',
    children: [
      {
        // Listagem: /suppliers
        path: '',
        loadComponent: () => 
          import('./features/suppliers/suppliers-page/suppliers-page.component')
            .then(m => m.SuppliersPageComponent)
      },
      {
        // Cadastro: /suppliers/new
        path: 'new',
        loadComponent: () => 
          import('./features/suppliers/supplier-form/supplier-form/supplier-form.component')
            .then(m => m.SupplierFormComponent)
      },
      { 
        // Edição: /suppliers/edit/123
        // Removemos o 'suppliers/' daqui pois já estamos dentro do contexto de suppliers
        path: 'edit/:id', 
        loadComponent: () => 
          import('./features/suppliers/supplier-form/supplier-form/supplier-form.component')
            .then(m => m.SupplierFormComponent)
      }
    ]
  },
  {
    path: 'companies',
    children: [
      {
        path: '',
        loadComponent: () => import('./features/companies/company-page/company-page.component').then(m => m.CompanyPageComponent)
      },
      {
        path: 'new',
        loadComponent: () => import('./features/companies/company-form/company-form.component').then(m => m.CompanyFormComponent)
      },
      {
        path: 'edit/:id',
        loadComponent: () => import('./features/companies/company-form/company-form.component').then(m => m.CompanyFormComponent)
      }
    ]
  },

  {
    path: '',
    redirectTo: 'suppliers',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'suppliers'
  }
];