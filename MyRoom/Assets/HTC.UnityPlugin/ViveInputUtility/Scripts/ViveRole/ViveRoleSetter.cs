//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========
// 현재 장비가 어떤 역할 (왼손, 오른손, 트래커)인지 결정하는 코드

using System.Collections.Generic;
using UnityEngine;

namespace HTC.UnityPlugin.Vive
{
    public interface IViveRoleComponent
    {
        ViveRoleProperty viveRole { get; }
    }

    /// <summary>
    /// This component sync its role to all child component that is an IViveRoleComponent
    /// </summary>
    public class ViveRoleSetter : MonoBehaviour
    {
        private static List<IViveRoleComponent> s_comps = new List<IViveRoleComponent>();


        
        [SerializeField]
        private ViveRoleProperty m_viveRole = ViveRoleProperty.New(HandRole.RightHand);

        //HandRole Role(LeftHand, RightHand, Controller) 어떤건지 표시하는 변수
        public ViveRoleProperty viveRole { get { return m_viveRole; } }
#if UNITY_EDITOR
        //오브젝트를 생성 후 인스펙터 뷰에서 리셋을 눌러줄 때 실행
        //객체의 속성값을 초기 값으로 설정해 줄 때 사용
        protected virtual void Reset()
        {
            // get role from first found component
            var comp = GetComponentInChildren<IViveRoleComponent>();
            if (comp != null)
            {
                m_viveRole.Set(comp.viveRole);
            }
        }

        protected virtual void OnValidate()
        {
            UpdateChildrenViveRole();
        }
#endif
        //스크립트가 실행될 때 호출, 모든 오브젝트가 초기화 된 후 호출된다 
        protected virtual void Awake()
        {
            m_viveRole.onRoleChanged += UpdateChildrenViveRole;
        }

        //자식 객체에 Role 셋팅
        public void UpdateChildrenViveRole()
        {
            GetComponentsInChildren(true, s_comps);
            for (int i = 0; i < s_comps.Count; ++i)
            {
                s_comps[i].viveRole.Set(m_viveRole);
            }
            s_comps.Clear();
        }

        //오브젝트 생존기간의 마지막 프레임이 업데이트 된 후 실행
        protected virtual void OnDestroy()
        {
            m_viveRole.onRoleChanged -= UpdateChildrenViveRole;
        }
    }
}