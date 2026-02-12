using ColinhoDaCa.Domain.Cupons.Enums;

namespace ColinhoDaCa.Domain.Cupons.Entities;

public class CupomDb
{
    public long Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public TipoCupom Tipo { get; set; }
    public decimal Percentual { get; set; }
    public decimal? ValorFixo { get; set; }
    public decimal? MinimoValorTotal { get; set; }
    public int? MinimoPets { get; set; }
    public int? MinimoDiarias { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }

    public static CupomDb Create(string codigo, string descricao, TipoCupom tipo, decimal percentual, decimal? valorFixo, decimal? minimoValorTotal, int? minimoPets, int? minimoDiarias, DateTime? dataInicio, DateTime? dataFim)
    {
        var now = DateTime.Now;
        return new CupomDb
        {
            Codigo = codigo,
            Descricao = descricao,
            Tipo = tipo,
            Percentual = percentual,
            ValorFixo = valorFixo,
            MinimoValorTotal = minimoValorTotal,
            MinimoPets = minimoPets,
            MinimoDiarias = minimoDiarias,
            DataInicio = dataInicio,
            DataFim = dataFim,
            Ativo = true,
            DataInclusao = now,
            DataAlteracao = now
        };
    }

    public void Alterar(string codigo, string descricao, TipoCupom tipo, decimal percentual, decimal? valorFixo, decimal? minimoValorTotal, int? minimoPets, int? minimoDiarias, DateTime? dataInicio, DateTime? dataFim)
    {
        Codigo = codigo;
        Descricao = descricao;
        Tipo = tipo;
        Percentual = percentual;
        ValorFixo = valorFixo;
        MinimoValorTotal = minimoValorTotal;
        MinimoPets = minimoPets;
        MinimoDiarias = minimoDiarias;
        DataInicio = dataInicio;
        DataFim = dataFim;
        DataAlteracao = DateTime.Now;
    }

    public void Inativar()
    {
        Ativo = false;
        DataAlteracao = DateTime.Now;
    }

    public decimal CalcularDesconto(decimal valorTotal, int quantidadePets, int quantidadeDiarias)
    {
        if (!Ativo) return 0;
        if (DataInicio.HasValue && DateTime.Now < DataInicio.Value) return 0;
        if (DataFim.HasValue && DateTime.Now > DataFim.Value) return 0;

        return Tipo switch
        {
            TipoCupom.PercentualSobreTotal => valorTotal * (Percentual / 100),
            TipoCupom.PercentualPorPetComMinimo => 
                quantidadePets >= (MinimoPets ?? 0) 
                    ? (valorTotal / quantidadePets) * (Percentual / 100) * quantidadePets 
                    : 0,
            TipoCupom.PercentualPorPetComDiarias => 
                quantidadePets >= (MinimoPets ?? 0) && quantidadeDiarias >= (MinimoDiarias ?? 0)
                    ? (valorTotal / quantidadePets) * (Percentual / 100) * quantidadePets
                    : 0,
            TipoCupom.ValorFixoComMinimo =>
                valorTotal >= (MinimoValorTotal ?? 0)
                    ? ValorFixo ?? 0
                    : 0,
            _ => 0
        };
    }
}
