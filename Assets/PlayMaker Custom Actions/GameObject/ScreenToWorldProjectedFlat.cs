using UnityEngine;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Camera)]
    public class ScreenToWorldProjectedFlat : FsmStateAction
    {
        public FsmVector2 screenPosition;
        public FsmFloat screenX;
        public FsmFloat screenY;

        public FsmFloat depth = 10f;

        public FsmBool normalized;

        [UIHint(UIHint.Variable)]
        public FsmVector3 storeWorldPosition;

        [UIHint(UIHint.Variable)]
        public FsmFloat storeX;

        [UIHint(UIHint.Variable)]
        public FsmFloat storeZ;

        public bool everyFrame;

        public override void Reset()
        {
            screenPosition = new FsmVector2 { UseVariable = true };
            screenX = new FsmFloat { UseVariable = true };
            screenY = new FsmFloat { UseVariable = true };
            depth = 10f;
            normalized = false;
            storeWorldPosition = null;
            storeX = null;
            storeZ = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            Convert();

            if (!everyFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            Convert();
        }

        void Convert()
        {
            if (Camera.main == null)
            {
                Finish();
                return;
            }

            Vector3 screenPos = Vector3.zero;

            if (!screenPosition.IsNone)
            {
                screenPos.x = screenPosition.Value.x;
                screenPos.y = screenPosition.Value.y;
            }

            if (!screenX.IsNone) screenPos.x = screenX.Value;
            if (!screenY.IsNone) screenPos.y = screenY.Value;

            if (normalized.Value)
            {
                screenPos.x *= Screen.width;
                screenPos.y *= Screen.height;
            }

            screenPos.z = depth.Value;

            // 월드 위치 계산
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            // 🧠 Y=0 평면에 투영: XZ 평면 유지, Y는 0
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            float enter;
            if (groundPlane.Raycast(ray, out enter))
            {
                worldPos = ray.GetPoint(enter); // 평면 위 교차점
            }

            // 저장
            if (!storeWorldPosition.IsNone) storeWorldPosition.Value = worldPos;
            if (!storeX.IsNone) storeX.Value = worldPos.x;
            if (!storeZ.IsNone) storeZ.Value = worldPos.z;
        }
    }
}
