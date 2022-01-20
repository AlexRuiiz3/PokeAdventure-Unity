using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Linq;


public class Pokemon
{
    #region Atributos
    private int hp;
    private int nivel;
    #endregion

    #region Constructores
    //Contructor sin parametros
    public Pokemon()
    {
        ID = 0;
        Nombre = "";
        hp = 0;
        nivel = 0;
        Ataque = 0;
        Defensa = 0;
        Velocidad = 0;
        Movimientos = new List<MovimientoPokemon>();
        Tipos = new List<string>();
        Debilidades = new List<string>();
        ImagenDeFrente = new byte[0];
        ImagenDeEspalda = new byte[0];
        HPMaximos = 100;
    }
    //Constructor con parametros
    public Pokemon(int id, string nombre, int hp, int nivel, int ataque, int defensa, int velocidad, List<MovimientoPokemon> movimientos, List<string> tipos, List<string> debilidades, byte[] imagenDeFrente, byte[] imagenDeEspalda)
    {
        ID = id;
        Nombre = nombre;
        this.hp = hp;
        this.nivel = nivel;
        Ataque = ataque;
        Defensa = defensa;
        Velocidad = velocidad;
        Movimientos = movimientos;
        Tipos = tipos;
        Debilidades = debilidades;
        ImagenDeFrente = imagenDeFrente;
        ImagenDeEspalda = imagenDeEspalda;
        HPMaximos = hp;
    }
    /*
    public Pokemon(PokeApiNet.Pokemon pokemonApi)
    {
        ID = pokemonApi.Id;
        Nombre = pokemonApi.Name;
        hp = (from pokemonStats in pokemonApi.Stats
              where pokemonStats.Stat.Name == "hp"
              select pokemonStats).First().Effort;
        Ataque = (from pokemonStats in pokemonApi.Stats
                  where pokemonStats.Stat.Name == "attack"
                  select pokemonStats).First().Effort;
        Defensa = (from pokemonStats in pokemonApi.Stats
                   where pokemonStats.Stat.Name == "defense"
                   select pokemonStats).First().Effort;
        Velocidad = (from pokemonStats in pokemonApi.Stats
                     where pokemonStats.Stat.Name == "speed"
                     select pokemonStats).First().Effort;
        HPMaximos = hp;

        Tipos = ListadosPokemon.obtenerNombreTiposPokemon(pokemonApi.Types,"es").Result;
        Debilidades = ListadosPokemon.obtenerNombreTiposDebilesPokemon(pokemonApi.Types,"es").Result;
        /*
        ImagenDeFrente = imagenDeFrente;
        ImagenDeEspalda = imagenDeEspalda;
    }*/
    #endregion

    #region Metodos Fundamentales(Propiedades)
    //id
    public int ID { get; }
    //nombre
    public string Nombre { get; set; }
    //ps
    public int HP
    {
        get { return hp; }
        set
        {
            if (value >= HPMaximos) //Pokemon ps 150, se le ataca(20) y se queda con 130 si se usa una pocion que le cure 100 de esta manera se quedara en su maximo(150) y no con 230 cuando sus ps maximos eran 150
            {
                hp = HPMaximos;
            }
            else
            {
                if (value > 0)
                {
                    hp = value;
                }
                else
                {
                    hp = 0;
                }
            }
        }
    }
    //HPMaximos
    public int HPMaximos { get; } //Servira para tener guardado los ps maximo de un pokemon ya que cuando se cure a un pokemon hay que saber hasta donde curarlo.

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
    //ataque
    public int Ataque { get; set; }
    //defensa
    public int Defensa { get; set; }
    //velocidad
    public int Velocidad { get; set; }
    //movimientos
    public List<MovimientoPokemon> Movimientos { get; set; }
    //tipos
    public List<string> Tipos { get; }
    //debilidades
    public List<string> Debilidades { get; }
    //ImagenDeFrente
    public byte[] ImagenDeFrente { get; }
    //ImagenDeEspalda
    public byte[] ImagenDeEspalda { get; }
    #endregion


}
