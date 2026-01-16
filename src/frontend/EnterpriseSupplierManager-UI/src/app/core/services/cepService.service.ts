import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {ViaCepResponse} from '../../shared/models/viacep-response.model'
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CepService {
  constructor(private http: HttpClient) {}

  consultarCep(cep: string): Observable<ViaCepResponse> {
    const cleanCep = cep.replace(/\D/g, '');
    
    // Validação básica antes de chamar a rede
    if (cleanCep.length !== 8) {
       // Você pode tratar isso aqui ou no componente
    }

    return this.http.get<ViaCepResponse>(`https://viacep.com.br/ws/${cleanCep}/json/`);
  }
}