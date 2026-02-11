-- Adicionar coluna RacaId na tabela Pets
ALTER TABLE "Pets" ADD COLUMN "RacaId" BIGINT NULL;

-- Adicionar FK para Racas
ALTER TABLE "Pets" ADD CONSTRAINT "FK_Pets_Racas" 
    FOREIGN KEY ("RacaId") REFERENCES "Racas"("Id");

-- Remover coluna Raca (string) antiga
ALTER TABLE "Pets" DROP COLUMN "Raca";
