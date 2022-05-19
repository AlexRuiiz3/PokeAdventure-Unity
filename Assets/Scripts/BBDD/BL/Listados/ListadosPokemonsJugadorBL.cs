using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosPokemonsJugadorBL
{

    public static List<PokemonJugador> obtenerPokemonsNoEquipadosJugador(int id)
    {
        return ListadosPokemonsJugadorDAL.obtenerPokemonsNoEquipadosJugador(id);
    }

    public static List<PokemonJugador> obtenerPokemonsJugadorEquipados(int id)
    {
        return ListadosPokemonsJugadorDAL.obtenerPokemonsJugadorEquipados(id);
    }
    
    public static int obtenerNumeroPokemonsJugador(int id)
    {
        return ListadosPokemonsJugadorDAL.obtenerNumeroPokemonsJugador(id);
    }
}

