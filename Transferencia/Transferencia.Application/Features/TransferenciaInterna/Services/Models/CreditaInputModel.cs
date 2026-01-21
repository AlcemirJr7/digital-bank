using Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Transferencia.Application.Features.TransferenciaInterna.Services.Models;

public record CreditaInputModel(
    [Required] int NumeroConta,
    [Required] decimal Valor, 
    string Tipo = TipoMovimento.Credito);