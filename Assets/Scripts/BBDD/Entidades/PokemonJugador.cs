using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonJugador : Pokemon
{
    #region Atributos
    private int experiencia;
    private int experienciaSiguienteNivel = 1;
    #endregion
    
    #region Atributos
    //Constructor sin parametros
    public PokemonJugador() : base()
    {
        IDJugador = 0;
        PokemonNumero = 0;
        NumeroEquipado = 0;
        experiencia = 0;
    }
    //Constructor con parametros
    public PokemonJugador(Pokemon pokemon, int idJugador, int pokemonNumero, int numeroEquipado, int experiencia) : base(pokemon.ID, pokemon.Nombre, pokemon.HP, pokemon.Nivel, pokemon.Ataque, pokemon.Defensa, pokemon.Velocidad, pokemon.Movimientos, pokemon.Tipos, pokemon.Debilidades)
    {
        IDJugador = idJugador;
        PokemonNumero = pokemonNumero;
        NumeroEquipado = numeroEquipado;
        Experiencia = experiencia;
    }
    #endregion 
    
    #region Metodos Fundamentales(Propiedades)
    //IdJugador
    public int IDJugador { get; }
    //PokemonNumero
    public int PokemonNumero { get; set; }
    //NumeroEquipado
    public int NumeroEquipado { get; set; }
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
    #endregion 

    #region Metodos añadidos
    /// <summary>
    /// Cabecera: public bool comprobarSubirNivel()
    /// Comentario: Este metodo se encarga 
    /// Entradas: Ninguna
    /// Salidas: bool subidaNivel
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se devolvera un booleano cuyo valor sera:
    ///                  true: Si el pokemon a subido de nivel 
    ///                  flase: Si el pokemon no ha subido de nivel
    /// </summary>
    /// <returns>bool</returns>
    public bool comprobarSubirNivel()
    {
        bool subidaNivel = false;
        //experiencia += experiencia;
        if (experiencia >= experienciaSiguienteNivel)
        {
            subidaNivel = true;
            Nivel += 1; //De esta manera se entra en set de Nivel y se controlara que el nivel no pase de 100
            experiencia = 0;//Se resetea la experiencia del pokemon
            experienciaSiguienteNivel += 100; //Aumenta la experiencia requerida para el siguiente nivel
        }
        return subidaNivel;
    }
    #endregion
    
    #region Metodos Heredados
    //Criterio de igualadad: Que los id y los pokemonNumero de los pokemons sean iguales
    public override bool Equals(object obj)
    {
        bool iguales = false;
        if (this == obj)
        {
            iguales = true;
        }
        else if (obj != null && typeof(PokemonJugador).IsInstanceOfType(obj))
        {
            PokemonJugador pokemon = (PokemonJugador)obj;
            if (ID == pokemon.ID && PokemonNumero == pokemon.PokemonNumero) {
                iguales = true;
            }
        }
        return iguales;
    }
    #endregion
}
