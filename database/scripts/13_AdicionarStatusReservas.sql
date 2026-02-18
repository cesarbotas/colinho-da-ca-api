-- Adicionar campos de status e pagamento na tabela Reservas
ALTER TABLE "Reservas" ADD COLUMN "Status" INT NOT NULL DEFAULT 1;
ALTER TABLE "Reservas" ADD COLUMN "ComprovantePagamento" TEXT NULL;
ALTER TABLE "Reservas" ADD COLUMN "DataPagamento" TIMESTAMP NULL;
ALTER TABLE "Reservas" ADD COLUMN "ObservacoesPagamento" TEXT NULL;
