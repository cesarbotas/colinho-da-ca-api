# Testes Unitários - ColinhoDaCa.Application

Este projeto contém os testes unitários para a camada de aplicação do sistema ColinhoDaCa, organizados por domínio seguindo a mesma estrutura do projeto principal.

## Estrutura dos Testes

```
ColinhoDaCa.TestesUnitarios/
├── Application/                    # Testes da camada de aplicação
│   ├── UseCases/                   # Testes de casos de uso
│   │   ├── LoginServiceTests.cs
│   │   └── CadastrarClienteServiceTests.cs
│   ├── Services/                   # Testes de serviços
│   │   ├── JwtServiceTests.cs
│   │   └── PasswordServiceTests.cs
│   └── Repositories/               # Testes de repositórios (futuro)
└── Domain/                         # Testes de entidades de domínio
    ├── ClienteTests.cs
    ├── CupomDbTests.cs
    ├── ExceptionsTests.cs
    ├── PetTests.cs
    ├── ReservaEntitiesTests.cs
    ├── ReservaTests.cs
    ├── SimpleEntitiesTests.cs
    └── UsuarioTests.cs
```

## Tecnologias Utilizadas

- **xUnit**: Framework de testes
- **Moq**: Biblioteca para criação de mocks
- **FluentAssertions**: Assertions mais legíveis
- **Coverlet**: Cobertura de código

## Executar os Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar com cobertura de código
```bash
dotnet test /p:CollectCoverage=true
```

### Gerar relatório HTML de cobertura
```bash
./run-unit-tests-coverage.sh
```

O relatório HTML será gerado em `coverage/html/index.html`

## Padrões de Teste

### Estrutura de um Teste
Cada teste segue o padrão AAA (Arrange, Act, Assert):

```csharp
[Fact]
public async Task Handle_ValidCommand_ShouldCreateEntity()
{
    // Arrange - Preparar dados e mocks
    var command = new CreateCommand { /* ... */ };
    _repositoryMock.Setup(x => x.Method()).ReturnsAsync(result);

    // Act - Executar o método testado
    await _service.Handle(command);

    // Assert - Verificar o resultado
    _repositoryMock.Verify(x => x.InsertAsync(It.IsAny<Entity>()), Times.Once);
}
```

### Nomenclatura
- **Classe de Teste**: `{ServiceName}Tests`
- **Método de Teste**: `{MethodName}_{Scenario}_{ExpectedBehavior}`

Exemplos:
- `Handle_ValidCommand_ShouldCreatePet`
- `Handle_InvalidId_ShouldThrowValidationException`
- `Handle_ExistingEmail_ShouldThrowValidationException`

## Cobertura de Código

Cobertura atual:
- **Application**: ~12%
- **Domain**: ~72%
- **Total**: ~24%

## Camadas Cobertas

### ✅ Application
- **UseCases**: LoginService, CadastrarClienteService
- **Services**: JwtService, PasswordService
- **Repositories**: (a ser implementado)

### ✅ Domain
- Entidades: Cliente, Pet, Reserva, Cupom, Usuario
- Exceções customizadas
- Regras de negócio

## Próximos Passos

- [ ] Aumentar cobertura para 80%+
- [ ] Adicionar testes para cenários de erro
- [ ] Adicionar testes de integração entre serviços
- [ ] Implementar testes de performance
