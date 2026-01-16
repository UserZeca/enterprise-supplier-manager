export interface Supplier {
  id?: string;
  name: string;
  document: string;
  email: string;
  type: 'PF' | 'PJ';
  rg?: string;
  birthDate?: Date;
  cep: string;
}