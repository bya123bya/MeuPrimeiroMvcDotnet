using System;
using System.Collections.Generic;

namespace MeuPrimeiroMvc.Models;

public partial class Jogador
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Email { get; set; }

    public string? Senha { get; set; }

    public int? IdEquipe { get; set; }

    public virtual Equipe? IdEquipeNavigation { get; set; }
}
