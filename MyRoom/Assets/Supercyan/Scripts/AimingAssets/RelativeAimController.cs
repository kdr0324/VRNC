using UnityEngine;

public class RelativeAimController : MonoBehaviour, IInitializable {

    public void Initialize(GameObject character) {
        InitializeAnimator(character);
        InitializeAimPoint(character);
    }

    public enum InputButtonType {
        Mouse,
        Key,
        Button
    }

    [Header("Aiming")]
    [SerializeField] private float m_aimSpeed = 100;
    [SerializeField] private string m_horizontalAxis = "Mouse X";
    [SerializeField] private string m_verticalAxis = "Mouse Y";

    [Header("Shooting")]
    [SerializeField] private InputButtonType m_shootButtonType = InputButtonType.Mouse;
    [SerializeField] private int m_shootMouseButton = 0;
    [SerializeField] private string m_shootButton;
    [SerializeField] private KeyCode m_shootKey;

    [Header("Reloading")]
    [SerializeField] private InputButtonType m_reloadButtonType = InputButtonType.Key;
    [SerializeField] private int m_reloadMouseButton;
    [SerializeField] private string m_reloadButton;
    [SerializeField] private KeyCode m_reloadKey = KeyCode.R;

    private CharacterWeaponAnimator m_animator;
    private Transform m_aimPoint;

    private bool m_isDead;
    public bool IsDead {
        set {
            m_isDead = value;
            if (m_animator != null) {
                if (m_isDead) { m_animator.SetGunInHand(false, m_animator.GunType); }
                else { m_animator.SetGunInHand(true, m_animator.GunType); }
            }
        }
    }

    private void Awake() {
        Initialize(gameObject);
        m_animator.SetAimDirection(transform.forward);
    }

    private void LateUpdate() {
        if (!m_isDead) {
            ShootUpdate();
            ReloadUpdate();
            AimUpdate();
        }
    }

    private void ShootUpdate() {
        bool shoot = false;
        switch(m_shootButtonType) {
            case InputButtonType.Mouse: shoot = Input.GetMouseButton(m_shootMouseButton); break;
            case InputButtonType.Button: shoot = Input.GetButton(m_shootButton); break;
            case InputButtonType.Key: shoot = Input.GetKey(m_shootKey); break;
        }

        if(shoot) { m_animator.Shoot(); }
    }

    private void ReloadUpdate() {
        bool reload = false;
        switch(m_reloadButtonType) {
            case InputButtonType.Mouse: reload = Input.GetMouseButtonDown(m_reloadMouseButton); break;
            case InputButtonType.Button: reload = Input.GetButtonDown(m_reloadButton); break;
            case InputButtonType.Key: reload = Input.GetKeyDown(m_reloadKey); break;
        }

        if (reload) { m_animator.Reload(); }
    }

    private void AimUpdate() {
        float h = Input.GetAxis(m_horizontalAxis);
        float v = -Input.GetAxis(m_verticalAxis);

        Vector3 aimDirection = m_animator.AimDirection;

        aimDirection = Quaternion.AngleAxis(h * m_aimSpeed * Time.fixedDeltaTime, Vector3.up) * aimDirection;

        float currentVerticalDot = Vector3.Dot(aimDirection, Vector3.up);
        bool updateVerticalAim = true;
        if (currentVerticalDot > 0.95f && v < 0.0f) { updateVerticalAim = false; }
        if (currentVerticalDot < -0.95f && v > 0.0f) { updateVerticalAim = false; }

        if (updateVerticalAim) {
            aimDirection = Quaternion.AngleAxis(v * m_aimSpeed * Time.fixedDeltaTime, Vector3.Cross(Vector3.up, aimDirection)) * aimDirection;
        }

        m_animator.SetAimDirection(aimDirection);
    }

    private void InitializeAnimator(GameObject character) {
        if (m_animator != null) { return; }
        m_animator = character.GetComponent<CharacterWeaponAnimator>();
    }

    private void InitializeAimPoint(GameObject character) {
        if (m_aimPoint != null) { return; }

        AimPoint point = character.GetComponentInChildren<AimPoint>();
        if (!point) {
            GameObject newPoint = new GameObject("AimPoint");
            point = newPoint.AddComponent<AimPoint>();
            newPoint.transform.parent = character.transform;
            newPoint.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        }

        m_aimPoint = point.transform;
    }
}
