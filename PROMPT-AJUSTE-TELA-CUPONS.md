# Prompt para Ajuste da Tela de Cupons

## Contexto
A tela de cupons no aplicativo está mostrando sempre a mesma informação do tipo de cupom, sem diferenciar os 4 tipos existentes e seus requisitos específicos.

## Objetivo
Ajustar a tela de listagem e detalhes de cupons para exibir informações específicas de cada tipo de cupom, conforme documentado em CUPONS-REGRAS.md.

## Requisitos

### 1. Tela de Listagem de Cupons

#### Card de Cupom deve exibir:
```
┌─────────────────────────────────────────┐
│ 🎟️ BEMVINDO10              [Ativo/Inativo]│
│                                         │
│ 10% de desconto em qualquer reserva     │
│                                         │
│ 💰 Desconto: 15%                        │
│ 📋 Tipo: Percentual sobre total         │
│ ✅ Requisitos: Nenhum requisito         │
│ 📅 Válido até 31/12/2024                │
└─────────────────────────────────────────┘
```

#### Lógica de Exibição por Tipo:

**Tipo 1 - PercentualSobreTotal:**
```
💰 Desconto: {Percentual}%
📋 Tipo: Percentual sobre total
✅ Requisitos: Nenhum requisito
```

**Tipo 2 - PercentualPorPetComMinimo:**
```
💰 Desconto: {Percentual}%
📋 Tipo: Percentual por pet
✅ Requisitos: Mínimo de {MinimoPets} pets
```

**Tipo 3 - PercentualPorPetComDiarias:**
```
💰 Desconto: {Percentual}%
📋 Tipo: Percentual por pet e diárias
✅ Requisitos: Mínimo de {MinimoPets} pets e {MinimoDiarias} diárias
```

**Tipo 4 - ValorFixoComMinimo:**
```
💰 Desconto: R$ {ValorFixo}
📋 Tipo: Valor fixo
✅ Requisitos: Valor mínimo de R$ {MinimoValorTotal}
```

**Tipo 5 - DescontoPorUltimoPet:**
```
💰 Desconto: {Percentual}% no último pet
📋 Tipo: Desconto por último pet
✅ Requisitos: Mínimo de {MinimoPets} pets
```

#### Lógica de Validade:
```typescript
function formatarValidade(dataInicio?: Date, dataFim?: Date): string {
  if (dataInicio && dataFim) {
    return `Válido de ${formatDate(dataInicio)} até ${formatDate(dataFim)}`;
  }
  if (dataInicio) {
    return `Válido a partir de ${formatDate(dataInicio)}`;
  }
  if (dataFim) {
    return `Válido até ${formatDate(dataFim)}`;
  }
  return 'Sem prazo de validade';
}
```

### 2. Tela de Detalhes do Cupom

Exibir todos os campos da listagem mais:
```
┌─────────────────────────────────────────┐
│ Detalhes do Cupom                       │
├─────────────────────────────────────────┤
│ Código: BEMVINDO10                      │
│ Status: Ativo                           │
│                                         │
│ Descrição:                              │
│ 10% de desconto em qualquer reserva     │
│                                         │
│ Tipo de Desconto:                       │
│ Percentual sobre total                  │
│                                         │
│ Valor do Desconto:                      │
│ 15%                                     │
│                                         │
│ Requisitos:                             │
│ Nenhum requisito                        │
│                                         │
│ Validade:                               │
│ Válido até 31/12/2024                   │
│                                         │
│ Criado em: 01/01/2024                   │
│ Atualizado em: 15/01/2024               │
└─────────────────────────────────────────┘
```

### 3. Aplicação de Cupom na Reserva

Ao aplicar cupom, exibir preview:
```
┌─────────────────────────────────────────┐
│ Cupom Aplicado                          │
├─────────────────────────────────────────┤
│ 🎟️ BEMVINDO10                           │
│ 10% de desconto em qualquer reserva     │
│                                         │
│ Valor Original:      R$ 500,00          │
│ Desconto:          - R$  50,00          │
│ ─────────────────────────────────       │
│ Valor Final:         R$ 450,00          │
└─────────────────────────────────────────┘
```

### 4. Validação de Cupom

Ao tentar aplicar cupom inválido, exibir mensagem específica:

```typescript
function validarCupom(cupom, reserva) {
  if (!cupom.ativo) {
    return { valido: false, mensagem: "Cupom inativo" };
  }
  
  if (cupom.dataInicio && new Date() < cupom.dataInicio) {
    return { valido: false, mensagem: "Cupom ainda não está válido" };
  }
  
  if (cupom.dataFim && new Date() > cupom.dataFim) {
    return { valido: false, mensagem: "Cupom expirado" };
  }
  
  // Validações por tipo
  switch (cupom.tipo) {
    case 2: // PercentualPorPetComMinimo
      if (reserva.quantidadePets < cupom.minimoPets) {
        return { 
          valido: false, 
          mensagem: `Este cupom requer no mínimo ${cupom.minimoPets} pets` 
        };
      }
      break;
      
    case 3: // PercentualPorPetComDiarias
      if (reserva.quantidadePets < cupom.minimoPets) {
        return { 
          valido: false, 
          mensagem: `Este cupom requer no mínimo ${cupom.minimoPets} pets` 
        };
      }
      if (reserva.quantidadeDiarias < cupom.minimoDiarias) {
        return { 
          valido: false, 
          mensagem: `Este cupom requer no mínimo ${cupom.minimoDiarias} diárias` 
        };
      }
      break;
      
    case 4: // ValorFixoComMinimo
      if (reserva.valorTotal < cupom.minimoValorTotal) {
        return { 
          valido: false, 
          mensagem: `Este cupom requer valor mínimo de R$ ${cupom.minimoValorTotal.toFixed(2)}` 
        };
      }
      break;
      
    case 5: // UltimoPetGratis
      if (reserva.quantidadePets < cupom.minimoPets) {
        return { 
          valido: false, 
          mensagem: `Este cupom requer no mínimo ${cupom.minimoPets} pets` 
        };
      }
      break;
  }
  
  return { valido: true };
}
```

