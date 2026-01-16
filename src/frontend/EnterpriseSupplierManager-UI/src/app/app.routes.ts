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
      /*{
        // Edição: /suppliers/edit/123
        path: 'edit/:id',
        loadComponent: () => 
          import('./features/suppliers/supplier-form/supplier-form.component')
            .then(m => m.SupplierFormComponent)
      }*/
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