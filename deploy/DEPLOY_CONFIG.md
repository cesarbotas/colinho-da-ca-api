# Configuração de Deploy - Colinho da Cá API

## Requisitos de Qualidade

### Cobertura de Testes
- **Mínimo exigido**: 20%
- **Validação**: Executada automaticamente no pipeline
- **Falha**: Deploy é bloqueado se cobertura < 20%

### Processo de Deploy
1. Testes unitários executados
2. Cobertura de código calculada
3. Validação do percentual mínimo
4. Build da imagem Docker (se aprovado)
5. Push para DockerHub (branch release)

### Configuração
- Ferramenta: XPlat Code Coverage
- Formato: Cobertura XML
- Projeto: ColinhoDaCa.TestesUnitarios