-- XX_Tabela{Entidade}.sql
-- Descrição: Criação da tabela {Entidade} com campos padrão

CREATE TABLE IF NOT EXISTS public."{Entidade}" (
    "Id" BIGINT GENERATED ALWAYS AS IDENTITY,
    "Nome" VARCHAR(200) NOT NULL,
    "Descricao" VARCHAR(500),
    "DataInclusao" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DataAlteracao" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_{Entidade}" PRIMARY KEY ("Id")
);

-- Índices
CREATE INDEX IF NOT EXISTS "IX_{Entidade}_Nome"
ON public."{Entidade}" ("Nome");

-- Grants
GRANT SELECT, INSERT, UPDATE, DELETE
ON public."{Entidade}"
TO {projeto}_db_user;

GRANT USAGE, SELECT
ON SEQUENCE public."{Entidade}_Id_seq"
TO {projeto}_db_user;

-- Comentários
COMMENT ON TABLE public."{Entidade}" IS 'Tabela de {entidade}';
COMMENT ON COLUMN public."{Entidade}"."Id" IS 'Identificador único';
COMMENT ON COLUMN public."{Entidade}"."Nome" IS 'Nome da {entidade}';
COMMENT ON COLUMN public."{Entidade}"."DataInclusao" IS 'Data de criação do registro';
COMMENT ON COLUMN public."{Entidade}"."DataAlteracao" IS 'Data da última alteração';
