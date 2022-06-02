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
    public static int obtenerMultiplicadorPorEfectividad(List<string> debilidadesPokemon, string tipoMovimientoAtaca, TextMeshProUGUI textoMostrarResultado)
    {
        int multiplicador = 1;
        bool danhoSuperEfectuivo = debilidadesPokemon.Contains(tipoMovimientoAtaca);
        if (danhoSuperEfectuivo)//Si el tipo del movimiento que va a atacar es de un tipo de los cuales el pokemon es debil, el daño es por 2
        {
            multiplicador = 2;
            textoMostrarResultado.text = $"Es supereficaz!";
        }
        return multiplicador;
    }

    public static int calcularDanhoCausado(int nivelPokemon, int danhoMovimiento, int efectividad, int ataquePokemon, int defensa)
    {
        return (int)(0.01 * 1.5 * efectividad * Random.Range(85, 101) *
                    ((0.2 * nivelPokemon + 1) * ataquePokemon * danhoMovimiento /
                    (20 * defensa) + 2));
    }

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
            textoMostrarResultado.text = $"{nombrePokemon} ha usado {nombreMovimiento}";
        }
        return danho;
    }

    public static int generarExperienciaDerrotarPokemonRival(int nivelPokemon)
    {
        int aleatorioCritico = Random.Range(10, 16); //Entre 10 y 15

        return aleatorioCritico *= nivelPokemon;
    }

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

    public static bool determinarCapturarPokemon(int indicePokeball,int psMaximos, int psActuales) {
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
