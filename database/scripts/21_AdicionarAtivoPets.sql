-- Script: Add Ativo column to Pets table
-- Date: 2024-02-15

ALTER TABLE "Pets" ADD COLUMN "Ativo" BOOLEAN NOT NULL DEFAULT true;

-- Update existing pets to be active
UPDATE "Pets" SET "Ativo" = true;