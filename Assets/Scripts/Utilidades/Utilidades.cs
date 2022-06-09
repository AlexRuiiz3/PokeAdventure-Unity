using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class Utilidades
{

    public static void obtenerDatosJugador(string nombreUsuario, string contrasenha)
    {
        try
        {
            ClsJugador jugadorBase = ListadosJugadorBL.obtenerJugador(nombreUsuario, contrasenha);
            List<PokemonJugador> pokemonsJugador = ListadosPokemonsJugadorBL.obtenerPokemonsJugadorEquipados(jugadorBase.ID);
            List<ItemConCantidad> itemsJugador = ListadosItemBL.obtenerItemsJugador(jugadorBase.ID);

            Resources.FindObjectsOfTypeAll<GameObject>()
                       .FirstOrDefault(g => g.CompareTag("Player"))
                       .GetComponent<PlayerController>().Jugador = new Jugador(jugadorBase, pokemonsJugador, itemsJugador);

            DatosGuardarJugador.PokemonsEncontradosJugador = ListadosPokemonEncontradosJugadorBL.obtenerPokemonsEncontradosDeJugador(jugadorBase.ID);
            DatosGuardarJugador.PokemonsAlmacenadosPC = ListadosPokemonsJugadorBL.obtenerPokemonsNoEquipadosJugador(jugadorBase.ID);
        }
        catch (Exception)
        {
            throw new Exception();
        }

    }
    /// <summary>
    /// Cabecera: public static bool comprobarCadenaVacia(string cadena)
    /// Comentario: Este metodo se encarga de comprobar si la cadena rebidida como parametros se encuentra vacia.
    /// Entradas: string cadena
    /// Salidas: bool
    /// Precondiciones: cadena debe ser distinto de null.
    /// Postcondiciones: Se devolvera un booleano que puede tomar dos posibles valores:
    ///                  true: Si la cadena recibida esta vacia.
    ///                  false: Si la cadena recibida no esta vacia.
    ///                  Si se recibide una cadena que este a null, se producira un NullPointerException.
    /// </summary>
    ///<param name="cadena"></param>
    public static bool comprobarCadenaVacia(string cadena)
    {
        return string.IsNullOrEmpty(cadena.Trim());
    }

    public static void cambiarPrimerCaracterMayuscula(string cadena)
    {
        cadena = cadena.Substring(0, 1).ToUpper() + cadena.Substring(1);
    }

   

    /// <summary>
    /// Cabecera: public static List<string> obtenerFrasesAleatoriasTrainer()
    /// Comentario: Este metodo se encarga de obtener una listado de string de forma aleatoria.
    /// Entradas: Ninguna
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtendra un List<string>
    /// </summary>
    public static List<string> obtenerFrasesAleatoriasTrainer()
    {
        List<List<string>> frasesEntrenadores = new List<List<string>> {
        new List<string> { "Hola viajero, veo que tienes un buen equipo.", "Dejame que compruebe si es verdad." },
        new List<string> { "¡Eh! ¡Tu equipo Pokémon me da buenas vibraciones! ", "¿Te importa que vea lo entrenado que está?" },
        new List<string> { "Te voy hacer comer polvo.", "Para que te hagas una idea, como el que va a comer Santi con el proyecto final." },
        new List<string> { "¿Qué necesita mi vista?"},
        new List<string> { "¡¿A quién llamas lagartija?! ¡Lagartija!"},
        new List<string> { "Solo podemos hacer una cosa", "Lo sabes ¿verdad?" },
        new List<string> { "Los entrenadores deben responder siempre y estar dispuestos a enfrentarse a cualquier reto."},
        new List<string> { "Muestras bastante confianza en tus habilidades.", "¡A ver que vales!" },
        new List<string> { "Estoy plenamente en forma.", "¡Listo para luchar!" },
        new List<string> { "Estoy aburridisimo.", "¿Echamos un combate para acabar con este tedio?" },
        new List<string> { " Mmm...", "Creo que puedo predecir cómo acabará esto..." },
        new List<string> { "Por muy fuerte que seas, si bajas la guardia acabarás teniendo problemas."}
    };
        return frasesEntrenadores[UnityEngine.Random.Range(0, frasesEntrenadores.Count)];
    }

    /// <summary>
    /// Cabecera: public static List<string> obtenerFrasesAleatoriasTrainerDerrotado()
    /// Comentario: Este metodo se encarga de obtener una listado de string de forma aleatoria.
    /// Entradas: Ninguna
    /// Salidas: List<string>
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se obtendra un List<string>
    /// </summary>
    public static List<string> obtenerFrasesAleatoriasTrainerDerrotado()
    {
        List<List<string>> frasesEntrenadoresDerotao = new List<List<string>> {
        new List<string> { "Pues si que eres fuerte."},
        new List<string> { "Pufff que combate más bueno, he aprendido mucho.", "Espero que volvamos a combatir otra vez." },
        new List<string> { "Que sepas que me he dejado.", "No quiero abusar de un crio." },
        new List<string> { "...","...","..."},
        new List<string> { "¡Vaya decepción!", "Tanto tiempo entrenando para esto." },
        new List<string> { "¡Tu si que sabes como sortear obstaculos con tu equipo Pokemon!"},
        new List<string> { "Anda, ríete de mi si quieres.", "Te dejo." },
        new List<string> { "Tienes bastante talento.", "Me has dejado boquiabierto." },
        new List<string> { "El mundo está lleno de Pokémon y de toda clase de entrenadores."},
        new List<string> { "Me siento mucho más fuerte, aunque haya perdido.", "Al fin y al cabo, hemos hecho todo lo que podíamos." },
        new List<string> { "Pokémon, niveles y números... ¿Qué más da? Lo importante es ganar.", "Los números no sirven para nada." },
        new List<string> { "Los entrenadores llevan a todo tipo de Pokémon con ellos, ¿no?", "Así que cuanto mas luches, ¡más llenaras tu Pokédex!" }
    };
        return frasesEntrenadoresDerotao[UnityEngine.Random.Range(0, frasesEntrenadoresDerotao.Count)];
    }
}
