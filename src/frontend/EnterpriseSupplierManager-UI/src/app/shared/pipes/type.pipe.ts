import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'type',
  standalone: true
})
export class PersonTypePipe implements PipeTransform {
  transform(value: string | undefined, returnLabel: boolean = false): string {
    if (!value) return returnLabel ? 'Não Identificado' : 'unknown';

    const rawValue = value.replace(/\D/g, ''); // Limpa pontos e traços

    if (rawValue.length === 11) {
      return returnLabel ? 'Pessoa Física' : 'PF';
    } 
    if (rawValue.length === 14) {
      return returnLabel ? 'Pessoa Jurídica' : 'PJ';
    }

    return returnLabel ? 'Documento Inválido' : 'invalid';
  }
}