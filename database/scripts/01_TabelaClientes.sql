CREATE TABLE IF NOT EXISTS public."Clientes" (
    "Id" BIGINT GENERATED ALWAYS AS IDENTITY,
    "Nome" VARCHAR(200) NOT NULL,
    "Email" VARCHAR(100) NOT NULL,
    "Celular" VARCHAR(20) NOT NULL,
    "Cpf" VARCHAR(11) NOT NULL,
    "Endereco" VARCHAR(500),
    "Observacoes" VARCHAR(1000),
    CONSTRAINT "PK_Clientes" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Clientes_Cpf"
ON public."Clientes" ("Cpf");

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Clientes_Email"
ON public."Clientes" ("Email");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Clientes"
TO colinho_da_ca_db_user;