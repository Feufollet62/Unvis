using UnityEngine;

public class Cutout : MonoBehaviour
{
    // Permet de faire disparaître les murs lorsque le joueur se trouve derrière
    
    [SerializeField] private Transform targetObject;
    [SerializeField] private Shader targetShader;

    [SerializeField] private float cutoutSize;
    [SerializeField] private float fallOffSize;

    private Material[] levelMats;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Level");
        levelMats = new Material[walls.Length];
        
        // On remplit la liste et on assigne les valeurs
        for (int i = 0; i < walls.Length; i++)
        {
            Renderer thisWall = walls[i].GetComponent<Renderer>();

            foreach (Material mat in thisWall.materials)
            {
                if (mat.shader == targetShader) levelMats[i] = mat;
            }
                
            levelMats[i].SetFloat("_CutoutSize", cutoutSize);
            levelMats[i].SetFloat("_FalloffSize", fallOffSize);
        }
    }

    private void Update()
    {
        // On calcule la position du joueur sur l'écran
        Vector2 cutoutPos = cam.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= UnityEngine.Screen.width / UnityEngine.Screen.height;

        // On transmet la valeur aux matériaux
        foreach (var mat in levelMats)
        {
            mat.SetVector("_CutoutPosition", cutoutPos);
        }
    }
}