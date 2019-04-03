//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========
//빔 가이드라인을 그리기 위해 라인 렌더러의 점을 설정하는 스크립트

using HTC.UnityPlugin.Pointer3D;
using UnityEngine;

public class GuideLineDrawer : MonoBehaviour
{
    public const float MIN_SEGMENT_LENGTH = 0.01f;

    public Vector3 gravityDirection = Vector3.down;

    //빔이 Hit되었을 경우에만 그려주는 bool type 변수
    public bool showOnHitOnly;
    public float segmentLength = 0.05f;


    public Pointer3DRaycaster raycaster;
    public LineRenderer lineRenderer;

#if UNITY_EDITOR
    //객체의 속성을 초기값으로 설정해 줄 때 호출됨
    protected virtual void Reset()
    {
        //부모 객체로 감, 가장 최상위 부모객체가 가진 3D Raycaster(Script)를 받음
        for (var tr = transform; raycaster == null && tr != null; tr = tr.parent)
        {
            raycaster = tr.GetComponentInChildren<Pointer3DRaycaster>();
        }

        if (lineRenderer == null) { lineRenderer = GetComponentInChildren<LineRenderer>(); }
        if (lineRenderer == null && raycaster != null) { lineRenderer = gameObject.AddComponent<LineRenderer>(); }
        if (lineRenderer != null)
        {
            //빔 Width 설정
#if UNITY_5_5_OR_NEWER
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0f;
#else
            lineRenderer.SetWidth(0.01f, 0f);
#endif
        }
    }
#endif

    //모든 Update가 끝난 뒤 호출
    protected virtual void LateUpdate()
    {
        var result = raycaster.FirstRaycastResult();
        //맞았을때만 나오게 설정했고 맞지 않았다면 빔 렌더링 안 함
        if (showOnHitOnly && !result.isValid)
        {
            lineRenderer.enabled = false;
            return;
        }

        var points = raycaster.BreakPoints;
        var pointCount = points.Count;

        if (pointCount < 2)
        {
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;

        var startPoint = points[0];
        var endPoint = points[pointCount - 1];

        //시작과 끝이 있는 경우
        if (pointCount == 2)
        {
#if UNITY_5_6_OR_NEWER
            lineRenderer.positionCount = 2;
#elif UNITY_5_5_OR_NEWER
            lineRenderer.numPositions = 2;
#else
            lineRenderer.SetVertexCount(2);
#endif
            //라인 렌더러 시작, 끝 점 정해줌
            lineRenderer.SetPosition(0, transform.InverseTransformPoint(startPoint));
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPoint));
        }
        else
        {
            //아닌 경우의 라인렌더러 점 처리
            var systemY = gravityDirection;
            var systemX = endPoint - startPoint;
            var systemZ = default(Vector3);

            Vector3.OrthoNormalize(ref systemY, ref systemX, ref systemZ);

            var initialV = Vector3.ProjectOnPlane(points[1] - points[0], systemZ); // initial projectile direction
            var initialVx = Vector3.Dot(initialV, systemX);
            var initialVy = Vector3.Dot(initialV, systemY);
            var initSlope = initialVy / initialVx;

            var approachV = endPoint - startPoint;
            var approachVx = Vector3.Dot(approachV, systemX);
            var approachVy = Vector3.Dot(approachV, systemY);

            var lenx = Mathf.Max(segmentLength, MIN_SEGMENT_LENGTH);
            var segments = Mathf.Max(Mathf.CeilToInt(approachVx / lenx), 0);

            var factor = (approachVy - initSlope * approachVx) / (approachVx * approachVx);

#if UNITY_5_6_OR_NEWER
            lineRenderer.positionCount = segments + 1;
#elif UNITY_5_5_OR_NEWER
            lineRenderer.numPositions = segments + 1;
#else
            lineRenderer.SetVertexCount(segments + 1);
#endif
            lineRenderer.SetPosition(0, transform.InverseTransformPoint(startPoint));
            for (int i = 1, imax = segments; i < imax; ++i)
            {
                var x = i * lenx;
                var y = factor * x * x + initSlope * x;
                lineRenderer.SetPosition(i, transform.InverseTransformPoint(systemX * x + systemY * y + startPoint));
            }
            lineRenderer.SetPosition(segments, transform.InverseTransformPoint(endPoint));
        }
    }

    protected virtual void OnDisable()
    {
        lineRenderer.enabled = false;
    }
}
