ALTER TABLE public."Usuarios" DROP COLUMN "Nome";
ALTER TABLE public."Usuarios" DROP COLUMN "Email";

ALTER TABLE public."Usuarios" ALTER COLUMN "ClienteId" SET NOT NULL;

CREATE UNIQUE INDEX "IX_Usuarios_ClienteId_Unique" ON public."Usuarios" ("ClienteId");
