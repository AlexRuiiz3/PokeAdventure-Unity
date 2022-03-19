using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class ConfiguracionEscenaAventura : MonoBehaviour
{
    private const string ORIENTACION_ARRIBA = "Arriba";
    private const string ORIENTACION_ABAJO = "Abajo";
    private const string ORIENTACION_DERECHA = "Derecha";
    private const string ORIENTACION_IZQUIERDA = "Izquierda";

    private Texture2D[] texturasEntrenadores;
    private List<PosicionTrainerNPC> posicionesEntrenadores;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("NameLastScene") == "LobbyScene") //Para que solo se creen la primera vez que se entre en la ecensa,que sera cuando la ultima escena cargada sea LobbyScene
        {
            texturasEntrenadores = Resources.LoadAll<Texture2D>("Imagenes/Trainers/");
            posicionesEntrenadores = new List<PosicionTrainerNPC>();

            switch (SceneManager.GetActiveScene().name)
            {
                case "SnowScene":
                    posicionesEntrenadores = new List<PosicionTrainerNPC> {
                    new PosicionTrainerNPC(new Vector2(-16.7f, 0.6f),new List<string>{ORIENTACION_ARRIBA,ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-7.62f, 5.02f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-0.38f, 0f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(6.41f, -11.27f),new List<string>{ORIENTACION_ARRIBA,ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(9.3f, -3.475f),new List<string>{ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-17.24f, 8.08f),new List<string>{ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-2.5f, 6.27f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(10.46f, -10.46f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-20.16f, -11.09f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-14.06f, -5.49f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(8.38f, 1.48f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(2.51f, 5.39f),new List<string>{ORIENTACION_ABAJO})
                };
                    break;
                case "CityScene":
                    posicionesEntrenadores = new List<PosicionTrainerNPC> {
                    new PosicionTrainerNPC(new Vector2(-12.5f, 6.9f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(12.5f, 43.25f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(7.95f, 25f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(15.15f, 14f),new List<string>{ORIENTACION_ARRIBA}),
                    new PosicionTrainerNPC(new Vector2(15.2f, -2.51f),new List<string>{ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-6.06f, 39.35f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-12.62f, 21.6f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-19f, 15.25f),new List<string>{ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(21.48f, 16.73f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(30.5f, 10.86f),new List<string>{ORIENTACION_ARRIBA})
                };
                    break;
                case "RouteScene":
                    posicionesEntrenadores = new List<PosicionTrainerNPC> {
                    new PosicionTrainerNPC(new Vector2(0.6f, 2.2f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-5.62f, -2.1f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-0.85f, 17.8f),new List<string>{ORIENTACION_DERECHA,ORIENTACION_ARRIBA}),
                    new PosicionTrainerNPC(new Vector2(8.84f, 32f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-9.5f, 29.53f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-14.8f, -0.5f),new List<string>{ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-0.72f, 9.76f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(13.47f, 13.48f),new List<string>{ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-2.48f, 35.18f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(3.5f, 35.18f),new List<string>{ORIENTACION_ABAJO})
                };
                    break;
                case "ForestScene":
                    posicionesEntrenadores = new List<PosicionTrainerNPC> {
                    new PosicionTrainerNPC(new Vector2(8.5f, 10.6f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(5.29f, -4.4f),new List<string>{ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-15.4f, 21.5f),new List<string>{ORIENTACION_DERECHA,ORIENTACION_IZQUIERDA,ORIENTACION_ARRIBA}),
                    new PosicionTrainerNPC(new Vector2(-9.46f, -2.56f),new List<string>{ORIENTACION_ARRIBA,ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-4.43f, -14.4f),new List<string>{ORIENTACION_ARRIBA}),
                    new PosicionTrainerNPC(new Vector2(7.3f, -10f),new List<string>{ORIENTACION_ARRIBA}),
                    new PosicionTrainerNPC(new Vector2(20.83f, 6f),new List<string>{ORIENTACION_IZQUIERDA}),
                    new PosicionTrainerNPC(new Vector2(-17.46f, -1.19f),new List<string>{ORIENTACION_ABAJO,ORIENTACION_DERECHA}),
                    new PosicionTrainerNPC(new Vector2(-8.55f, 6.45f),new List<string>{ORIENTACION_ABAJO}),
                    new PosicionTrainerNPC(new Vector2(-18.53f, -11.51f),new List<string>{ORIENTACION_ABAJO})
                };
                    break;
            }
            crearTrainersNPC();
        }
    }

    private void crearTrainersNPC()
    {
        CapsuleCollider2D collider2D;
        SpriteRenderer spriteRenderer;
        PosicionTrainerNPC posicionTrainer;
        Texture2D texturaAleatoria;

        string orientacionAleatoria;
        int randomPosicionTrainer = (int)Random.Range(0f, posicionesEntrenadores.Count);
        int numeroEntrenadores = (int)(Random.Range(2f, 6f)); //Entrenadores entre 2 y 5
        Debug.Log(numeroEntrenadores);
        for (int i = 0; i < numeroEntrenadores; i++)
        {
            //Se crea el gameObject que sera un entrenador
            GameObject trainer = new GameObject();
            //Se a�ade un script 
            trainer.AddComponent<TrainerNPC>();

            //Se obtiene una posicion de forma aleatoria y se le a�ade al gameObject. Despues se le a�ade un escalado
            posicionTrainer = obtenerPosicionAleatoriaTrainer();
            
            //De la posicion obtenida de forma aleatoria, de las orientaciones que tiene se obtiene una de forma que indicara que tipo de sprite se obtendra
            orientacionAleatoria = posicionTrainer.Orientaciones[(int)Random.Range(0f, posicionTrainer.Orientaciones.Count)];
            
            //Se prepara el gameObject que sera la vision que tendra para detectar al jugador y se le asigna
            prepararGameObjectZonaVision(orientacionAleatoria).transform.parent = trainer.transform;
           
            //Se modifica su posicion y escalado
            trainer.transform.position = posicionTrainer.Posicion;
            trainer.transform.localScale = new Vector2(2.2f, 2.2f);

            //Se le a�ade un collider
            collider2D = trainer.AddComponent<CapsuleCollider2D>();
            collider2D.size = new Vector2(0.26f, 0.4f);

            //Se le a�ade un spriteRendere al cual se le modificara el sprite
            spriteRenderer = trainer.AddComponent<SpriteRenderer>();

            //Se obtiene una textura(Sera una imagen de un NPC) de forma aleatoria
            texturaAleatoria = obtenerTexturaAleatoriaTrainer();

            //Para asignar el sprite al SpriteRenderer. Se obtienen todos los sprite que tiene la textura que se obtuvo de forma
            //aleatoria.Los sprite de la textura son siempre 4 y el nombre que tienen es Arriba,Abajo,Derecha,Izquierda.
            //Entonces se va obteniendo uno a uno los sprite y el que su nombre coincida con el de la orientacion(Obtenida de
            //forma aleatoria) de la posicion que se obtuvo antes de forma aleatoria, se obtiene(Ese sprite).
            spriteRenderer.sprite = (from spriteNPC in Resources.LoadAll<Sprite>("Imagenes/Trainers/" + texturaAleatoria.name)
                                     where spriteNPC.name == orientacionAleatoria
                                     select spriteNPC).First();
            spriteRenderer.sortingLayerName = "NPC";

            //Se prepara un gameObject que contendra un sprite de exclamacion
            prepararGameObjectExclamacion(posicionTrainer.Posicion).transform.parent = trainer.transform;
            DontDestroyOnLoad(trainer);//Para que cuando se vaya a la escena de un combate se mantengan la escena
        }
    }
    private Texture2D obtenerTexturaAleatoriaTrainer() {
        Texture2D texture = new Texture2D(0,0); //Textura por defecto
        bool texturaObtenida = false;
        int randomTexturaTrainer = (int)Random.Range(0f, texturasEntrenadores.Length);

        while (!texturaObtenida) {

            if (texturasEntrenadores[randomTexturaTrainer] != null)
            {
                texture = texturasEntrenadores[randomTexturaTrainer];
                texturaObtenida = true;
                texturasEntrenadores[randomTexturaTrainer] = null; //Se pone a null, para "Eliminar" del array la textura que se acaba de obtener,asi se evita que haya dos entrenadores iguales
            }
            else {
                randomTexturaTrainer = (int)Random.Range(0f, texturasEntrenadores.Length);
            }
        }
        return texture;
    }

    private PosicionTrainerNPC obtenerPosicionAleatoriaTrainer()
    {
        PosicionTrainerNPC posicionTrainer = null; 
        bool posicionObtenida = false;
        int randomPosicionTrainer = (int)Random.Range(0f, posicionesEntrenadores.Count);

        while (!posicionObtenida)
        {
            //Debug.Log(randomPosicionTrainer);
            if (posicionesEntrenadores[randomPosicionTrainer] != null)
            {
                posicionTrainer = posicionesEntrenadores[randomPosicionTrainer];
                posicionObtenida = true;
                posicionesEntrenadores[randomPosicionTrainer] = null;//Se pone a null, para "Eliminar" del array de las posiciones que se acaba de obtener,asi se evita que haya dos entrenadores en la misma posicion
            }
            else
            {
                randomPosicionTrainer = (int)Random.Range(0f, posicionesEntrenadores.Count);
            }
        }
        return posicionTrainer;
    }
    private GameObject prepararGameObjectZonaVision(string orientacion) {
        GameObject zonaVision = new GameObject();

        BoxCollider2D boxCollider = zonaVision.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.offset = new Vector2(0.003630318f, -0.6459866f);
        boxCollider.size = new Vector2(0.1360126f, 1.088027f);
        //Se a�ade el sprict que tiene la funcion ontrigger para cuando el jugador entre en el boxCollider
        zonaVision.AddComponent<ConfiguracionTrainerNPC>();
        
        //Por defecto esta mirando abajo 
        Vector3 rotacionZonaVision = new Vector3(0,0,360);
        switch (orientacion) {
            case ORIENTACION_ARRIBA:
                rotacionZonaVision.z = 180f;
                break;
            case ORIENTACION_DERECHA:
                rotacionZonaVision.z = 90f;
                break;
                
            case ORIENTACION_IZQUIERDA:
                rotacionZonaVision.z = 270f;
                break;
        }
        zonaVision.transform.Rotate(rotacionZonaVision); //La rotacion sera la zona en la que apunte el boxCollider y eso depende de la orientacion que tenga el Trainer
        return zonaVision;
    }

    private GameObject prepararGameObjectExclamacion(Vector2 posicionTrainer) {
        GameObject iconoExclamacion = new GameObject();
        iconoExclamacion.transform.position = new Vector2(posicionTrainer.x, posicionTrainer.y +0.75f);
        iconoExclamacion.transform.localScale = new Vector3(3.5f,3.5f,0f);

        SpriteRenderer spriteRendere = iconoExclamacion.AddComponent<SpriteRenderer>();
        spriteRendere.sprite = Resources.Load<Sprite>("Imagenes/iconoExclamacion");
        spriteRendere.sortingLayerName = "NPC";
        iconoExclamacion.SetActive(false);

        return iconoExclamacion;
    }
}
