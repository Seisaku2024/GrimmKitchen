using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInstanceStaff : MonoBehaviour
{
    // 制作者 田内
    // スタッフのインスタンスを作成する

    //==================================================
    //                  実行処理
    //==================================================

    void Start()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        // ポイントデータを基にスタッフを作成
        foreach (var data in StaffManager.instance.StaffPointDataList)
        {
            // 必要情報が無ければ
            if (data == null || data.StaffStatusData == null) continue;

            // スタッフのデータを取得
            var staffData = StaffMemberDataBaseManager.instance.GetStaffMemberData(data.StaffStatusData.StaffID);
            if (staffData == null) continue;

            // スタッフを作成する
            GameObject createStaff = Instantiate(staffData.StaffPrefab, data.SetPoint.transform.position, data.SetPoint.transform.rotation);

            // スタッフタイプをセット
            if (createStaff.TryGetComponent<StaffData>(out var staff))
            {
                staff.StaffStatusData = data.StaffStatusData;
                staff.StaffStatusData.StaffType = data.SetStaffType;
            }
            else
            {
                Debug.LogError("StaffDataコンポーネントがアタッチされていません");
            }

            // モーターを更新
            if (createStaff.TryGetComponent<MyCharacterController>(out var core))
            {
                core.SetPositionMotor(createStaff.transform.position);
            }
            else
            {
                Debug.LogError("MyCharacterControllerがアタッチされていません");
                return;
            }
        }
    }

}
