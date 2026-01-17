import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'documentMask',
  standalone: true // Mantendo o padrão de componentes independentes
})
export class DocumentMaskPipe implements PipeTransform {
  transform(value: string | number | undefined): string {
    if (!value) return '';

    // Remove qualquer caractere que não seja número para garantir a formatação
    const rawValue = value.toString().replace(/\D/g, '');

    if (rawValue.length === 11) {
      // Formato CPF: 000.000.000-00
      return rawValue.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    } 
    
    if (rawValue.length === 14) {
      // Formato CNPJ: 00.000.000/0000-00
      return rawValue.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
    }

    return rawValue; // Retorna o valor limpo caso não se encaixe nos padrões
  }
}