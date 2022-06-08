using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
* Clase que sirve para tener almacenados de manera global y rapida datos del jugador. Su principal finalidad es la de guardar datos del jugador pero sin pasar por la base de datos, 
* solo se guardaran en la base de datos cuando el jugador realice la funcion de Guardar Datos, entonces el contenido de esta clase sera obtenido y guardado en la base de datos. 
* De esta manera se puede conseguir la funcionalidad de si cierras el juego y no guardas, pierdes todo lo nuevo conseguido.
*/
public class DatosGuardarJugador
{

    public static List<PokemonEncontrado> PokemonsEncontradosJugador { get; set; }
    public static List<PokemonJugador> PokemonsAlmacenadosPC { get; set; }

}

