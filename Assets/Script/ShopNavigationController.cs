
using UnityEngine;

public class ShopNavigationController : MonoBehaviour
{
    [Header("Shop Containers")]
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private GameObject shipContainer;

    private void Start()
    {
        ShowItemContainer();
    }

    public void ShowShipContainer()
    {
        itemContainer.SetActive(false);
        shipContainer.SetActive(true);
    }

    public void ShowItemContainer()
    {
        shipContainer.SetActive(false);
        itemContainer.SetActive(true);
    }
}
