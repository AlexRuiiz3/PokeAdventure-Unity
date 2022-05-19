using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ListadosPokemonEncontradosJugadorBL
{
    public static List<PokemonEncontrado> obtenerPokemonsEncontradosDeJugador(int nombreUsuario)
    {
        return ListadosPokemonEncontradosJugadorDAL.obtenerPokemonsEncontradosDeJugador(nombreUsuario);
    }
    public static int obtenerNumeroPokemonsEncontradosDeJugador(int id)
    {
        return ListadosPokemonEncontradosJugadorDAL.obtenerNumeroPokemonsEncontradosDeJugador(id);
    }
}
