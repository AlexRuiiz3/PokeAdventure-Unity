using System.Linq;
using UnityEngine;


namespace Assets.Scripts.Utilidades
{
    public class UtilidadesEscena : MonoBehaviour
    {
        public static void eliminarGameObjectsItemsYEntrenadores() {
           GameObject[] trainersYItems = GameObject.FindGameObjectsWithTag("Trainer");
           trainersYItems.Concat(GameObject.FindGameObjectsWithTag("Item"));

           foreach (GameObject gameObject in trainersYItems) {
                Destroy(gameObject);
           }
        }
    }
}
