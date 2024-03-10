using UnityEngine;

public class DetectHoveredObject : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] LayerMask _selectedLayer;
    [SerializeField] float _hoverTimeThreshold = 0.5f;

    float _hoverTimer;
    
    SelectedObject _currentSelectedObject;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {
        // Check if the mouse is hovering over an object in the _selectedLayer.
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        bool isHoveringOverObject = Physics.Raycast(ray, out hit, Mathf.Infinity, _selectedLayer);

        // If the mouse is hovering over an object, process the hovered object.
        if (isHoveringOverObject)
        {
            _hoverTimer += Time.deltaTime;
            
            if (_hoverTimer >= _hoverTimeThreshold)
            {
                _hoverTimer = 0;
                HandleHoveredObject(hit.transform.gameObject);
            }
        }
        // If the mouse is not hovering over an object, hide the previously hovered object.
        else
        {
            HidePreviouslySelectedObject();
        }
    }
    void HandleHoveredObject(GameObject hoveredChildObject)
    {
        Transform parentObject = hoveredChildObject.transform.parent;

        if (parentObject != null)
        {
            SelectedObject selectedObjectComponent = parentObject.GetComponentInChildren<SelectedObject>(true);

            if (selectedObjectComponent != null)
            {
                UpdateSelectedObject(selectedObjectComponent, hoveredChildObject);
            }
            else
            {
                Debug.LogError("SelectedObject component not found in the children of the parent object");
            }
        }
        else
        {
            Debug.LogError("No parent object found");
        }
    }
    void UpdateSelectedObject(SelectedObject selectedObjectComponent, GameObject hoveredChildObject)
    {
        if (_currentSelectedObject != selectedObjectComponent)
        {
            // Hide the previously selected object
            if (_currentSelectedObject != null)
            {
                _currentSelectedObject.Hide();
            }
            
            _hoverTimer = 0;

            // Show the new selected object
            _currentSelectedObject = selectedObjectComponent;
            _currentSelectedObject.Show();
            Debug.Log("Mouse is hovering over: " + hoveredChildObject.name);
        }
    }
    void HidePreviouslySelectedObject()
    {
        if (_currentSelectedObject != null)
        {
            _currentSelectedObject.Hide();
            _currentSelectedObject = null;
        }
    }
}
