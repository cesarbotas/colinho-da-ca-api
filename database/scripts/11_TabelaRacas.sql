-- Criar tabela Racas
CREATE TABLE IF NOT EXISTS "Racas" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Nome" VARCHAR(100) NOT NULL,
    "Porte" VARCHAR(1) NULL
);

-- Inserir raças pequenas
INSERT INTO "Racas" ("Nome", "Porte") VALUES
('Shih-tzu', 'P'),
('Lhasa Apso', 'P'),
('Yorkshire Terrier', 'P'),
('Maltês', 'P'),
('Poodle (Toy / Anão)', 'P'),
('Spitz Alemão (Lulu da Pomerânia)', 'P'),
('Chihuahua', 'P'),
('Pinscher', 'P'),
('Dachshund (Salsicha)', 'P'),
('Pug', 'P'),
('Buldogue Francês', 'P'),
('Boston Terrier', 'P');

-- Inserir raças médias
INSERT INTO "Racas" ("Nome", "Porte") VALUES
('Beagle', 'M'),
('Cocker Spaniel Inglês', 'M'),
('Schnauzer (Médio)', 'M'),
('Border Collie', 'M'),
('Bulldog Inglês', 'M'),
('Basset Hound', 'M'),
('Shar Pei', 'M'),
('Chow Chow', 'M'),
('Pastor Australiano', 'M'),
('Whippet', 'M');

-- Inserir raças grandes
INSERT INTO "Racas" ("Nome", "Porte") VALUES
('Labrador Retriever', 'G'),
('Golden Retriever', 'G'),
('Pastor Alemão', 'G'),
('Rottweiler', 'G'),
('Doberman', 'G'),
('Husky Siberiano', 'G'),
('Akita', 'G'),
('Dálmata', 'G'),
('Boxer', 'G'),
('Weimaraner', 'G'),
('São Bernardo', 'G'),
('Dogue Alemão', 'G');

-- Inserir raças sem definição
INSERT INTO "Racas" ("Nome", "Porte") VALUES
('SRD (Sem Raça Definida)', NULL),
('Vira-lata Caramelo', NULL);
