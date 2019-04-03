//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========
//포즈 이동량, 회전량을 안정화 하는 코드입니다

using HTC.UnityPlugin.Utility;
using UnityEngine;

namespace HTC.UnityPlugin.PoseTracker
{
    public class PoseStablizer : BasePoseModifier
    {
        public float positionThreshold = 0.0005f; // meter
        public float rotationThreshold = 0.5f; // degree

        private bool firstPose = true;
        private RigidPose prevPose;

        protected override void OnEnable()
        {
            base.OnEnable();
            ResetFirstPose();
        }

        //포즈가 변경될 때 호출
        public override void ModifyPose(ref RigidPose pose, Transform origin)
        {
            //첫 번째 포즈이면 이전포즈가 없음
            if (firstPose)
            {
                firstPose = false;
            }
            else
            {
                //포즈 변환 벡터3
                Vector3 posDiff = prevPose.pos - pose.pos;
                //포즈 변환 벡터의 제곱한 값이 Threshold의 제곱보다 크면 즉 정해준 값 보다 많이 움직였으면 정해진 만큼만 이동
                if (positionThreshold > 0f || posDiff.sqrMagnitude > positionThreshold * positionThreshold)
                {
                    //최대값이 MaxLength (positionThreshold)로 고정된 Vector값을 현재의 Pose에 더한다.
                    pose.pos = pose.pos + Vector3.ClampMagnitude(posDiff, positionThreshold);
                }
                else
                {
                    //만약 Threshold만큼 이동하지 않았다면 그냥 움직이지 않는다.
                    pose.pos = prevPose.pos;
                }

                //회전량이 rotationThreshold 보다 크면
                if (rotationThreshold > 0f || Quaternion.Angle(pose.rot, prevPose.rot) > rotationThreshold)
                {
                    //이전방향에서 현재방향으로 Threshold 만큼만 회전
                    pose.rot = Quaternion.RotateTowards(pose.rot, prevPose.rot, rotationThreshold);
                }
                else
                {
                    //아니면 예전방향 사용
                    pose.rot = prevPose.rot;
                }
            }

            prevPose = pose;
        }

        public void ResetFirstPose() { firstPose = true; }
    }
}