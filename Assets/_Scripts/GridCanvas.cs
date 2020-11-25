using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class GridCanvas : MonoBehaviour
{
    [SerializeField] GameObject gridTileSprite;
    [SerializeField] Color gridColor = Color.black;
    [SerializeField] string buildableTileComponentName = "PlaceableTile";

    [Header("Grid Units")]
    [SerializeField] float unitsPerGrid = 1f;
    [SerializeField] Vector2 startPosition = new Vector2(0,0);

    private float width = 0f;
    private float height = 0f;

    void Start()
    {
        var rect = GetComponent<RectTransform>();
        width = rect.rect.width;
        height = rect.rect.height;
        InitializeGrid();
    }

    public void ToggleGrid(){
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void InitializeGrid() {
        var x = startPosition.x;
        var y = startPosition.y;

        var maxX = (width / 2) - x;
        var maxY = (height / 2) - y;

        while( x <= maxX ) {
            y = startPosition.y;
            while( y <= maxY ) {

                var position = new Vector3(x,y,0);
                var isPlaceableTile = CheckForObjectType(buildableTileComponentName, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y), Vector3.down);

                y++;
                if( !isPlaceableTile) { continue; }
                var obj = Instantiate(gridTileSprite, position, Quaternion.identity);
                obj.transform.SetParent(transform, false);
                var sprite = obj.GetComponent<SpriteRenderer>();
                sprite.color = gridColor;
            }
            x++;
        }
    }

    void OnDrawGizmos() {
        // Gizmos.DrawSphere(new Vector3(-1, 0.2001f, 0), 0.02f);
        // Gizmos.DrawLine(new Vector3(-1, 0.2001f, 0), new Vector3(-1, -1, 0));
    }


    // Will search for a terrain-tagged game object with given component name using the given
    // position and direction.  Returns true if found.
    private bool CheckForObjectType(string componentName, Vector3 position, Vector3 direction) {
        RaycastHit hit;
        var didHit = Physics.Raycast(position, direction, out hit, 1, 1 << LayerMask.NameToLayer("Terrain"));
        if( didHit )
        {
            var objectType = hit.transform.gameObject.GetComponent(componentName);
            if( objectType != null ) {
                return true;
            }
        }

        return false;
    }
}
