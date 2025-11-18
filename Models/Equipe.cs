using System;
using System.Collections.Generic;

namespace MeuPrimeiroMvc.Models;

public partial class Equipe
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Imagem { get; set; }

    public virtual ICollection<Jogador> Jogadors { get; set; } = new List<Jogador>();
}
