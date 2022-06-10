using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class UtilidadesSystemaBatalla
{
    /// <summary>
    /// Cabecera: public static void modificarBarraSalud(Image barraSalud, int hp, int hpMaximos)
    /// Comentario: Este metodo se encarga de modificar la imagen que representa la vida de un pokemon en funcion de la vida que tenga este.
    /// Entradas: Image barraSalud, int hp, int hpMaximos
    /// Salidas: Ninguna
    /// Precondiciones: barraSalud no debe estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se modificara la imagen de vida de un pokemon, cambiando el color de esta en funcion de la vida que tenga el pokemon.
    /// </summary>
    /// <param name="barraSalud"></param>
    /// <param name="hp"></param>
    /// <param name="hpMaximos"></param>
    public static void modificarBarraSalud(Image barraSalud, int hp, int hpMaximos)
    {
        barraSalud.transform.localScale = new Vector3((float)hp / hpMaximos, 1f, 1f);

        if (barraSalud.transform.localScale.x >= 0.5f)
        {
            barraSalud.color = new Color32(0, 255, 106, 255);//Verde
        }
        else if (barraSalud.transform.localScale.x < 0.15f)
        {
            barraSalud.color = Color.red;
        }
        else
        {
            barraSalud.color = Color.yellow;
        }
    }
    
    /// <summary>
    /// Cabecera: public static int obtenerMultiplicadorPorEfectividad(List<string> debilidadesPokemon, string tipoMovimientoAtaca, TextMeshProUGUI textoMostrarResultado)
    /// Comentario: Este metodo se encarga de obtener un multiplicador en funcion de si movimiento de ataque es superefectivo o no.
    /// Entradas: List<string> debilidadesPokemon, string tipoMovimientoAtaca, TextMeshProUGUI textoMostrarResultado
    /// Salidas: int multiplicador
    /// Precondiciones: debilidadesPokemon y textoMostrarResultado no deben estar a null(Sino se producira un NullPointerException)
    /// Postcondiciones: Se obtendra un entero que puede tomar dos valores:
    ///                  1: Si no el tipo de movimiento que ataca no es superefectivo.
    ///                  2: Si el tipo de movimiento que ataca es superefectivo.
    /// </summary>
    /// <param name="debilidadesPokemon"></param>
    /// <param name="tipoMovimientoAtaca"></param>
    /// <param name="textoMostrarResultado"></param>
    /// <returns>int</returns>
    public static int obtenerMultiplicadorPorEfectividad(List<string> debilidadesPokemon, string tipoMovimientoAtaca, TextMeshProUGUI textoMostrarResultado)
    {
        //Battle.PlayerTurn si es multiplicador por 1.5 para que el jugador tenga mas ventaja
        int multiplicador = 1;
        bool danhoSuperEfectuivo = debilidadesPokemon.Contains(tipoMovimientoAtaca);
        if (danhoSuperEfectuivo)//Si el tipo del movimiento que va a atacar es de un tipo de los cuales el pokemon es debil, el daño es por 2
        {
            multiplicador = 2;
            textoMostrarResultado.text = $"Es supereficaz!";
        }
        return multiplicador;
    }

    /// <summary>
    /// Cabecera: public static int calcularDanhoCausado(int nivelPokemon, int danhoMovimiento, int efectividad, int ataquePokemon, int defensa)
    /// Comentario: Este metodo se encarga de calcular cuanto daño va hacer un pokemon.
    /// Entradas: int nivelPokemon, int danhoMovimiento, int efectividad, int ataquePokemon, int defensa
    /// Salidas: int
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtiene un entero que sera el daño que causara un pokemon al atacar.
    /// </summary>
    /// <param name="nivelPokemon"></param>
    /// <param name="danhoMovimiento"></param>
    /// <param name="efectividad"></param>
    /// <param name="ataquePokemon"></param>
    /// <param name="defensa"></param>
    /// <returns>int</returns>
    public static int calcularDanhoCausado(int nivelPokemon, int danhoMovimiento, int efectividad, int ataquePokemon, int defensa)
    {
        return (int)(0.01 * 1.5 * efectividad * Random.Range(85, 101) *
                    ((0.2 * nivelPokemon + 1) * ataquePokemon * danhoMovimiento /
                    (20 * defensa) + 2));
    }

    /// <summary>
    /// Cabecera: public static int incrementarDanhoMovimientoPorCritico(int danho, int probabilidadCritico, string nombrePokemon, string nombreMovimiento, TextMeshProUGUI textoMostrarResultado)
    /// Comentario: Este metodo se encarga de incrementar el daño de un movimiento.
    /// Entradas: int danho, int probabilidadCritico, string nombrePokemon, string nombreMovimiento, TextMeshProUGUI textoMostrarResultado
    /// Salidas: int danho
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se incrementa el daño que hace un movimiento. Si no se produce un critico el daño de movimiento no se modifica y se devuelve igual que llego.
    /// </summary>
    /// <param name="danho"></param>
    /// <param name="probabilidadCritico"></param>
    /// <param name="nombrePokemon"></param>
    /// <param name="nombreMovimiento"></param>
    /// <param name="textoMostrarResultado"></param>
    /// <returns>int</returns>
    public static int incrementarDanhoMovimientoPorCritico(int danho, int probabilidadCritico, string nombrePokemon, string nombreMovimiento, TextMeshProUGUI textoMostrarResultado)
    {
        int aleatorioCritico = Random.Range(1, 10);
        if (aleatorioCritico <= probabilidadCritico)
        {
            textoMostrarResultado.text = $"{nombrePokemon} ha usado {nombreMovimiento}, golpe critico!";
            danho *= (int)1.3;
        }
        else
        {
            textoMostrarResultado.text = $"{nombrePokemon} ha usado {nombreMovimiento}!";
        }
        return danho;
    }
    /// <summary>
    /// Cabecera: public static int generarExperienciaDerrotarPokemonRival(int nivelPokemon)
    /// Comentario: Este metodo se encarga de generar un entero que sera la experiencia que ganara un pokemon del jugador al derrotar a un pokemon rival.
    /// Entradas: int nivelPokemon
    /// Salidas: int 
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un entero.
    /// </summary>
    /// <param name="nivelPokemon"></param>
    /// <returns>int</returns>
    public static int generarExperienciaDerrotarPokemonRival(int nivelPokemon)
    {
        return Random.Range(10, 16) * nivelPokemon;
    }
    /// <summary>
    /// Cabecera: public static int determinarNivelPokemonRival(List<PokemonJugador> pokemonsJugador)
    /// Comentario: Este metodo se encarga de determinar el nivel que tendra un pokemon rival.
    /// Entradas: int nivelPokemon
    /// Salidas: int nivelPokemonRival
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un entero.
    /// </summary>
    /// <param name="nivelPokemon"></param>
    /// <returns>int</returns>
    public static int determinarNivelPokemonRival(List<PokemonJugador> pokemonsJugador)
    {
        int nivelPokemonRival,
            nivelMedioEquipoJugador = 0,
            aletorioSubidaNivel = Random.Range(1, 3),//pokemonsJugador.Count + 1);
            randomSubirNivelONo = Random.Range(1, 3);
        
        foreach (PokemonJugador pokemonJugador in pokemonsJugador)
        {
            nivelMedioEquipoJugador += pokemonJugador.Nivel;
        }
        nivelMedioEquipoJugador /= pokemonsJugador.Count;
        
        if(randomSubirNivelONo == 1){ //Si es 1 se subira el nivel del pokemon rival
            nivelPokemonRival = nivelMedioEquipoJugador + aletorioSubidaNivel;
            if(nivelPokemonRival > 100){
                nivelPokemonRival = 100;
            }
        }else{
            nivelPokemonRival = nivelMedioEquipoJugador - aletorioSubidaNivel;
            if(nivelPokemonRival < 1){
                nivelPokemonRival = 1;
            }
        }
        return nivelPokemonRival;
    }

    /// <summary>
    /// Cabecera: public static bool determinarCapturarPokemon(int indicePokeball,int psMaximos, int psActuales)
    /// Comentario: Este metodo se encarga de determinar si se capturara un pokemon o no.
    /// Entradas: int indicePokeball ,int psMaximos, int psActuales
    /// Salidas: bool
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un booleano que puede tomar dos valores:
    ///                  true:  Si se capturara el pokemon.
    ///                  false: Si no se capturara el pokemon.
    /// </summary>
    /// <param name="indicePokeball"></param>
    /// <param name="psMaximos"></param>
    /// <param name="psActuales"></param>
    /// <returns>bool</returns>
    public static bool determinarCapturarPokemon(int indicePokeball, int psMaximos, int psActuales) {
        int random = Random.Range(1, 101);
        int numero = (3 * psMaximos - 2 * psActuales) * indicePokeball;
        numero /= (3 * psMaximos);
        return random <= numero;
    }

    public static bool comprobarPokemonsVivos(List<Pokemon> pokemons)
    {
        return pokemons.Exists(g => g.HP > 0);
    }
}
