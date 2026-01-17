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

import { CepService } from '../../../core/services/cepService.service';

// Componentes Reutilizáveis

import { CompanyService } from '../../../core/services/company.service';
import { CrudGridComponent } from '../../../shared/components/crud-grid/crud-grid-page/crud-grid-page.component';
import { Company } from '../../../core/models/company.model';

@Component({
  selector: 'app-company-form',
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
  templateUrl: './company-form.component.html'
})
export class CompanyFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private companyService = inject(CompanyService);
  private snackBar = inject(MatSnackBar);
  private cepService = inject(CepService);

  companyForm!: FormGroup;
  isEditMode = false;
  companyId?: string;

  ngOnInit(): void {
    this.initForm();
    
    // Verifica se há um ID na rota para entrar em modo de edição
    this.companyId = this.route.snapshot.params['id'];
    if (this.companyId) {
      debugger;
      this.isEditMode = true;
      this.loadCompany();
    }
  }

  private initForm(): void {
    this.companyForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      document: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      cep: ['', [Validators.required]],
      type: ['PJ', [Validators.required]],
      rg: [''],
      birthDate: [null]
    });

    this.companyForm.get('type')?.valueChanges.subscribe(type => {
      this.updateValidators(type);
    });
  }

  private updateValidators(type: 'PF' | 'PJ'): void {
    const birthDateControl = this.companyForm.get('birthDate');
    const rgControl = this.companyForm.get('rg');

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
    const cepValue = this.companyForm.get('cep')?.value;

    if (!cepValue || cepValue.length < 8) return;

    this.cepService.consultarCep(cepValue).subscribe({
      next: (data) => {
        if (data.erro) {
          this.snackBar.open('CEP não encontrado.', 'Fechar', { duration: 3000 });
          return;
        }

        this.companyForm.patchValue({
          street: data.logradouro,
          neighborhood: data.bairro,
          city: data.localidade,
          state: data.uf
        });
      },
      error: () => this.snackBar.open('Erro ao buscar o endereço.', 'Fechar', { duration: 3000 })
    });
  }


  private prepareRequestData(): Company {
    debugger;
    const formValue = this.companyForm.value;

    const cleanDocument = formValue.document ? formValue.document.replace(/\D/g, '') : '';
    const cleanCep = formValue.cep ? formValue.cep.replace(/\D/g, '') : '';

    return {
      name: formValue.name,
      document: cleanDocument, // Envia apenas números
      email: formValue.email,
      cep: cleanCep,           // Envia apenas números
    };
  }

  private loadCompany(): void {
    this.companyService.getById(this.companyId!).subscribe({
      next: (company: any) => {
        // 1. Identifica o tipo (prioriza o campo 'type', senão infere pelo documento)
        // Se o documento tiver mais de 11 caracteres, assume PJ, senão PF.
        const inferredType = company.type || (company.document?.length > 11 ? 'PJ' : 'PF');

        // 2. Alimenta o formulário
        this.companyForm.patchValue({
          ...company,
          type: inferredType, // Isso seleciona o Radio Button automaticamente
          birthDate: company.birthDate ? new Date(company.birthDate) : null
        });

        // 3. Executa a lógica de validadores para PF/PJ
        this.updateValidators(inferredType);

        // 4. Marca como "tocado" para o Angular validar o estado do botão Salvar
        this.companyForm.markAllAsTouched();
      },
      error: () => {
        this.snackBar.open('Erro ao carregar dados da Empresa.', 'Fechar', { duration: 3000 });
      }
    });
  }

  onSubmit(): void {
    if (this.companyForm.invalid) return;

    
    const companyData = this.prepareRequestData();

    const operation$ = this.isEditMode 
      ? this.companyService.update(this.companyId!, companyData)
      : this.companyService.create(companyData);

    operation$.subscribe({
      next: () => {
        this.snackBar.open('Empresa salva com sucesso!', 'Fechar', { duration: 3000 });
        this.router.navigate(['/companies']);
      },
      error: (err) => {

        const errorMessage = err.error?.errors 
          ? Object.values(err.error.errors).flat().join(' | ') 
          : 'Erro desconhecido ao salvar';

        this.snackBar.open('Erro ao salvar empresa!\n' + errorMessage, 'Fechar', { duration: 10000 });
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/companies']);
  }
}