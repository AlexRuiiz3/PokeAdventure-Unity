using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonJugador : Pokemon
{
    #region Atributos
    private int experiencia;
    private int experienciaSiguienteNivel = 1;
    #endregion

    public PokemonJugador() : base()
    {
        IDJugador = 0;
        PokemonNumero = 0;
        NumeroEquipado = 0;
        experiencia = 0;
    }

    public PokemonJugador(Pokemon pokemon, int idJugador, int pokemonNumero, int numeroEquipado, int experiencia) : base(pokemon.ID, pokemon.Nombre, pokemon.HP, pokemon.Nivel, pokemon.Ataque, pokemon.Defensa, pokemon.Velocidad, pokemon.Movimientos, pokemon.Tipos, pokemon.Debilidades, pokemon.ImagenDeFrente, pokemon.ImagenDeEspalda)
    {
        IDJugador = idJugador;
        PokemonNumero = pokemonNumero;
        NumeroEquipado = numeroEquipado;
        Experiencia = experiencia;
    }

    //IdJugador
    public int IDJugador { get; }
    //PokemonNumero
    public int PokemonNumero { get; }
    //NumeroEquipado
    public int NumeroEquipado { get; }
    //Experiencia
    public int Experiencia
    {
        get { return experiencia; }
        set
        {
            if (value >= 0)
            {
                experiencia = value;
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="experiencia"></param>
    public void aumentarExperiencia(int experiencia)
    {
        this.experiencia += experiencia;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool comprobarSubirNivel()
    {
        bool subidaNivel = false;
        //experiencia += experiencia;
        if (experiencia >= experienciaSiguienteNivel)
        {
            subidaNivel = true;
            Nivel += 1; //De esta manera se entra en set de Nivel y se controlara que el nivel no pase de 100
            experiencia = 0;//experienciaSiguienteNivel; //Se resetea la experiencia del pokemon
            experienciaSiguienteNivel += 100; //Aumenta la experiencia requerida para el siguiente nivel
        }
        return subidaNivel;
    }
}
