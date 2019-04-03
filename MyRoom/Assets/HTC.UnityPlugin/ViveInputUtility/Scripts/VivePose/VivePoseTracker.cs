//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========
//역할(왼손, 오른손, 트래커)에 맞는 Pose Listener를 추가하는 코드

using HTC.UnityPlugin.PoseTracker;
using HTC.UnityPlugin.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HTC.UnityPlugin.Vive
{
    //메뉴안의 어느 위치든 생성한 스크립트가 위치할 수 있도록 해줌
    [AddComponentMenu("HTC/VIU/Device Tracker/Vive Pose Tracker (Transform)", 7)]

    // Simple component to track Vive devices.
    public class VivePoseTracker : BasePoseTracker, INewPoseListener, IViveRoleComponent
    {
        [Serializable]
        public class UnityEventBool : UnityEvent<bool> { }

        private bool m_isValid;

        public Transform origin;


        //현재 Role 담는 변수
        [SerializeField]
        private ViveRoleProperty m_viveRole = ViveRoleProperty.New(HandRole.RightHand);

        //값이 바뀔 때 이벤트 처리
        public UnityEventBool onIsValidChanged;

        //변수를 인스펙터 뷰에서 보이지 않도록 설정
        [HideInInspector]
        [Obsolete("Use VivePoseTracker.viveRole instead")]
        public DeviceRole role = DeviceRole.Invalid;

        //Role 변수
        public ViveRoleProperty viveRole { get { return m_viveRole; } }

        public bool isPoseValid { get { return m_isValid; } }

        protected void SetIsValid(bool value, bool forceSet = false)
        {
            if (ChangeProp.Set(ref m_isValid, value) || forceSet)
            {
                if (onIsValidChanged != null)
                {
                    onIsValidChanged.Invoke(value);
                }
            }
        }

        //Update 함수가 호출되기 전에 한 번만 호출, 다른 스크립트의 Awake가 모두 실행 된 후 실행
        protected virtual void Start()
        {
            //Role(right hand, left hand)를 Valid로 변경
            SetIsValid(VivePose.IsValid(m_viveRole), true);
        }
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            // change old DeviceRole value to viveRole value
            var serializedObject = new UnityEditor.SerializedObject(this);

            var roleValueProp = serializedObject.FindProperty("role");
            var oldRoleValue = roleValueProp.intValue;

            if (oldRoleValue != (int)DeviceRole.Invalid)
            {
                Type newRoleType;
                int newRoleValue;

                if (oldRoleValue == -1)
                {
                    newRoleType = typeof(DeviceRole);
                    newRoleValue = (int)DeviceRole.Hmd;
                }
                else
                {
                    newRoleType = typeof(HandRole);
                    newRoleValue = oldRoleValue;
                }

                if (Application.isPlaying)
                {
                    roleValueProp.intValue = (int)DeviceRole.Invalid;
                    m_viveRole.Set(newRoleType, newRoleValue);
                }
                else
                {
                    roleValueProp.intValue = (int)DeviceRole.Invalid;
                    serializedObject.ApplyModifiedProperties();
                    m_viveRole.Set(newRoleType, newRoleValue);
                    serializedObject.Update();
                }
            }
            serializedObject.Dispose();
        }
#endif
        //인스펙터뷰에서 체크를 통해 게임오브젝트 활성화 될 때 호출
        protected virtual void OnEnable()
        {
            //현재 인스턴스가 Tracker의 Pose를 Listen 할 수 있게 함
            VivePose.AddNewPosesListener(this);
        }

        //게임 오브젝트 또는 스크립트가 비 활성화 되었을 때 호출
        protected virtual void OnDisable()
        {
            //현재 인스턴스가 Tracker의 Pose를 Listen 할 수 없게 함
            VivePose.RemoveNewPosesListener(this);

            // Pose Tracker를 InValid 하게 변경
            SetIsValid(false);
        }

        public virtual void BeforeNewPoses() { }

        public virtual void OnNewPoses()
        {
            var deviceIndex = m_viveRole.GetDeviceIndex();
            var isValid = VivePose.IsValid(deviceIndex);

            if (isValid)
            {
                TrackPose(VivePose.GetPose(deviceIndex), origin);
            }

            SetIsValid(isValid);
        }

        public virtual void AfterNewPoses() { }
    }
}