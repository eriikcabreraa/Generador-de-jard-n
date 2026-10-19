using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GardenGenerator : MonoBehaviour
{
    #region Properties
    #endregion

    #region Fields
    [SerializeField] private GameObject[] _gardenPrefabs;
    [SerializeField] private int _gardenCount = 10;
    private List<GameObject> _gardenList = new List<GameObject>();
    #endregion

    #region Unity Callbacks
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/gardenData.json"))
        {
            Load();
        }
        else
        {
            Generate();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Save();
        }
    }
    //private void OnDestroy()
    //{
    //    Save();
    //}
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Generate()
    {
        for (int i = 0; i < _gardenCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10, 10), 0.25f, Random.Range(-10, 10));

            GameObject gardenPrefab = _gardenPrefabs[Random.Range(0, _gardenPrefabs.Length)];
            GameObject gardenInstance = Instantiate(gardenPrefab, position, Quaternion.identity);
            _gardenList.Add(gardenInstance);
        }
    }
    private void Save()
    {
        SaveData saveData = new SaveData();
        
        foreach (GameObject garden in _gardenList)
        {
            if (garden == null)
            {
                continue;
            }
    
            GardenObject gardenObject = garden.GetComponent<GardenObject>();
            saveData.ObjectType.Add(gardenObject.Type);
            saveData.ObjectPosition.Add(garden.transform.position);
            saveData.ObjectScale.Add(garden.transform.localScale);
        }
        
        string json = JsonUtility.ToJson(saveData, true);
        Debug.Log(json);
        string path = Application.persistentDataPath + "/gardenData.json";
        File.WriteAllText(path, json);
        Debug.Log("Jardín guardado en: " + path);
    }

    private void Load()
    {
        string path = Application.persistentDataPath + "/gardenData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log(json);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            for (int i = 0; i < saveData.ObjectType.Count; i++)
            {
                int type = saveData.ObjectType[i];
                Vector3 position = saveData.ObjectPosition[i];
                Vector3 scale = saveData.ObjectScale[i];

                GameObject prefab = _gardenPrefabs[type];
                GameObject garden = Instantiate(prefab, position, Quaternion.identity);

                garden.transform.localScale = scale;
                _gardenList.Add(garden);
            }
        }
    }
    #endregion
}
public class SaveData
{
    public List<int> ObjectType = new List<int>();
    public List<Vector3> ObjectPosition = new List<Vector3>();
    public List<Vector3> ObjectScale = new List<Vector3>();

}
