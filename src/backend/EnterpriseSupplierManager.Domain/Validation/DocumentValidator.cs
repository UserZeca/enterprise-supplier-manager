using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseSupplierManager.Domain.Validation
{
    public static class DocumentValidator
    {
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            string sanitizedCpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (sanitizedCpf.Length != 11 || IsRepeatedNumbers(sanitizedCpf))
                return false;

            int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = sanitizedCpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += digit1;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return sanitizedCpf.EndsWith(digit1.ToString() + digit2.ToString());
        }

        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;

            // Remove caracteres não numéricos
            string sanitizedCnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Validação básica de tamanho e sequências repetidas
            if (sanitizedCnpj.Length != 14 || IsRepeatedNumbers(sanitizedCnpj))
                return false;

            // Cálculo dos Dígitos Verificadores
            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = sanitizedCnpj.Substring(0, 12);

            // Primeiro dígito
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += digit1;

            // Segundo dígito
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return sanitizedCnpj.EndsWith(digit1.ToString() + digit2.ToString());
        }

        private static bool IsRepeatedNumbers(string text)
        {
            return text.Distinct().Count() == 1;
        }
    }
}
