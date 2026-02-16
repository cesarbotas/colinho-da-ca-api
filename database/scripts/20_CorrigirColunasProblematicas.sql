-- Script 20: Corrigir colunas problemáticas geradas pelo Entity Framework
-- Este script remove colunas auto-geradas pelo EF que estão causando conflitos

-- 1. Verificar e remover colunas problemáticas na tabela ReservaPets
DO $$
BEGIN
    -- Remover coluna ReservaDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'ReservaPets' 
               AND column_name = 'ReservaDbId') THEN
        ALTER TABLE public."ReservaPets" DROP COLUMN "ReservaDbId";
        RAISE NOTICE 'Coluna ReservaDbId removida da tabela ReservaPets';
    END IF;
    
    -- Remover coluna PetDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'ReservaPets' 
               AND column_name = 'PetDbId') THEN
        ALTER TABLE public."ReservaPets" DROP COLUMN "PetDbId";
        RAISE NOTICE 'Coluna PetDbId removida da tabela ReservaPets';
    END IF;
END $$;

-- 2. Verificar e remover colunas problemáticas na tabela UsuarioPerfis
DO $$
BEGIN
    -- Remover coluna UsuarioDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'UsuarioPerfis' 
               AND column_name = 'UsuarioDbId') THEN
        ALTER TABLE public."UsuarioPerfis" DROP COLUMN "UsuarioDbId";
        RAISE NOTICE 'Coluna UsuarioDbId removida da tabela UsuarioPerfis';
    END IF;
    
    -- Remover coluna PerfilDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'UsuarioPerfis' 
               AND column_name = 'PerfilDbId') THEN
        ALTER TABLE public."UsuarioPerfis" DROP COLUMN "PerfilDbId";
        RAISE NOTICE 'Coluna PerfilDbId removida da tabela UsuarioPerfis';
    END IF;
END $$;

-- 3. Verificar e remover colunas problemáticas na tabela ReservaStatusHistorico
DO $$
BEGIN
    -- Remover coluna ReservaDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'ReservaStatusHistorico' 
               AND column_name = 'ReservaDbId') THEN
        ALTER TABLE public."ReservaStatusHistorico" DROP COLUMN "ReservaDbId";
        RAISE NOTICE 'Coluna ReservaDbId removida da tabela ReservaStatusHistorico';
    END IF;
    
    -- Remover coluna UsuarioDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'ReservaStatusHistorico' 
               AND column_name = 'UsuarioDbId') THEN
        ALTER TABLE public."ReservaStatusHistorico" DROP COLUMN "UsuarioDbId";
        RAISE NOTICE 'Coluna UsuarioDbId removida da tabela ReservaStatusHistorico';
    END IF;
END $$;

-- 4. Verificar e remover colunas problemáticas na tabela Pets
DO $$
BEGIN
    -- Remover coluna ClienteDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'Pets' 
               AND column_name = 'ClienteDbId') THEN
        ALTER TABLE public."Pets" DROP COLUMN "ClienteDbId";
        RAISE NOTICE 'Coluna ClienteDbId removida da tabela Pets';
    END IF;
    
    -- Remover coluna RacaDbId se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'Pets' 
               AND column_name = 'RacaDbId') THEN
        ALTER TABLE public."Pets" DROP COLUMN "RacaDbId";
        RAISE NOTICE 'Coluna RacaDbId removida da tabela Pets';
    END IF;
END $$;

-- 5. Verificar e remover outras colunas problemáticas que possam existir
DO $$
BEGIN
    -- Remover coluna ClienteDbId da tabela Usuarios se existir
    IF EXISTS (SELECT 1 FROM information_schema.columns 
               WHERE table_schema = 'public' 
               AND table_name = 'Usuarios' 
               AND column_name = 'ClienteDbId') THEN
        ALTER TABLE public."Usuarios" DROP COLUMN "ClienteDbId";
        RAISE NOTICE 'Coluna ClienteDbId removida da tabela Usuarios';
    END IF;
END $$;

-- 6. Verificar estrutura final das tabelas
SELECT 'Estrutura final das tabelas após limpeza:' as status;

-- Verificar ReservaPets
SELECT 'ReservaPets' as tabela, column_name, data_type, is_nullable
FROM information_schema.columns 
WHERE table_schema = 'public' AND table_name = 'ReservaPets' 
ORDER BY ordinal_position;

-- Verificar UsuarioPerfis
SELECT 'UsuarioPerfis' as tabela, column_name, data_type, is_nullable
FROM information_schema.columns 
WHERE table_schema = 'public' AND table_name = 'UsuarioPerfis' 
ORDER BY ordinal_position;

-- Verificar ReservaStatusHistorico
SELECT 'ReservaStatusHistorico' as tabela, column_name, data_type, is_nullable
FROM information_schema.columns 
WHERE table_schema = 'public' AND table_name = 'ReservaStatusHistorico' 
ORDER BY ordinal_position;

-- Verificar Pets
SELECT 'Pets' as tabela, column_name, data_type, is_nullable
FROM information_schema.columns 
WHERE table_schema = 'public' AND table_name = 'Pets' 
ORDER BY ordinal_position;