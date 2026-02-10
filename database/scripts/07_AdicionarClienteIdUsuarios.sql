ALTER TABLE public."Usuarios" ADD COLUMN "ClienteId" BIGINT;

ALTER TABLE public."Usuarios" 
ADD CONSTRAINT "FK_Usuarios_Clientes_ClienteId" 
FOREIGN KEY ("ClienteId") 
REFERENCES public."Clientes"("Id") 
ON DELETE RESTRICT;

CREATE INDEX "IX_Usuarios_ClienteId" ON public."Usuarios" ("ClienteId");
