//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========
//캔버스 UI가 빔 레이캐스트에 충돌되는 처리를 하는 스크립트

using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{
    public interface ICanvasRaycastTarget
    {
        Canvas canvas { get; }
        bool enabled { get; }
        bool ignoreReversedGraphics { get; }
    }

    [AddComponentMenu("HTC/VIU/UI Pointer/Canvas Raycast Target", 6)]
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class CanvasRaycastTarget : UIBehaviour, ICanvasRaycastTarget
    {
        private Canvas m_canvas;

        //뒤집어 진거 신경 안씀
        [SerializeField]
        private bool m_IgnoreReversedGraphics = true;

        //캔버스 받아옴
        public virtual Canvas canvas { get { return m_canvas ?? (m_canvas = GetComponent<Canvas>()); } }

        public bool ignoreReversedGraphics { get { return m_IgnoreReversedGraphics; } set { m_IgnoreReversedGraphics = value; } }


        //타겟 처리
        protected override void OnEnable()
        {
            base.OnEnable();
            CanvasRaycastMethod.AddTarget(this);
        }


        //타겟 해제
        protected override void OnDisable()
        {
            base.OnDisable();
            CanvasRaycastMethod.RemoveTarget(this);
        }
    }
}