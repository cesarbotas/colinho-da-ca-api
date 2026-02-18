-- Criar tabela Cupons
CREATE TABLE IF NOT EXISTS "Cupons" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Codigo" VARCHAR(50) NOT NULL UNIQUE,
    "Descricao" VARCHAR(200) NOT NULL,
    "Tipo" INT NOT NULL,
    "Percentual" DECIMAL(5,2) NOT NULL,
    "ValorFixo" DECIMAL(10,2) NULL,
    "MinimoValorTotal" DECIMAL(10,2) NULL,
    "MinimoPets" INT NULL,
    "MinimoDiarias" INT NULL,
    "DataInicio" TIMESTAMP NULL,
    "DataFim" TIMESTAMP NULL,
    "Ativo" BOOLEAN NOT NULL DEFAULT true,
    "DataInclusao" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "DataAlteracao" TIMESTAMP WITHOUT TIME ZONE NOT NULL
);

-- Adicionar campo CupomId em Reservas
ALTER TABLE "Reservas" ADD COLUMN "CupomId" BIGINT NULL;
ALTER TABLE "Reservas" ADD CONSTRAINT "FK_Reservas_Cupons" 
    FOREIGN KEY ("CupomId") REFERENCES "Cupons"("Id");

-- Inserir cupons de exemplo
INSERT INTO "Cupons" ("Codigo", "Descricao", "Tipo", "Percentual", "ValorFixo", "MinimoValorTotal", "MinimoPets", "MinimoDiarias", "Ativo", "DataInclusao", "DataAlteracao") VALUES
('DESC5', '5% de desconto sobre o valor total', 1, 5.00, NULL, NULL, NULL, NULL, true, NOW(), NOW()),
('3PETS50', '50% de desconto se tiver 3 ou mais pets', 2, 50.00, NULL, NULL, 3, NULL, true, NOW(), NOW()),
('2PETS5DIAS', '10% por pet se 2+ pets e 5+ diárias', 3, 10.00, NULL, NULL, 2, 5, true, NOW(), NOW()),
('50REAIS300', 'R$ 50 de desconto em diárias acima de R$ 300', 4, 0.00, 50.00, 300.00, NULL, NULL, true, NOW(), NOW());
