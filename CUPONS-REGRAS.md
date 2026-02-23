# Regras de Cupons - Colinho da Cá

## Tipos de Cupons

### 1. PercentualSobreTotal (Tipo 1)
**Descrição:** Desconto percentual aplicado sobre o valor total da reserva.

**Regras:**
- Aplica o percentual definido sobre o valor total
- Não possui requisitos mínimos
- Exemplo: Cupom de 10% desconta 10% do valor total

**Campos Obrigatórios:**
- Código
- Descrição
- Tipo = 1
- Percentual (ex: 10 para 10%)

**Campos Opcionais:**
- Data Início
- Data Fim

**Exemplo de Uso:**
```
Cupom: BEMVINDO10
Descrição: 10% de desconto em qualquer reserva
Percentual: 10
Valor da Reserva: R$ 500,00
Desconto: R$ 50,00
Valor Final: R$ 450,00
```

---

### 2. PercentualPorPetComMinimo (Tipo 2)
**Descrição:** Desconto percentual aplicado quando atinge quantidade mínima de pets.

**Regras:**
- Requer quantidade mínima de pets
- Aplica o percentual sobre o valor total quando atinge o mínimo
- Se não atingir o mínimo, cupom não é aplicado

**Campos Obrigatórios:**
- Código
- Descrição
- Tipo = 2
- Percentual (ex: 15 para 15%)
- Mínimo de Pets (ex: 2)

**Campos Opcionais:**
- Data Início
- Data Fim

**Exemplo de Uso:**
```
Cupom: MULTIPETS15
Descrição: 15% de desconto para 2 ou mais pets
Percentual: 15
Mínimo de Pets: 2
Valor da Reserva: R$ 600,00
Quantidade de Pets: 3
Desconto: R$ 90,00
Valor Final: R$ 510,00
```

---

### 3. PercentualPorPetComDiarias (Tipo 3)
**Descrição:** Desconto percentual aplicado quando atinge quantidade mínima de pets E diárias.

**Regras:**
- Requer quantidade mínima de pets E quantidade mínima de diárias
- Ambos os requisitos devem ser atendidos
- Aplica o percentual sobre o valor total quando atinge ambos os mínimos

**Campos Obrigatórios:**
- Código
- Descrição
- Tipo = 3
- Percentual (ex: 20 para 20%)
- Mínimo de Pets (ex: 2)
- Mínimo de Diárias (ex: 5)

**Campos Opcionais:**
- Data Início
- Data Fim

**Exemplo de Uso:**
```
Cupom: LONGSTAY20
Descrição: 20% de desconto para 2+ pets em 5+ diárias
Percentual: 20
Mínimo de Pets: 2
Mínimo de Diárias: 5
Valor da Reserva: R$ 1.000,00
Quantidade de Pets: 2
Quantidade de Diárias: 7
Desconto: R$ 200,00
Valor Final: R$ 800,00
```

---

### 4. ValorFixoComMinimo (Tipo 4)
**Descrição:** Desconto de valor fixo aplicado quando atinge valor mínimo total.

**Regras:**
- Requer valor mínimo total da reserva
- Desconta valor fixo definido
- Se não atingir o valor mínimo, cupom não é aplicado

**Campos Obrigatórios:**
- Código
- Descrição
- Tipo = 4
- Valor Fixo (ex: 50.00)
- Mínimo Valor Total (ex: 300.00)

**Campos Opcionais:**
- Data Início
- Data Fim

**Exemplo de Uso:**
```
Cupom: DESCONTO50
Descrição: R$ 50 de desconto em reservas acima de R$ 300
Valor Fixo: R$ 50,00
Mínimo Valor Total: R$ 300,00
Valor da Reserva: R$ 400,00
Desconto: R$ 50,00
Valor Final: R$ 350,00
```

---

### 5. DescontoPorUltimoPet (Tipo 5)
**Descrição:** Desconto percentual aplicado no valor do último pet.

**Regras:**
- Requer quantidade mínima de pets
- Calcula o valor por pet (valorTotal / quantidadePets)
- Aplica o percentual configurado sobre o valor de 1 pet
- Só aplica se atingir o mínimo de pets

**Campos Obrigatórios:**
- Código
- Descrição
- Tipo = 5
- Percentual (ex: 100 para 100%, 50 para 50%)
- Mínimo de Pets (ex: 2)

**Campos Opcionais:**
- Data Início
- Data Fim

**Exemplo de Uso 1 (100%):**
```
Cupom: ULTIMOPET
Descrição: Último pet grátis para 2 ou mais pets
Percentual: 100
Mínimo de Pets: 2
Valor da Reserva: R$ 600,00
Quantidade de Pets: 3
Valor por Pet: R$ 200,00
Desconto: R$ 200,00 (100% de 1 pet)
Valor Final: R$ 400,00
```

**Exemplo de Uso 2 (50%):**
```
Cupom: MEIOPET
Descrição: 50% de desconto no último pet
Percentual: 50
Mínimo de Pets: 2
Valor da Reserva: R$ 600,00
Quantidade de Pets: 3
Valor por Pet: R$ 200,00
Desconto: R$ 100,00 (50% de 1 pet)
Valor Final: R$ 500,00
```

