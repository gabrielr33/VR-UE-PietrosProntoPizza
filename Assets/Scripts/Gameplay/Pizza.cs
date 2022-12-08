using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : MonoBehaviour
    {

        [SerializeField] private bool tomatoSauce;
        [SerializeField] private bool mozzarella;
        [SerializeField] private bool mushrooms;
        [SerializeField] private bool olives;
        [SerializeField] private bool onions;
        [SerializeField] private bool pepper;
        [SerializeField] private bool egg;
        [SerializeField] private bool salami;
        [SerializeField] private bool bacon;
        [SerializeField] private Material mat_tomato;
        [SerializeField] private Material mat_mozzarella;


        public List<PizzaIngredient> Ingredients { get; set; }

        private void Start()
        {
            // TODO for testing purposes only
            Ingredients = new List<PizzaIngredient>();
            Ingredients.Add(PizzaIngredient.Mozzarella);
            Ingredients.Add(PizzaIngredient.TomatoSauce);
            Ingredients.Add(PizzaIngredient.Onion);
        }

        private void OnTriggerEnter(Collider other)
        {


            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null)
                return;

            enableIngredient(other);
           


            
            Ingredients.Add(ingredient.IngredientType);
        }

        private void enableIngredient(Collider other)
        {
            foreach(Transform child in transform)
            {
                if(child.tag == other.tag)
                {
                    child.gameObject.SetActive(true);


                    if(child.tag == "Bacon")
                    {
                        bacon = true;
                    }

                    if (child.tag == "Salami")
                    {
                        salami = true;
                    }

                    if (child.tag == "Onion")
                    {
                        onions = true;
                    }

                    if (child.tag == "Tomato")
                    {
                        child.GetComponent<MeshRenderer>().material = mat_tomato;
                    }
                    else
                    {
                        Destroy(other.gameObject);
                    }

                }
            }


        }
    }
}
