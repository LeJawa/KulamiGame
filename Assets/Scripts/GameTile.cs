using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    private UnitTile _unitTileReference;

    internal void Initialize(UnitTile tile)
    {
        _unitTileReference = tile;
    }

    public void OnMouseUpAsButton()
    {
        GameManager.Instance.GameTileClickedEvent(_unitTileReference);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
