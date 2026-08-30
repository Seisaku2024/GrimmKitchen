using Arbor;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementCameraController : MonoBehaviour
{
    // カメラ角度を変える
    // ほぼAimCameraControllerと同じ（山本）


    private ICameraInputProvider m_cameraInputProvider = null;
    [SerializeField]
    private CinemachineVirtualCamera m_useVirtualCamera = null;
    [SerializeField]
    private AxisState m_vertical;
    public AxisState Vertical { get { return m_vertical; } }
    [SerializeField]
    private AxisState m_horizontal;
    public AxisState Horizontal { get { return m_horizontal; } }

    [SerializeField]
    private float m_maxDist = 4.5f;
    private float m_setCameraQuartanionX = 0.0f;


    void Awake()
    {
        m_cameraInputProvider = new CameraInputProvider();
        m_setCameraQuartanionX =  m_useVirtualCamera.transform.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraInput = m_cameraInputProvider.CameraXY;
        Vector2 inputVec = new(
            cameraInput.x * -1.0f,// リバースしているので-1をかける
            cameraInput.y);
       
        m_vertical.m_InputAxisValue = inputVec.y;
        m_horizontal.m_InputAxisValue = inputVec.x;

       

        m_vertical.Update(Time.deltaTime);
        m_horizontal.Update(Time.deltaTime);


        if(m_horizontal.Value>=m_horizontal.m_MaxValue)
        {
            m_horizontal.Value = 0.0f;
        }
        else if(m_horizontal.Value<=m_horizontal.m_MinValue)
        {
            m_horizontal.Value=0.0f;
        }


       
        var horizontalRotation = Quaternion.AngleAxis(m_horizontal.Value, Vector3.up);
        var verticalRotation = Quaternion.identity;
        verticalRotation.x = m_setCameraQuartanionX;
        

        ////　指定角度以下なら動かしてない判定
        //if (Mathf.Abs(m_horizontal.Value) < 5.0f)
        //{
        //    horizontalRotation.y = 0.0f;

        //}

        var somTrans = horizontalRotation * verticalRotation;
       
        m_useVirtualCamera.transform.rotation = somTrans;
        

        // トランスポーサーのオフセット値を調整
        var transposer = m_useVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {

            // 現在の角度を取得
            Vector3 currentAngles = new Vector3();
            currentAngles.y = m_horizontal.m_MaxValue;

            transposer.m_FollowOffset.x
                = -m_maxDist * (m_useVirtualCamera.transform.rotation.y / Quaternion.Euler(currentAngles).y);

        }


    }

    private void OnDisable()
    {
        //m_verticalTarget.localRotation = Quaternion.identity;
        //m_horizontalTarget.rotation = Quaternion.identity;
        //m_vertical.Value = 0;
        //m_horizontal.Value = 0;
    }

    private void OnEnable()
    {
        //Vector3 angle = Camera.main.transform.localRotation.eulerAngles;
        //m_horizontalTarget.eulerAngles = new Vector3(0, angle.y, 0);

        //Vector3 horizonVec = m_horizontalTarget.rotation.eulerAngles;
        //m_horizontal.Value = horizonVec.y;
    }
}
