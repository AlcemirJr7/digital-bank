namespace Tarifa.Infrastructure.Database;

public static class DbScripts
{
    public const string CreateTables = @"
        CREATE TABLE IF NOT EXISTS tarifa (
	        idtarifa TEXT(37) PRIMARY KEY, -- identificacao unica da tarifa
	        idcontacorrente TEXT(37) NOT NULL, -- identificacao unica da conta corrente
	        datamovimento TEXT(25) NOT NULL, -- data do transferencia no formato DD/MM/YYYY
	        valor REAL NOT NULL, -- valor da tarifa. Usar duas casas decimais.
	        FOREIGN KEY(idtarifa) REFERENCES tarifa(idtarifa)
        );

		CREATE TABLE IF NOT EXISTS idempotencia (
		    chave_idempotencia TEXT(37) PRIMARY KEY, -- identificacao chave de idempotencia
		    requisicao TEXT(1000), -- dados de requisicao
		    resultado TEXT(1000), -- dados de retorno
            statuscode INTEGER(3), -- status code requisição
            datacriacao TEXT(25) -- data criacao do registro
	    );
    ";
}