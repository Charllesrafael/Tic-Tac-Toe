using UnityEngine;

public class BlockBoard : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private ColorScriptable overColor;
    [SerializeField] private ColorScriptable idleColor;
    [SerializeField] internal BoxCollider boxCollider;
    [SerializeField] private GameObject[] prefab;

    private bool selected;


    private void Start() {
        mesh.material.color = idleColor.color;
    }
    
    private void OnMouseUp() {
        BoardController.ClickBlock(this);
    }

    private void OnMouseEnter() {
        mesh.material.color = overColor.color;
    }

    private void OnMouseExit() {
        mesh.material.color = idleColor.color;
    }
    
    internal void SetActivePrefab(int idPrefab){
        if(selected)
            return;

        mesh.material.color = overColor.color;
        prefab[idPrefab].SetActive(true);
        boxCollider.enabled = false;
        selected = true;
        mesh.material.color = idleColor.color;
    }

    internal void ResetBlock(){
        foreach (GameObject item in prefab)
            item.SetActive(false);


        prefab[0].SetActive(false);
        prefab[1].SetActive(false);
        selected = false;
        mesh.material.color = idleColor.color;

        //SetActive(true, true);
    }

    internal void SetActive(bool value, bool all = false){
        if(all || !selected){
            boxCollider.enabled = value;
            mesh.material.color = idleColor.color;
        }
    }
}
