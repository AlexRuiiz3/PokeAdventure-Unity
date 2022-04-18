using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ListadosTipoBL
{
    public static List<string> obtenerTiposPokemon(int idPokemon)
    {
        return ListadosTipoDAL.obtenerTiposPokemon(idPokemon);
    }

    public static List<string> obtenerTiposDebilesTipo(string tipo)
    {
        return ListadosTipoDAL.obtenerTiposDebilesTipo(tipo);
    }
}
