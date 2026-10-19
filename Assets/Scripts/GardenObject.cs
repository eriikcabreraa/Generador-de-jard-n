using UnityEngine;

public class GardenObject : MonoBehaviour
{

    #region Properties
    public int Type => _type;
    #endregion
    #region Fields
    [SerializeField] private int _type;
    #endregion

 
}

