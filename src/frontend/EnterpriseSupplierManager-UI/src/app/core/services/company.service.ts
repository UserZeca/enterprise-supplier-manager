import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Company } from '../models/company.model';
import { Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CompanyService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/companies`;

  companies = signal<Company[]>([]);
  loading = signal<boolean>(false);

   getById(id: string): Observable<Company> {
     return this.http.get<Company>(`${this.apiUrl}/${id}`);
   }

    getAll(): Observable<Company[]> {
      this.loading.set(true);
      return this.http.get<Company[]>(this.apiUrl).pipe(
        tap({
          next: (data) => {
            debugger;
            this.companies.set(data);
            this.loading.set(false);
          },
          error: () => this.loading.set(false)
        })
      );
    }
  
    create(company: Partial<Company>): Observable<Company> {
      this.loading.set(true);
      return this.http.post<Company>(this.apiUrl, company).pipe(
        tap(newCompany => {
          // Adiciona o novo fornecedor ao Signal local
          this.companies.update(list => [...list, newCompany]);
          this.loading.set(false);
        })
      );
    }
  
    update(id: string, company: Partial<Company>): Observable<Company> {
      this.loading.set(true);
      return this.http.put<Company>(`${this.apiUrl}/${id}`, company).pipe(
        tap(updatedCompany => {
          // Atualiza apenas o item editado no Signal
          this.companies.update(list => 
            list.map(s => s.id === id ? updatedCompany : s)
          );
          this.loading.set(false);
        })
      );
    }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.companies.update(list => list.filter(c => c.id !== id)))
    );
  }

  // Adicione create e update conforme necessário
}