CREATE TABLE public."Pets" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Nome" VARCHAR(200) NOT NULL,
    "Raca" VARCHAR(100),
    "Idade" INTEGER NOT NULL,
    "Peso" DOUBLE PRECISION NOT NULL,
    "Observacoes" VARCHAR(1000),
    "ClienteId" BIGINT NOT NULL,
    CONSTRAINT "FK_Pets_Clientes_ClienteId" FOREIGN KEY ("ClienteId") 
        REFERENCES public."Clientes"("Id") 
        ON DELETE RESTRICT
);

CREATE INDEX "IX_Pets_ClienteId" ON public."Pets" ("ClienteId");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Pets"
TO colinho_da_ca_db_user;
