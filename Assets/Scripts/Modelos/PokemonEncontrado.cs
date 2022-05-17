using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PokemonEncontrado
{
    public PokemonEncontrado(int id, string nombre) {
        Id = id;
        Nombre = nombre;

    }
    public int Id { get; set; }
    public string Nombre { get; set; }
}

