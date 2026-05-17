using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

// 敵のロックオンカメラ　伊波
// https://zenn.dev/aruk_vs/articles/ae723b82fd69ec
public class LockOnCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera lockonCameral;
    [SerializeField] private CapsuleCollider playerCol;
    [SerializeField, Range(0.0f, 100.0f)] private float lockonDist = 20;
    [SerializeField, Range(0.0f, 100.0f)] private float lockonCancelDist = 30;
    [Tooltip("ターゲットを長押しで切り替えるときの時間間隔")]
    [SerializeField, Range(0.0f, 1.0f)] private float targetChangeDelay = 0.5f;
    [Tooltip("ターゲットを切り替えるために必要な傾き")]
    [SerializeField, Range(0.0f, 1.0f)] private float targetChangeInputMagnitude = 0.5f;
    [Tooltip("カメラの注視点をどれだけずらすか おおきいほど横向きのカメラになりやすいはず")]
    [SerializeField, Range(0.0f, 10.0f)] private float adjustCameraTargetDist = 1.0f;

    private bool isLockon = false;
    private Camera mainCamera;
    private Rigidbody cameraTargetRigid;
    private CharacterCore targetCore;
    private float changeDelayCounter;

    void Start()
    {
        mainCamera = Camera.main;
        changeDelayCounter = 0.0f;
    }

    void Update()
    {
        // カメラがNUllになってしまっている時にセットする用（山本）
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }


        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "LockOn"))
        {
            if (isLockon)
            {
                isLockon = false;
                InactiveLockonCamera();
                return;
            }

            // ロックオン対象の検索、いるならロックオン
            if (!SetTargetData(GetLockonTarget(LockonSearchType.all))) return;
            ActiveLockonCamera(cameraTargetRigid.transform);
        }

        // ロックオンカーソル
        if (isLockon)
        {
            if (!targetCore)
            {
                InactiveLockonCamera();
                return;
            }

            // 敵と離れすぎたらロックオン解除
            Vector3 toTargetDist = targetCore.transform.position - playerCol.transform.position;
            if (lockonCancelDist <= toTargetDist.magnitude)
            {
                InactiveLockonCamera();
                return;
            }

            changeDelayCounter -= Time.deltaTime;

            // 左右入力値
            var cameraInput = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Camera, "CameraXY").ReadValue<Vector2>();

            // ターゲットが倒されたら次に切り替え
            if (targetCore.Status.m_hp.Value <= 0.0f)
            {
                InactiveLockonCamera();
                if (SetTargetData(GetLockonTarget(LockonSearchType.all)))
                {
                    ActiveLockonCamera(cameraTargetRigid.transform);
                }
                else
                {
                    return;
                }
            }
            // ターゲットの左右切り替え処理
            else if (changeDelayCounter <= 0.0f)
            {
                changeDelayCounter = 0.0f;
                if (cameraInput.magnitude != 0.0f)
                {
                    Transform newTarget = null;
                    if (cameraInput.x > targetChangeInputMagnitude) newTarget = GetLockonTarget(LockonSearchType.left);
                    if (cameraInput.x < -targetChangeInputMagnitude) newTarget = GetLockonTarget(LockonSearchType.right);
                    if (newTarget)
                    {
                        InactiveLockonCamera();
                        if (SetTargetData(newTarget))
                        {
                            ActiveLockonCamera(cameraTargetRigid.transform);
                        }
                    }
                }
            }
            else
            {
                if (cameraInput.magnitude < 0.05f)
                {
                    changeDelayCounter = 0.0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLockon)
        {
            // カメラのターゲットの座標をプレイヤーの奥にずらす
            Vector3 toEnemyVec = targetCore.transform.position - playerCol.transform.position;
            toEnemyVec.y = 0.0f;
            Vector3 toVec = targetCore.transform.position + toEnemyVec.normalized * adjustCameraTargetDist;
            toVec -= cameraTargetRigid.transform.position;
            //cameraTargetRigid.transform.transform.position = cursorTargetObj.transform.position;
            //cameraTargetRigid.velocity =
            //    toEnemyVec.normalized * adjustCameraTargetDist * (1 / Time.fixedDeltaTime);
            cameraTargetRigid.linearVelocity =
                toVec * (1 / Time.fixedDeltaTime * 0.5f);
        }
    }


    bool SetTargetData(Transform target)
    {
        //if (!target) return false;
        //if (!target.TryGetComponent(out cameraTargetRigid)) return false;
        //target.transform.localPosition = new Vector3(0, 0, 0);
        //if (!target.root.TryGetComponent(out targetCore)) return false;
        //return true;

        if (!target) return false;
        Transform cameraTarget = target.transform.GetChild(0);
        if (!cameraTarget) return false;
        if (!cameraTarget.TryGetComponent(out cameraTargetRigid)) return false;
        cameraTarget.transform.localPosition = new Vector3(0, 0, 0);
        if (!target.root.TryGetComponent(out targetCore)) return false;
        return true;
    }

    /// <summary>
    /// ロックオン対象の計算処理を行い取得する
    /// </summary>
    private CharacterMeta charaMeta = null;
    class SearchData
    {
        public GameObject obj;
        public float angle;
        public SearchData(GameObject obj, float angle)
        {
            this.obj = obj;
            this.angle = angle;
        }
    }

    enum LockonSearchType
    {
        all,
        right,
        left
    }

    Transform GetLockonTarget(LockonSearchType _searchType)
    {
        if (!charaMeta) charaMeta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
        List<SearchData> hitObjects = new();

        Vector3 targetScreenPos = Vector3.zero;
        Vector3 charaScreenPos;
        float angle;
        if (_searchType != LockonSearchType.all)
        {
            targetScreenPos = mainCamera.WorldToScreenPoint(targetCore.transform.root.position);
        }

        foreach (var chara in charaMeta.EnemyList)
        {
            if (chara.Status.m_hp.Value <= 0.0f) continue;

            Vector3 dir = chara.transform.position - playerCol.transform.position;
            if (dir.magnitude > lockonDist) continue;
            angle = Vector3.Angle(mainCamera.transform.forward, chara.transform.position - mainCamera.transform.position);

            switch (_searchType)
            {
                case LockonSearchType.all:
                    if (Mathf.Abs(angle) > mainCamera.fieldOfView * 0.5f) continue;
                    break;
                case LockonSearchType.right:
                    if (chara.transform == targetCore.transform.root) continue;
                    charaScreenPos = mainCamera.WorldToScreenPoint(chara.transform.position);
                    if (targetScreenPos.x > charaScreenPos.x) continue;
                    if (targetScreenPos.x >= Screen.width) continue;
                    break;
                case LockonSearchType.left:
                    if (chara.transform == targetCore.transform.root) continue;
                    charaScreenPos = mainCamera.WorldToScreenPoint(chara.transform.position);
                    if (targetScreenPos.x < charaScreenPos.x) continue;
                    if (targetScreenPos.x <= 0.0f) continue;
                    break;
            }

            if (!chara.TryGetComponent(out Collider targetCollider)) continue;

            dir = targetCollider.bounds.center - (mainCamera.transform.position);
            float dist = dir.magnitude;
            var ray = new Ray(mainCamera.transform.position, dir);
            if (!Physics.Raycast(ray, out RaycastHit hit, dist, LayerMask.NameToLayer("Terrain")))
            {
                //if (hit.collider.gameObject.name == targetCollider.gameObject.name)
                {
                    hitObjects.Add(new SearchData(chara.gameObject, Mathf.Abs(angle)));
                }
            }
        }

        var tumpleData = GetOptimalEnemy(hitObjects);

        //// 求めた一番小さい値が一定値より小さい場合、ターゲッティングをオンにします
        if (tumpleData)
        {
            if (tumpleData.TryGetComponent(out ShareNodes nodes))
            {
                return nodes.Nodes["LockOn"];
            }
        }
        return null;
    }

    // 2のリスト全てのベクトルとカメラのベクトルを比較し、画面中央に一番近いものを探す
    // degreep: カメラの前方ベクトルX,Z成分からなる角度
    private GameObject GetOptimalEnemy(List<SearchData> hitObjects)
    {
        float degreemum = mainCamera.fieldOfView * 2.0f;
        GameObject target = null;
        foreach (var enemy in hitObjects)
        {
            Vector3 pos = playerCol.transform.position - enemy.obj.transform.position;
            // pos.magnitude: 敵とカメラの距離
            // pos.magnitudeに応じて角度に重みをかけ、距離が近いほど角度の重みが大きく選好される
            float degree = enemy.angle + enemy.angle * (pos.magnitude / lockonDist);
            if (Mathf.Abs(degreemum) >= degree)
            {
                degreemum = degree;
                target = enemy.obj;
            }
        }
        return target;
    }

    private void ActiveLockonCamera(Transform cameraTarget)
    {
        isLockon = true;
        lockonCameral.Priority = 15;
        lockonCameral.LookAt = cameraTarget.transform;
        if (targetCore) targetCore.EnemyParameters.LockOnCursorCanvas.SetActive(true);
        changeDelayCounter = targetChangeDelay;
    }

    private void InactiveLockonCamera()
    {
        isLockon = false;
        lockonCameral.Priority = 0;
        lockonCameral.LookAt = null;
        if (targetCore) targetCore.EnemyParameters.LockOnCursorCanvas.SetActive(false);
        targetCore = null;
        changeDelayCounter = 0.0f;
    }
}