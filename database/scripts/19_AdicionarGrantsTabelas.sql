-- Adicionar GRANT permissions para tabelas que não possuem

-- Tabela Racas (script 11)
GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Racas"
TO colinho_da_ca_db_user;

-- Tabela ReservaStatusHistorico (script 14)
GRANT SELECT, INSERT, UPDATE, DELETE
ON public."ReservaStatusHistorico"
TO colinho_da_ca_db_user;

-- Tabela Cupons (script 16)
GRANT SELECT, INSERT, UPDATE, DELETE
ON public."Cupons"
TO colinho_da_ca_db_user;