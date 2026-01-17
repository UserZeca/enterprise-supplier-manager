import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatRadioModule } from '@angular/material/radio';

import { MatDatepickerModule } from '@angular/material/datepicker'; // Para o campo de data
import { MatNativeDateModule } from '@angular/material/core';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

import { CepService } from '../../../../core/services/cepService.service';

// Componentes Reutilizáveis

import { SupplierService } from '../../../../core/services/supplier.service';
import { CrudGridComponent } from '../../../../shared/components/crud-grid/crud-grid-page/crud-grid-page.component';
import { Supplier } from '../../../../core/models/supplier.model';

@Component({
  selector: 'app-supplier-form',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    CrudGridComponent,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    NgxMaskDirective,
  ],
  templateUrl: './supplier-form.component.html'
})
export class SupplierFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private supplierService = inject(SupplierService);
  private snackBar = inject(MatSnackBar);
  private cepService = inject(CepService);

  supplierForm!: FormGroup;
  isEditMode = false;
  supplierId?: string;

  ngOnInit(): void {
    this.initForm();
    
    // Verifica se há um ID na rota para entrar em modo de edição
    this.supplierId = this.route.snapshot.params['id'];
    if (this.supplierId) {
      debugger;
      this.isEditMode = true;
      this.loadSupplier();
    }
  }

  private initForm(): void {
    this.supplierForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      document: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      cep: ['', [Validators.required]],
      type: ['PJ', [Validators.required]],
      rg: [''],
      birthDate: [null]
    });

    this.supplierForm.get('type')?.valueChanges.subscribe(type => {
      this.updateValidators(type);
    });
  }

  private updateValidators(type: 'PF' | 'PJ'): void {
    const birthDateControl = this.supplierForm.get('birthDate');
    const rgControl = this.supplierForm.get('rg');

    if (type === 'PF') {
      // Se for Física, os campos podem se tornar obrigatórios ou apenas visíveis
      birthDateControl?.setValidators([Validators.required]);
    } else {
      // Se for Jurídica, limpamos os valores e validadores
      birthDateControl?.clearValidators();
      birthDateControl?.setValue(null);
      rgControl?.setValue('');
    }
    
    birthDateControl?.updateValueAndValidity();
  }

  searchCep(): void {
    const cepValue = this.supplierForm.get('cep')?.value;

    if (!cepValue || cepValue.length < 8) return;

    this.cepService.consultarCep(cepValue).subscribe({
      next: (data) => {
        if (data.erro) {
          this.snackBar.open('CEP não encontrado.', 'Fechar', { duration: 3000 });
          return;
        }

        this.supplierForm.patchValue({
          street: data.logradouro,
          neighborhood: data.bairro,
          city: data.localidade,
          state: data.uf
        });
      },
      error: () => this.snackBar.open('Erro ao buscar o endereço.', 'Fechar', { duration: 3000 })
    });
  }


  private prepareRequestData(): Supplier {
    debugger;
    const formValue = this.supplierForm.value;

    const cleanDocument = formValue.document ? formValue.document.replace(/\D/g, '') : '';
    const cleanCep = formValue.cep ? formValue.cep.replace(/\D/g, '') : '';

    return {
      name: formValue.name,
      type: formValue.type,
      document: cleanDocument, // Envia apenas números
      email: formValue.email,
      cep: cleanCep,           // Envia apenas números
      rg: formValue.type === 'PF' ? formValue.rg : null,
      birthDate: formValue.type === 'PF' ? formValue.birthDate : null
    };
  }

  private loadSupplier(): void {
    this.supplierService.getById(this.supplierId!).subscribe({
      next: (supplier: any) => {
        // 1. Identifica o tipo (prioriza o campo 'type', senão infere pelo documento)
        // Se o documento tiver mais de 11 caracteres, assume PJ, senão PF.
        const inferredType = supplier.type || (supplier.document?.length > 11 ? 'PJ' : 'PF');

        // 2. Alimenta o formulário
        this.supplierForm.patchValue({
          ...supplier,
          type: inferredType, // Isso seleciona o Radio Button automaticamente
          birthDate: supplier.birthDate ? new Date(supplier.birthDate) : null
        });

        // 3. Executa a lógica de validadores para PF/PJ
        this.updateValidators(inferredType);

        // 4. Marca como "tocado" para o Angular validar o estado do botão Salvar
        this.supplierForm.markAllAsTouched();
      },
      error: () => {
        this.snackBar.open('Erro ao carregar dados do fornecedor.', 'Fechar', { duration: 3000 });
      }
    });
  }

  onSubmit(): void {
    if (this.supplierForm.invalid) return;

    
    const supplierData = this.prepareRequestData();

    const operation$ = this.isEditMode 
      ? this.supplierService.update(this.supplierId!, supplierData)
      : this.supplierService.create(supplierData);

    operation$.subscribe({
      next: () => {
        this.snackBar.open('Fornecedor salvo com sucesso!', 'Fechar', { duration: 3000 });
        this.router.navigate(['/suppliers']);
      },
      error: (err) => {

        const errorMessage = err.error?.errors 
          ? Object.values(err.error.errors).flat().join(' | ') 
          : 'Erro desconhecido ao salvar';

        this.snackBar.open('Erro ao salvar fornecedor!\n' + errorMessage, 'Fechar', { duration: 10000 });
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/suppliers']);
  }
}