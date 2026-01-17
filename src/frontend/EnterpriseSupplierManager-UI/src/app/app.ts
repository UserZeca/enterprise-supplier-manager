import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { JsonPipe } from '@angular/common'; // Importante para debugar
import { SupplierService } from './core/services/supplier.service';

@Component({
  selector: 'app-root',
  standalone: true,
  // Adicionamos o JsonPipe nos imports para visualizar o objeto no HTML
  imports: [RouterOutlet, JsonPipe], 
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  
  protected readonly supplierService = inject(SupplierService);
  protected readonly title = signal('EnterpriseSupplierManager-UI');

  ngOnInit(): void {
    
    this.supplierService.getAll().subscribe({
      error: (err) => console.error('Erro ao conectar com a API:', err)
    });
  }
}