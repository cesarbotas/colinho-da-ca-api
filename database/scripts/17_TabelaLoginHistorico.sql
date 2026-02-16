CREATE TABLE IF NOT EXISTS public."LoginHistorico" (
    "Id" BIGSERIAL PRIMARY KEY,
    "UsuarioId" BIGINT NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "UserAgent" TEXT,
    "Platform" VARCHAR(100),
    "Language" VARCHAR(10),
    "ScreenResolution" VARCHAR(20),
    "Timezone" VARCHAR(50),
    "ClientIP" VARCHAR(45),
    "DataLogin" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT "FK_LoginHistorico_Usuario" FOREIGN KEY ("UsuarioId") REFERENCES public."Usuarios"("Id")
);

CREATE INDEX "IX_LoginHistorico_UsuarioId" ON public."LoginHistorico"("UsuarioId");
CREATE INDEX "IX_LoginHistorico_DataLogin" ON public."LoginHistorico"("DataLogin");
CREATE INDEX "IX_LoginHistorico_Email" ON public."LoginHistorico"("Email");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."LoginHistorico"
TO colinho_da_ca_db_user;