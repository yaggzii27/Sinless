using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform referenceTransform;
    public float collisionOffset = 0.2f; //To prevent Camera from clipping through Objects

    Vector3 defaultPos;
    Vector3 directionNormalized;
    Transform parentTransform;
    float defaultDistance;

    float rotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    public Transform Obstruction;
    float zoomSpeed = 2f;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);

    }

    // FixedUpdate for physics calculations
    void LateUpdate()
    {
        CamControl();
        ViewObstructed();
    }
    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        transform.LookAt(Target);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {

            Player.rotation = Quaternion.Euler(0, mouseX, 0);

            //Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, Quaternion.Euler(0, mouseX, 0), 50 * Time.deltaTime);

            //Quaternion.Lerp(Quaternion.Euler(Player.forward.x, Player.forward.y, Player.forward.z), Quaternion.Euler(0, mouseX, 0), 1f);

            //Mathf.Lerp(transform.rotation.x, mouseX, .5f);
        }          
        
    }
    void ViewObstructed()
    {
        if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Vector3 currentPos = defaultPos;
        RaycastHit hit;
        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance))
        {
            if(hit.collider.gameObject.name == "Terrain")
            currentPos = (directionNormalized * (hit.distance - collisionOffset));
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 15f);
    }
}