## Estrutura de Dados

### Interface Cupom
```typescript
interface Cupom {
  id: number;
  codigo: string;
  descricao: string;
  tipo: 1 | 2 | 3 | 4 | 5;
  percentual: number;
  valorFixo?: number;
  minimoValorTotal?: number;
  minimoPets?: number;
  minimoDiarias?: number;
  dataInicio?: Date;
  dataFim?: Date;
  ativo: boolean;
  dataInclusao: Date;
  dataAlteracao: Date;
}
```

### Função Helper para Tipo
```typescript
function getTipoDescricao(tipo: number): string {
  const tipos = {
    1: 'Percentual sobre total',
    2: 'Percentual por pet',
    3: 'Percentual por pet e diárias',
    4: 'Valor fixo',
    5: 'Desconto por último pet'
  };
  return tipos[tipo] || 'Desconhecido';
}
```

### Função Helper para Desconto
```typescript
function getDescontoDescricao(cupom: Cupom): string {
  if (cupom.tipo === 4) {
    return `R$ ${cupom.valorFixo?.toFixed(2)}`;
  }
  if (cupom.tipo === 5) {
    return `${cupom.percentual}% no último pet`;
  }
  return `${cupom.percentual}%`;
}
```

### Função Helper para Requisitos
```typescript
function getRequisitosDescricao(cupom: Cupom): string {
  switch (cupom.tipo) {
    case 1:
      return 'Nenhum requisito';
    case 2:
      return `Mínimo de ${cupom.minimoPets} pets`;
    case 3:
      return `Mínimo de ${cupom.minimoPets} pets e ${cupom.minimoDiarias} diárias`;
    case 4:
      return `Valor mínimo de R$ ${cupom.minimoValorTotal?.toFixed(2)}`;
    case 5:
      return `Mínimo de ${cupom.minimoPets} pets`;
    default:
      return 'Requisitos não definidos';
  }
}
```

## Exemplo de Implementação React/React Native

```typescript
import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface CupomCardProps {
  cupom: Cupom;
}

const CupomCard: React.FC<CupomCardProps> = ({ cupom }) => {
  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <Text style={styles.codigo}>🎟️ {cupom.codigo}</Text>
        <Text style={cupom.ativo ? styles.ativo : styles.inativo}>
          {cupom.ativo ? 'Ativo' : 'Inativo'}
        </Text>
      </View>
      
      <Text style={styles.descricao}>{cupom.descricao}</Text>
      
      <View style={styles.info}>
        <Text style={styles.label}>💰 Desconto:</Text>
        <Text style={styles.value}>{getDescontoDescricao(cupom)}</Text>
      </View>
      
      <View style={styles.info}>
        <Text style={styles.label}>📋 Tipo:</Text>
        <Text style={styles.value}>{getTipoDescricao(cupom.tipo)}</Text>
      </View>
      
      <View style={styles.info}>
        <Text style={styles.label}>✅ Requisitos:</Text>
        <Text style={styles.value}>{getRequisitosDescricao(cupom)}</Text>
      </View>
      
      <View style={styles.info}>
        <Text style={styles.label}>📅 Validade:</Text>
        <Text style={styles.value}>
          {formatarValidade(cupom.dataInicio, cupom.dataFim)}
        </Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 16,
    marginBottom: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  codigo: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  ativo: {
    color: '#4CAF50',
    fontWeight: 'bold',
  },
  inativo: {
    color: '#F44336',
    fontWeight: 'bold',
  },
  descricao: {
    fontSize: 14,
    color: '#666',
    marginBottom: 12,
  },
  info: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 4,
  },
  label: {
    fontSize: 14,
    color: '#666',
  },
  value: {
    fontSize: 14,
    color: '#333',
    fontWeight: '500',
  },
});

export default CupomCard;
```

## Checklist de Implementação

- [ ] Criar funções helper (getTipoDescricao, getDescontoDescricao, getRequisitosDescricao, formatarValidade)
- [ ] Atualizar componente de card de cupom na listagem
- [ ] Atualizar tela de detalhes do cupom
- [ ] Implementar validação de cupom com mensagens específicas
- [ ] Atualizar preview de aplicação de cupom na reserva
- [ ] Testar todos os 4 tipos de cupons
- [ ] Testar validações de data (início, fim, expirado)
- [ ] Testar validações de requisitos (pets, diárias, valor mínimo)
- [ ] Adicionar feedback visual para cupons inativos
- [ ] Adicionar feedback visual para cupons expirados
