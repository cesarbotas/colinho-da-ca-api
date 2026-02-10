CREATE TABLE public."Usuarios" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Nome" VARCHAR(200) NOT NULL,
    "Email" VARCHAR(100) NOT NULL,
    "SenhaHash" VARCHAR(500) NOT NULL,
    "DataInclusao" TIMESTAMP NOT NULL,
    "DataAlteracao" TIMESTAMP NOT NULL,
    CONSTRAINT "UQ_Usuarios_Email" UNIQUE ("Email")
);

CREATE INDEX "IX_Usuarios_Email" ON public."Usuarios" ("Email");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Usuarios"
TO colinho_da_ca_db_user;
