-- Script de dados de teste para testes integrados
-- Este script insere dados básicos que podem ser usados pelos testes

-- 1. Inserir cliente de teste
INSERT INTO public."Clientes" ("Nome", "Email", "Celular", "Cpf", "Observacoes", "DataInclusao", "DataAlteracao")
VALUES 
('Cliente Teste', 'cliente.teste@email.com', '11999999999', '12345678901', 'Cliente para testes integrados', NOW(), NOW())
ON CONFLICT DO NOTHING;

-- 2. Inserir usuário de teste para o cliente
INSERT INTO public."Usuarios" ("ClienteId", "SenhaHash", "Ativo", "DataInclusao", "DataAlteracao")
SELECT c."Id", '$2a$11$example.hash.for.testing.purposes.only', true, NOW(), NOW()
FROM public."Clientes" c 
WHERE c."Email" = 'cliente.teste@email.com'
ON CONFLICT DO NOTHING;

-- 3. Associar perfil Cliente ao usuário de teste
INSERT INTO public."UsuarioPerfis" ("UsuarioId", "PerfilId")
SELECT u."Id", 2
FROM public."Usuarios" u
INNER JOIN public."Clientes" c ON c."Id" = u."ClienteId"
WHERE c."Email" = 'cliente.teste@email.com'
ON CONFLICT DO NOTHING;

-- 4. Inserir pet de teste
INSERT INTO public."Pets" ("Nome", "RacaId", "Idade", "Peso", "Porte", "Observacoes", "ClienteId", "Ativo", "DataInclusao", "DataAlteracao")
SELECT 'Pet Teste', 1, 3, 15.5, 'M', 'Pet para testes integrados', c."Id", true, NOW(), NOW()
FROM public."Clientes" c 
WHERE c."Email" = 'cliente.teste@email.com'
ON CONFLICT DO NOTHING;

-- 5. Inserir reserva de teste
INSERT INTO public."Reservas" ("ClienteId", "DataInicial", "DataFinal", "Status", "Observacoes", "DataInclusao", "DataAlteracao")
SELECT c."Id", CURRENT_DATE + INTERVAL '1 day', CURRENT_DATE + INTERVAL '3 days', 'Pendente', 'Reserva para testes integrados', NOW(), NOW()
FROM public."Clientes" c 
WHERE c."Email" = 'cliente.teste@email.com'
ON CONFLICT DO NOTHING;

-- 6. Associar pet à reserva
INSERT INTO public."ReservaPets" ("ReservaId", "PetId")
SELECT r."Id", p."Id"
FROM public."Reservas" r
INNER JOIN public."Clientes" c ON c."Id" = r."ClienteId"
INNER JOIN public."Pets" p ON p."ClienteId" = c."Id"
WHERE c."Email" = 'cliente.teste@email.com'
ON CONFLICT DO NOTHING;