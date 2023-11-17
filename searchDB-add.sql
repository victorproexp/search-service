BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "document" (
	"id"	INTEGER,
	"url"	TEXT,
	"idxTime"	TEXT,
	"creationTime"	TEXT,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "word" (
	"id"	INTEGER,
	"name"	VARCHAR(50),
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "Occ" (
	"wordId"	INTEGER,
	"docId"	INTEGER,
	FOREIGN KEY("docId") REFERENCES "document"("id"),
	FOREIGN KEY("wordId") REFERENCES "word"("id")
);
INSERT INTO "document" VALUES (1,'additional mail document','10/5/2023 8:27:09PM','11/1/2023 11:07:08PM');
INSERT INTO "word" VALUES (1,'Let');
INSERT INTO "word" VALUES (2,'let');
INSERT INTO "Occ" VALUES (1,1);
INSERT INTO "Occ" VALUES (2,1);

CREATE INDEX IF NOT EXISTS "word_index" ON "Occ" (
	"wordId"
);
COMMIT;
