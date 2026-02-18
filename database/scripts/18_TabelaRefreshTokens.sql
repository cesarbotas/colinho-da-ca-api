CREATE TABLE IF NOT EXISTS public."RefreshTokens" (
    "Id" BIGSERIAL PRIMARY KEY,
    "UsuarioId" BIGINT NOT NULL,
    "Token" VARCHAR(500) NOT NULL,
    "ExpiresAt" TIMESTAMP NOT NULL,
    "IsRevoked" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "RevokedAt" TIMESTAMP NULL,
    
    CONSTRAINT "FK_RefreshTokens_Usuario" FOREIGN KEY ("UsuarioId") REFERENCES public."Usuarios"("Id")
);

CREATE INDEX "IX_RefreshTokens_UsuarioId" ON public."RefreshTokens"("UsuarioId");
CREATE INDEX "IX_RefreshTokens_Token" ON public."RefreshTokens"("Token");
CREATE INDEX "IX_RefreshTokens_ExpiresAt" ON public."RefreshTokens"("ExpiresAt");

GRANT SELECT, INSERT, UPDATE, DELETE
ON public."RefreshTokens"
TO colinho_da_ca_db_user;