using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour
{
    public float speed = 50.0f;
    public GameObject selectionRing;

    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            selectionRing.renderer.enabled = _isSelected;
        }
    }

    private const float CloseDist = 1.0f;
    private MouseInputHandler mouseInputHandler;
    private Vector3 targetPos;

    void Start()
    {
        mouseInputHandler = (MouseInputHandler)GameObject.FindObjectOfType( typeof( MouseInputHandler ) );
        mouseInputHandler.OnRightClick += new MouseClickHandler( mouseInputHandler_OnRightClick );
    }

    void mouseInputHandler_OnRightClick( Vector3 displacement )
    {
        if ( IsSelected ) {
            targetPos = transform.position + displacement;
            targetPos.y = transform.position.y;
        }
    }

    void Update()
    {
        if ( targetPos != Vector3.zero ) // TODO: more robust check
        {
            transform.LookAt( targetPos );
            transform.Translate( Vector3.forward * speed * Time.deltaTime );
            transform.position = new Vector3( transform.position.x,
                Terrain.activeTerrain.SampleHeight( transform.position ) + 1,
                transform.position.z );

            if ( Vector3.Distance( transform.position, targetPos ) <= CloseDist ) {
                targetPos = Vector3.zero;
            }
        }
    }
}
