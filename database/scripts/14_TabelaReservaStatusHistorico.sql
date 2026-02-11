-- Criar tabela ReservaStatusHistorico
CREATE TABLE IF NOT EXISTS "ReservaStatusHistorico" (
    "Id" BIGSERIAL PRIMARY KEY,
    "ReservaId" BIGINT NOT NULL,
    "Status" INT NOT NULL,
    "UsuarioId" BIGINT NOT NULL,
    "DataAlteracao" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT "FK_ReservaStatusHistorico_Reservas" FOREIGN KEY ("ReservaId") REFERENCES "Reservas"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ReservaStatusHistorico_Usuarios" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios"("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_ReservaStatusHistorico_ReservaId" ON "ReservaStatusHistorico"("ReservaId");
