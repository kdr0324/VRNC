using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponAnimator : MonoBehaviour, IInitializable {

    public void Initialize(GameObject character) {
        if (m_animator == null) {
            m_animator = character.GetComponent<Animator>();
        }
    }

    public enum AimMode {
        Free,
        Horizontal
    }

    [SerializeField] private AimMode m_mode = AimMode.Free;
    [SerializeField] private Animator m_animator;

    private int m_aimAnimLayerNum;
    private int m_weaponFireAnimLayerNum;
    private int m_gunType = -1;
    public int GunType { get { return m_gunType; } }

    private Vector3 m_aimDirection;
    private float m_verticalAimAngle;

    private bool m_currentShoot = false;
    private bool m_previousShoot = false;

    private bool m_currentReload = false;
    private bool m_previousReload = false;

    private bool m_gunInHand = false;

    public Vector3 AimDirection { get { return m_aimDirection; } }
    public Animator Animator { get { return m_animator; } }

    public void SetAimDirection(Vector3 aimDirection) {
        m_aimDirection = aimDirection;
        switch(m_mode) {
            case AimMode.Horizontal: HorizontalAimUpdate(m_aimDirection); break;
            case AimMode.Free: FreeAimUpdate(m_aimDirection); break;
            default: Debug.LogError("Enum not supported", gameObject); break;
        }
        m_animator.SetFloat("AimAngle", m_verticalAimAngle);
    }

    public void Shoot() {
        m_currentShoot = true;
    }

    public void Reload() {
        m_currentReload = true;
    }

    private void Awake() {
        Initialize(gameObject);
        m_aimAnimLayerNum = m_animator.GetLayerIndex("AimOverride");
        m_weaponFireAnimLayerNum = m_animator.GetLayerIndex("WeaponFireAdditive");
    }

    public void SetGunInHand(bool value, int gunType) {
        m_gunInHand = value;
        m_gunType = gunType;

        if (m_gunInHand && m_gunType != -1) {
            m_animator.SetLayerWeight(m_aimAnimLayerNum, 1.0f);
            m_animator.SetLayerWeight(m_weaponFireAnimLayerNum, 1.0f);
            m_animator.SetInteger("GunType", gunType);
        } else {
            m_animator.SetLayerWeight(m_aimAnimLayerNum, 0.0f);
            m_animator.SetLayerWeight(m_weaponFireAnimLayerNum, 0.0f);
        }
    }

    private void Update() {
        switch(m_gunType) {
            case 0:
                if (m_currentShoot) { m_animator.SetTrigger("WeaponFire"); } break;
            case -1:
            case 1:
            case 2:
                if (m_currentShoot && !m_previousShoot) { m_animator.SetTrigger("WeaponFire"); } break;
        }

        if (m_currentReload && !m_previousReload) { m_animator.SetTrigger("WeaponReload"); }

        m_previousShoot = m_currentShoot;
        m_currentShoot = false;

        m_previousReload = m_currentReload;
        m_currentReload = false;
    }

    private void HorizontalAimUpdate(Vector3 aimDirection) {
        Vector3 aimdir = aimDirection;
        aimdir.y = 0.0f;

        Quaternion aimRotation = Quaternion.LookRotation(aimdir);
        transform.rotation = Quaternion.Euler(0.0f, aimRotation.eulerAngles.y, 0.0f);
        m_verticalAimAngle = 0.45f;
    }

    private void FreeAimUpdate(Vector3 aimDirection) {
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection);
        transform.rotation = Quaternion.Euler(0.0f, aimRotation.eulerAngles.y, 0.0f);

        m_verticalAimAngle = aimRotation.eulerAngles.x;
        if (m_verticalAimAngle > 90) { m_verticalAimAngle = (m_verticalAimAngle - 360); }
        m_verticalAimAngle = 1 - ((m_verticalAimAngle + 90) / 180);
    }
}
