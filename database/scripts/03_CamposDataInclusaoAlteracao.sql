alter table public."Clientes" add column "DataInclusao" TIMESTAMP null;
alter table public."Clientes" add column "DataAlteracao" TIMESTAMP null;

alter table public."Pets" add column "Porte" VARCHAR(1) null;

alter table public."Pets" add column "DataInclusao" TIMESTAMP null;
alter table public."Pets" add column "DataAlteracao" TIMESTAMP null;
