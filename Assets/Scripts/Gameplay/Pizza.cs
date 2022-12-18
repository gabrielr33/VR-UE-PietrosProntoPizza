using System.Collections;
using System.Collections.Generic;
using Networking;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : NetworkObject
    {
        [field: SerializeField] public List<PizzaIngredient> Ingredients { get; set; }
        public bool CanBePickedUp { get; set; }
        public BakingStageEnum BakingStage  { get; private set; }

        [SerializeField] private Transform _pizzaDough;
        [SerializeField] private List<Material> _pizzaDoughMaterials;
        [SerializeField] private Ingredient _tomatoSauce;

        private int _bakingCounter = 0;
        private GameManager _gameManager;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Start()
        {            
            Ingredients = new List<PizzaIngredient>();
            CanBePickedUp = true;
            SetPizzaBakingStage(_bakingCounter);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided: " + other.name);
            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null || ingredient.IsContainer)
                return;

            if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                if (ingredient.IngredientType.Equals(PizzaIngredient.TomatoSauce) && other.GetComponent<TomatoSauceFillingManager>().IsSpoonFilled)
                {
                        _tomatoSauce.gameObject.SetActive(true);
                        other.GetComponent<TomatoSauceFillingManager>().EmptySpoon();
                }
                
                photonView.RPC("EnableIngredientForOthers", RpcTarget.All, (int)ingredient.IngredientType);
                
                if (!ingredient.IngredientType.Equals(PizzaIngredient.TomatoSauce) && PhotonNetwork.LocalPlayer.Equals(ingredient.GetComponent<PhotonView>().Owner))
                    PhotonNetwork.Destroy(ingredient.gameObject);
            }
        }

        [PunRPC]
        private void EnableIngredientForOthers(int ingredientType)
        {
            Ingredients.Add((PizzaIngredient)ingredientType);
            
            foreach(Ingredient child in transform.GetComponentsInChildren<Ingredient>(true))
            {
                if(child.IngredientType.Equals((PizzaIngredient)ingredientType))
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public void StartPizzaBakingProcess()
        {
            StartCoroutine(PizzaBakingProcess());
        }

        private IEnumerator PizzaBakingProcess()
        {
            Debug.Log("Pizza inserted in oven! Start baking!");
            CanBePickedUp = true;
            
            yield return new WaitForSeconds(_gameManager.GameValues.PizzaDoneTime);

            _bakingCounter++;
            SetPizzaBakingStage(_bakingCounter);

            yield return new WaitForSeconds(_gameManager.GameValues.PizzaBurnTime);

            _bakingCounter++;
            SetPizzaBakingStage(_bakingCounter);

            // pizza.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void SetPizzaBakingStage(int bakingCounter)
        {
            if (bakingCounter > 2)
                return;
            
            BakingStage = (BakingStageEnum)bakingCounter;
            
            switch (BakingStage)
            {
                case BakingStageEnum.Raw:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[0];
                    break;
                case BakingStageEnum.Done:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[1];
                    break;
                case BakingStageEnum.Burned:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[2];
                    break;
            }
        }

        public void StopBakingProcess()
        {
            photonView.RPC("StopPizzaBakingProcess", RpcTarget.All);
        }

        [PunRPC]
        private void StopPizzaBakingProcess()
        {
            StopAllCoroutines();
        }

        public enum BakingStageEnum
        {
            Raw = 0,
            Done,
            Burned
        }
    }
}
