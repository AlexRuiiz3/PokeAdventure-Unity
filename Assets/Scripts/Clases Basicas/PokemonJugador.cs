using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonJugador : Pokemon
{
    #region Atributos
    private int nivel;
    private int experiencia;
    private int experienciaSiguienteNivel = 1000;
    #endregion

    public PokemonJugador(int idJugador, int pokemonNumero, int numeroEquipado, int nivel, int experiencia) : base()
    {
        IDJugador = 0;
        PokemonNumero = 0;
        NumeroEquipado = 0;
        nivel = 1;
        experiencia = 0;
    }

    public PokemonJugador(Pokemon pokemon, int idJugador, int pokemonNumero, int numeroEquipado, int nivel, int experiencia) : base(pokemon.ID, pokemon.Nombre, pokemon.HP, pokemon.Nivel, pokemon.Ataque, pokemon.Defensa, pokemon.Velocidad, pokemon.Movimientos, pokemon.Tipos, pokemon.Debilidades, pokemon.ImagenDeFrente, pokemon.ImagenDeEspalda)
    {
        IDJugador = idJugador;
        PokemonNumero = pokemonNumero;
        NumeroEquipado = numeroEquipado;
        Nivel = nivel;
        Experiencia = experiencia;
    }
    //IdJugador
    public int IDJugador { get; }
    //PokemonNumero
    public int PokemonNumero { get; }
    //NumeroEquipado
    public int NumeroEquipado { get; }
    //Nivel
    public int Nivel
    {
        get { return nivel; }
        set
        {
            if (value >= 1 && value <= 100)
            {
                nivel = value;
            }
        }
    }
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

        if (experiencia >= experienciaSiguienteNivel)
        {
            subidaNivel = true;
            Nivel = nivel + 1; //De esta manera se entra en set de Nivel y se controlara que el nivel no pase de 100
            experiencia = 0; //Se resetea la experiencia del pokemon
            experienciaSiguienteNivel += 200; //Aumenta la experiencia requerida para el siguiente nivel
        }
        return subidaNivel;
    }
}
