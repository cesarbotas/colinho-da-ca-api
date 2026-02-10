ALTER TABLE public."Usuarios" ADD COLUMN "Ativo" BOOLEAN NOT NULL DEFAULT true;

CREATE TABLE public."Perfis" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
    "Descricao" VARCHAR(500)
);

CREATE TABLE public."UsuarioPerfis" (
    "UsuarioId" BIGINT NOT NULL,
    "PerfilId" BIGINT NOT NULL,
    CONSTRAINT "PK_UsuarioPerfis" PRIMARY KEY ("UsuarioId", "PerfilId"),
    CONSTRAINT "FK_UsuarioPerfis_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") 
        REFERENCES public."Usuarios"("Id") 
        ON DELETE CASCADE,
    CONSTRAINT "FK_UsuarioPerfis_Perfis_PerfilId" FOREIGN KEY ("PerfilId") 
        REFERENCES public."Perfis"("Id") 
        ON DELETE RESTRICT
);

CREATE INDEX "IX_UsuarioPerfis_PerfilId" ON public."UsuarioPerfis" ("PerfilId");

INSERT INTO public."Perfis" ("Nome", "Descricao") VALUES ('Administrador', 'Acesso total ao sistema');
INSERT INTO public."Perfis" ("Nome", "Descricao") VALUES ('Cliente', 'Acesso de cliente padrão');

GRANT SELECT, INSERT, UPDATE, DELETE ON public."Perfis" TO colinho_da_ca_db_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON public."UsuarioPerfis" TO colinho_da_ca_db_user;
