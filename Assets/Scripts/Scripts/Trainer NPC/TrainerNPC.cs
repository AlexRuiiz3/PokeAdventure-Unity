using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainerNPC : MonoBehaviour
{
    #region Constructores
    //Constructor sin parametros
    public TrainerNPC() {
        Frases = new List<string>();
        EquipoPokemon = new List<Pokemon>();
        Mochila = new List<ItemConCantidad>();
        derrotado = false;
    }
    //Constructor con parametros
    public TrainerNPC(List<string> frases, List<Pokemon> equipoPokemon, List<ItemConCantidad> mochila)
    {
        Frases = frases;
        EquipoPokemon = equipoPokemon;
        Mochila = mochila;
    }
    #endregion
    
    #region Metodos fundamentales(Propiedades)
    //Frases
    public Sprite Imagen { get; set; }
    public List<string> Frases { get; set; }
    public List<string> FrasesDerrotado { get; set; }
    public bool derrotado { get; set; }
    public int dineroAlDerrotar { get; set; }

    //EquipoPokemon
    public List<Pokemon> EquipoPokemon { get; set; }
    //Mochila
    public List<ItemConCantidad> Mochila { get; set; }
    #endregion


    #region Metodos añadidos
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!derrotado && collision.CompareTag("Player") && !collision.isTrigger)//Si entra en contacto con el jugador  
        {
            Imagen = (from spriteNPC in Resources.LoadAll<Sprite>("Imagenes/Trainers/" + transform.parent.gameObject.name)
                                       where spriteNPC.name == "Abajo"
                                       select spriteNPC).First();
            UtilidadesEscena.activarPausarMusicaEscenaActiva(false);
            UtilidadesEscena.activarMusicaTemporal($"Batalla/TrainerSeesYou{Random.Range(1, 5)}", true);
            DatosGenerales.trainerLuchando = this;
            PlayerPrefs.SetString("InteraccionConObjeto", transform.parent.tag);
            StartCoroutine(activarExclamacionTrainerCombate());
            GameObject dialogo = Resources.FindObjectsOfTypeAll<GameObject>().First(g => g.name == "CanvasDialogo");
            dialogo.SetActive(true);
            dialogo.transform.GetChild(0).gameObject.SetActive(true);//La imagen del dialogo
            ControlDialogos controlDialogos = dialogo.transform.GetChild(0).gameObject.GetComponent<ControlDialogos>();
            controlDialogos.ListaFrases = Frases.ToArray();
            controlDialogos.activarDialogo();
        }
    }

    /// <summary>
    /// Cabecera: IEnumerator activarExclamacionTrainerCombate()
    /// Comentario: Esta corrutina se encarga de activar la imagen de exclamacion que tiene un entrenador
    /// Entradas: Ninguna
    /// Salidas: Ninguna
    /// Precondiciones: Ninguna
    /// Postcondiciones: Se activara la imagen de una exclacion duranto un breve periodo de tiempo y despues se desactivara
    /// </summary>
    /// <returns></returns>
    public IEnumerator activarExclamacionTrainerCombate()
    {
        PlayerPrefs.SetString("EntrenadorLuchando", transform.parent.name);
        GameObject iconoExclamacion = gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
        iconoExclamacion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        iconoExclamacion.SetActive(false);
        
        gameObject.SetActive(false); //Se desactiva el gameObject que tiene el boxCollider, asi solo se podra combatir con el mismo entrenador solo una vez
        StopCoroutine(activarExclamacionTrainerCombate());
    }
    #endregion
}
