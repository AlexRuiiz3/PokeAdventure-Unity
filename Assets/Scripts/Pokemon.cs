using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Pokemon
{
    #region Atributos
    private int id;
    private string nombre;
    private int ps;
    private int ataque;
    private int defensa;
    private int velocidad;
    private List<MovimientoPokemon> movimientos;
    private List<string> tipos;
    private List<string> debilidades;

    private int psMaximos; //Servira para tener guardado los ps maximo de un pokemon ya que cuando se cure a un pokemon hay que saber hasta donde curarlo.
    #endregion

    #region Constructor por defecto
    public Pokemon()
    {
        id = 0;
        nombre = "";
        ps = 0;
        ataque = 0;
        defensa = 0;
        velocidad = 0;
        movimientos = new List<MovimientoPokemon>();
        tipos = new List<string>();
        debilidades = new List<string>();
        psMaximos = 0;
    }
    #endregion

    #region Constructor con parametros
    public Pokemon(int id, string nombre, int ps, int ataque, int defensa, int velocidad, List<MovimientoPokemon> movimientos, List<string> tipos, List<string> debilidades)
    {
        this.id = id;
        this.nombre = nombre;
        this.ps = ps;
        this.ataque = ataque;
        this.defensa = defensa;
        this.velocidad = velocidad;
        this.movimientos = movimientos;
        this.tipos = tipos;
        this.debilidades = debilidades;
        psMaximos = ps;
    }
    #endregion

    #region Metodos Fundamentales(Propiedades)
    //id
    public int ID { get; }
    //nombre
    public string Nombre { get; set; }
    //ps
    public int PS
    {
        get { return ps; }
        set {
            if (value >= psMaximos) //Pokemon ps 150, se le ataca(20) y se queda con 130 si se usa una pocion que le cure 100 de esta manera se quedara en su maximo(150) y no con 230 cuando sus ps maximos eran 150
            { 
                ps = psMaximos;
            }else 
            {
                if (value > 0)
                {
                    ps = value;
                }
                else {
                    ps = 0;
                }
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
    public List<MovimientoPokemon> Movimentos { get; set; }
    //tipos
    public List<string> Tipos { get; }
    //debilidades
    public List<string> Debilidades { get; }
    #endregion


}
