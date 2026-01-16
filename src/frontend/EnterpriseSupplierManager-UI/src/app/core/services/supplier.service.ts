import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Supplier } from '../models/supplier.model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/suppliers`;

  // Signals para gerenciamento de estado
  suppliers = signal<Supplier[]>([]);
  loading = signal<boolean>(false);

  // Método para buscar todos os fornecedores
  getAll(): Observable<Supplier[]> {
    this.loading.set(true);
    return this.http.get<Supplier[]>(this.apiUrl).pipe(
      tap({
        next: (data) => {
          debugger;
          this.suppliers.set(data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      })
    );
  }

  create(supplier: Partial<Supplier>): Observable<Supplier> {
    this.loading.set(true);
    return this.http.post<Supplier>(this.apiUrl, supplier).pipe(
      tap(newSupplier => {
        // Adiciona o novo fornecedor ao Signal local
        this.suppliers.update(list => [...list, newSupplier]);
        this.loading.set(false);
      })
    );
  }

  update(id: string, supplier: Partial<Supplier>): Observable<Supplier> {
    this.loading.set(true);
    return this.http.put<Supplier>(`${this.apiUrl}/${id}`, supplier).pipe(
      tap(updatedSupplier => {
        // Atualiza apenas o item editado no Signal
        this.suppliers.update(list => 
          list.map(s => s.id === id ? updatedSupplier : s)
        );
        this.loading.set(false);
      })
    );
  }

  delete(id: string): Observable<void> { 
    this.loading.set(true);
  
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => {
        this.suppliers.update(list => list.filter(s => s.id !== id));
        this.loading.set(false);
      })
    );
  }

}