---

## Validações de Cupom

### Validações Gerais
1. **Cupom Ativo:** Cupom deve estar ativo (Ativo = true)
2. **Data Início:** Se definida, data atual deve ser >= Data Início
3. **Data Fim:** Se definida, data atual deve ser <= Data Fim
4. **Código Único:** Cada cupom deve ter código único

### Validações por Tipo

#### Tipo 1 - PercentualSobreTotal
- Nenhuma validação adicional

#### Tipo 2 - PercentualPorPetComMinimo
- Quantidade de pets >= Mínimo de Pets

#### Tipo 3 - PercentualPorPetComDiarias
- Quantidade de pets >= Mínimo de Pets
- Quantidade de diárias >= Mínimo de Diárias

#### Tipo 4 - ValorFixoComMinimo
- Valor total da reserva >= Mínimo Valor Total

#### Tipo 5 - UltimoPetGratis
- Quantidade de pets >= Mínimo de Pets

---

## Mensagens de Erro

### Cupom Não Encontrado
```
"Cupom não encontrado"
```

### Cupom Inativo
```
"Cupom inativo"
```

### Cupom Ainda Não Válido
```
"Cupom ainda não está válido"
```

### Cupom Expirado
```
"Cupom expirado"
```

### Requisitos Não Atendidos
```
"Cupom não atende aos requisitos mínimos para esta reserva"
```

---

## Informações a Exibir na Tela

### Listagem de Cupons
Para cada cupom, exibir:
- **Código:** BEMVINDO10
- **Descrição:** 10% de desconto em qualquer reserva
- **Status:** Ativo/Inativo
- **Tipo de Desconto:** 
  - Tipo 1: "Percentual sobre total"
  - Tipo 2: "Percentual por pet (mínimo)"
  - Tipo 3: "Percentual por pet e diárias"
  - Tipo 4: "Valor fixo (mínimo)"
  - Tipo 5: "Desconto por último pet"
- **Valor do Desconto:**
  - Tipos 1, 2, 3: "15%"
  - Tipo 4: "R$ 50,00"
  - Tipo 5: "50% no último pet" ou "100% no último pet"
- **Requisitos:**
  - Tipo 1: "Nenhum requisito"
  - Tipo 2: "Mínimo de 2 pets"
  - Tipo 3: "Mínimo de 2 pets e 5 diárias"
  - Tipo 4: "Valor mínimo de R$ 300,00"
  - Tipo 5: "Mínimo de 2 pets"
- **Validade:**
  - Se tem data início e fim: "Válido de 01/01/2024 até 31/12/2024"
  - Se tem apenas data início: "Válido a partir de 01/01/2024"
  - Se tem apenas data fim: "Válido até 31/12/2024"
  - Se não tem datas: "Sem prazo de validade"

### Detalhes do Cupom
Exibir todos os campos acima mais:
- **Data de Criação**
- **Última Alteração**

### Aplicação de Cupom na Reserva
Ao aplicar cupom, exibir:
- **Código do Cupom:** BEMVINDO10
- **Descrição:** 10% de desconto em qualquer reserva
- **Valor Original:** R$ 500,00
- **Desconto Aplicado:** - R$ 50,00
- **Valor Final:** R$ 450,00

---

## Exemplos de Cupons Comuns

### Cupom de Boas-Vindas
```
Código: BEMVINDO10
Tipo: 1 (PercentualSobreTotal)
Percentual: 10
Descrição: 10% de desconto para novos clientes
```

### Cupom Multi-Pets
```
Código: MULTIPETS15
Tipo: 2 (PercentualPorPetComMinimo)
Percentual: 15
Mínimo de Pets: 2
Descrição: 15% de desconto para 2 ou mais pets
```

### Cupom Estadia Longa
```
Código: LONGSTAY20
Tipo: 3 (PercentualPorPetComDiarias)
Percentual: 20
Mínimo de Pets: 2
Mínimo de Diárias: 5
Descrição: 20% de desconto para estadias longas com múltiplos pets
```

### Cupom Valor Fixo
```
Código: DESCONTO50
Tipo: 4 (ValorFixoComMinimo)
Valor Fixo: 50.00
Mínimo Valor Total: 300.00
Descrição: R$ 50 de desconto em reservas acima de R$ 300
```

### Cupom Último Pet Grátis (100%)
```
Código: ULTIMOPET
Tipo: 5 (DescontoPorUltimoPet)
Percentual: 100
Mínimo de Pets: 2
Descrição: Último pet grátis para 2 ou mais pets
```

### Cupom Meio Pet (50%)
```
Código: MEIOPET
Tipo: 5 (DescontoPorUltimoPet)
Percentual: 50
Mínimo de Pets: 2
Descrição: 50% de desconto no último pet
```

### Cupom Promocional Temporário
```
Código: NATAL2024
Tipo: 1 (PercentualSobreTotal)
Percentual: 25
Data Início: 2024-12-01
Data Fim: 2024-12-31
Descrição: 25% de desconto especial de Natal
```
