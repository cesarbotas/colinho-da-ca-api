-- Adicionar campos de desconto e valor final
ALTER TABLE "Reservas" ADD COLUMN "ValorDesconto" DECIMAL(18,2) NOT NULL DEFAULT 0;
ALTER TABLE "Reservas" ADD COLUMN "ValorFinal" DECIMAL(18,2) NOT NULL DEFAULT 0;

-- Atualizar ValorFinal com ValorTotal para registros existentes
UPDATE "Reservas" SET "ValorFinal" = "ValorTotal" WHERE "ValorFinal" = 0;
