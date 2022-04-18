using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ListadosMovimientoBL
{
    public static List<MovimientoPokemon> obtenerMovimientosPokemon(int idJugador, int idPokemon, int numeroPokemon)
    {
        return ListadosMovimientoDAL.obtenerMovimientosPokemon(idJugador, idPokemon, numeroPokemon);
    }
}
