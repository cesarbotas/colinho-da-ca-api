CREATE TABLE public."Reservas" (
    "Id" BIGSERIAL PRIMARY KEY,
    "ClienteId" BIGINT NOT NULL,
    "DataInicial" TIMESTAMP NOT NULL,
    "DataFinal" TIMESTAMP NOT NULL,
    "Observacoes" VARCHAR(1000),
    "DataInclusao" TIMESTAMP NOT NULL,
    "DataAlteracao" TIMESTAMP NOT NULL,
    CONSTRAINT "FK_Reservas_Clientes_ClienteId" FOREIGN KEY ("ClienteId") 
        REFERENCES public."Clientes"("Id") 
        ON DELETE RESTRICT
);

CREATE INDEX "IX_Reservas_ClienteId" ON public."Reservas" ("ClienteId");

CREATE TABLE public."ReservaPets" (
    "ReservaId" BIGINT NOT NULL,
    "PetId" BIGINT NOT NULL,
    CONSTRAINT "PK_ReservaPets" PRIMARY KEY ("ReservaId", "PetId"),
    CONSTRAINT "FK_ReservaPets_Reservas_ReservaId" FOREIGN KEY ("ReservaId") 
        REFERENCES public."Reservas"("Id") 
        ON DELETE CASCADE,
    CONSTRAINT "FK_ReservaPets_Pets_PetId" FOREIGN KEY ("PetId") 
        REFERENCES public."Pets"("Id") 
        ON DELETE RESTRICT
);

CREATE INDEX "IX_ReservaPets_PetId" ON public."ReservaPets" ("PetId");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Reservas"
TO colinho_da_ca_db_user;

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."ReservaPets"
TO colinho_da_ca_db_user;
