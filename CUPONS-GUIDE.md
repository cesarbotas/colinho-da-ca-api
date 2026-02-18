# Sistema de Cupons de Desconto

## 📋 Estrutura

### Tabela Cupons
- **Id**: Identificador único
- **Codigo**: Código do cupom (único, ex: "DESC5", "3PETS50")
- **Descricao**: Descrição do cupom
- **Tipo**: Tipo de desconto (enum)
- **Percentual**: Percentual de desconto
- **ValorFixo**: Valor fixo em reais (nullable)
- **MinimoValorTotal**: Valor mínimo total para aplicar (nullable)
- **MinimoPets**: Mínimo de pets (nullable)
- **MinimoDiarias**: Mínimo de diárias (nullable)
- **DataInicio**: Data início validade (nullable)
- **DataFim**: Data fim validade (nullable)
- **Ativo**: Se o cupom está ativo
- **DataInclusao/DataAlteracao**: Auditoria

### Relacionamento
- Reserva N:1 Cupom (CupomId nullable)

## 🎯 Tipos de Cupom

### 1. PercentualSobreTotal (Tipo 1)
**Exemplo**: 5% sobre o valor total

**Configuração:**
```json
{
  "codigo": "DESC5",
  "tipo": 1,
  "percentual": 5.00,
  "minimoPets": null,
  "minimoDiarias": null
}
```

**Cálculo:**
```
ValorDesconto = ValorTotal * (5 / 100)
```

### 2. PercentualPorPetComMinimo (Tipo 2)
**Exemplo**: 50% de desconto se tiver 3 ou mais pets

**Configuração:**
```json
{
  "codigo": "3PETS50",
  "tipo": 2,
  "percentual": 50.00,
  "minimoPets": 3,
  "minimoDiarias": null
}
```

**Cálculo:**
```
SE quantidadePets >= 3:
  ValorDesconto = (ValorTotal / quantidadePets) * (50 / 100) * quantidadePets
SENÃO:
  ValorDesconto = 0
```

### 3. PercentualPorPetComDiarias (Tipo 3)
**Exemplo**: 10% por pet se 2+ pets e 5+ diárias

**Configuração:**
```json
{
  "codigo": "2PETS5DIAS",
  "tipo": 3,
  "percentual": 10.00,
  "minimoPets": 2,
  "minimoDiarias": 5
}
```

**Cálculo:**
```
SE quantidadePets >= 2 E quantidadeDiarias >= 5:
  ValorDesconto = (ValorTotal / quantidadePets) * (10 / 100) * quantidadePets
SENÃO:
  ValorDesconto = 0
```

### 4. ValorFixoComMinimo (Tipo 4)
**Exemplo**: R$ 50 de desconto em diárias acima de R$ 300

**Configuração:**
```json
{
  "codigo": "50REAIS300",
  "tipo": 4,
  "percentual": 0.00,
  "valorFixo": 50.00,
  "minimoValorTotal": 300.00
}
```

**Cálculo:**
```
SE ValorTotal >= 300:
  ValorDesconto = 50.00
SENÃO:
  ValorDesconto = 0
```

## 🔄 Fluxo de Aplicação

1. Cliente informa código do cupom ao criar reserva
2. Sistema busca cupom por código
3. Valida se cupom está ativo
4. Valida se está dentro do período de validade
5. Calcula desconto baseado no tipo
6. Aplica desconto: ValorFinal = ValorTotal - ValorDesconto
7. Vincula CupomId à reserva

## 📊 Exemplos Práticos

### Exemplo 1: DESC5
- ValorTotal: R$ 400,00
- Cupom: 5% sobre total
- **ValorDesconto: R$ 20,00**
- **ValorFinal: R$ 380,00**

### Exemplo 2: 3PETS50
- ValorTotal: R$ 600,00 (3 pets)
- Cupom: 50% se >= 3 pets
- **ValorDesconto: R$ 300,00**
- **ValorFinal: R$ 300,00**

### Exemplo 3: 2PETS5DIAS
- ValorTotal: R$ 500,00 (2 pets, 5 diárias)
- Cupom: 10% por pet se >= 2 pets e >= 5 diárias
- **ValorDesconto: R$ 50,00**
- **ValorFinal: R$ 450,00**

### Exemplo 4: 50REAIS300
- ValorTotal: R$ 350,00
- Cupom: R$ 50 se >= R$ 300
- **ValorDesconto: R$ 50,00**
- **ValorFinal: R$ 300,00**

## 🛠️ Próximos Passos

1. Criar ICupomRepository e CupomRepository
2. Criar endpoint POST /api/v1/reservas/{id}/aplicar-cupom
3. Criar AplicarCupomService com validações
4. Adicionar CupomId no ReservasDto
5. Criar CRUD de Cupons (admin)
