using FoodInfo;
using IngredientInfo;
using ItemInfo;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StorageManagerWindow : EditorWindow
{
    private StorageManager storageManager;

    private bool m_detailOpen = false;

    private readonly int typeWidth = 250;
    private readonly int numWidth = 150;

    // 単品アイテムの設定フィールド
    private ItemTypeID selectedItemTypeID = ItemTypeID.Food;
    private IngredientID selectedIngredientID = IngredientID.SleepApple;
    private FoodID selectedFoodID = FoodID.Omelette;
    private int selectedNum = 1;

    [MenuItem("PROJECT_C/Storage Manager")]
    public static void ShowWindow()
    {
        // ウィンドウを開く
        GetWindow<StorageManagerWindow>("Storage Manager");
    }

    private void OnGUI()
    {
        // StorageManagerの参照を取得
        if (storageManager == null)
        {
            storageManager = FindObjectOfType<StorageManager>();
        }

        if (storageManager == null)
        {
            EditorGUILayout.HelpBox("StorageManagerがシーンに見つかりません", MessageType.Warning);
            return;
        }

        GUI_ShowTypeStock();

        GUI_ShowDetailStockList();

        GUI_AddItem();

        // ウィンドウの更新ボタン
        if (GUILayout.Button("更新"))
        {
            Repaint(); // ウィンドウを再描画して最新情報を表示
        }
    }

    private void GUI_AddItem()
    {
        EditorGUILayout.LabelField("単品アイテムの追加", EditorStyles.boldLabel);

        // アイテム設定フィールド
        selectedItemTypeID = (ItemTypeID)EditorGUILayout.EnumPopup("アイテム種類", selectedItemTypeID);

        if (selectedItemTypeID == ItemTypeID.Ingredient)
        {
            selectedIngredientID = (IngredientID)EditorGUILayout.EnumPopup("素材ID", selectedIngredientID);
        }
        else if (selectedItemTypeID == ItemTypeID.Food)
        {
            selectedFoodID = (FoodID)EditorGUILayout.EnumPopup("料理ID", selectedFoodID);
        }

        selectedNum = EditorGUILayout.IntSlider("数量", selectedNum, 1, 99);

        if (GUILayout.Button("アイテムを追加"))
        {
            AddSingleItemToStorage();
            Repaint();
        }
    }

    private void GUI_ShowDetailStockList()
    {
        m_detailOpen = EditorGUILayout.BeginFoldoutHeaderGroup(m_detailOpen, "詳細");

        if (m_detailOpen)
        {
            int i = 0;
            foreach (var item in storageManager.ItemDataRC)
            {
                EditorGUILayout.BeginHorizontal();

                switch (item.ItemTypeID)
                {
                    case ItemTypeID.Ingredient:
                        IngredientID name = (IngredientID)item.ItemID;
                        EditorGUILayout.LabelField("[" + i.ToString() + "] アイテム種別: " + name, GUILayout.Width(typeWidth));

                        break;
                    case ItemTypeID.Food:
                        FoodID foodName = (FoodID)item.ItemID;
                        EditorGUILayout.LabelField("アイテム種別: " + foodName, GUILayout.Width(typeWidth));
                        break;
                }
                EditorGUILayout.LabelField("所持数: " + item.Num.ToString(), GUILayout.Width(numWidth));
                EditorGUILayout.EndHorizontal();
                ++i;
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void GUI_ShowTypeStock()
    {
        // 現在の在庫を表示
        EditorGUILayout.LabelField("現在の在庫", EditorStyles.boldLabel);

        Dictionary<FoodID, int> foodDataList = new();
        Dictionary<IngredientID, int> ingredientDataList = new();
        if (storageManager.ItemDataRC.Count == 0)
        {
            EditorGUILayout.LabelField("無し");
        }
        foreach (var item in storageManager.ItemDataRC)
        {
            switch (item.ItemTypeID)
            {
                case ItemTypeID.Ingredient:
                    IngredientID ingredientID = (IngredientID)item.ItemID;
                    if (ingredientDataList.ContainsKey(ingredientID))
                    {
                        ingredientDataList[ingredientID] += item.Num;
                    }
                    else
                    {
                        ingredientDataList.Add(ingredientID, item.Num);
                    }
                    break;
                case ItemTypeID.Food:
                    FoodID foodID = (FoodID)item.ItemID;
                    if (foodDataList.ContainsKey(foodID))
                    {
                        foodDataList[foodID] += item.Num;
                    }
                    else
                    {
                        foodDataList.Add(foodID, item.Num);
                    }
                    break;
            }
        }
        foreach (var foodItem in foodDataList)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("アイテム種別: " + foodItem.Key, GUILayout.Width(typeWidth));
            EditorGUILayout.LabelField("所持数: " + foodItem.Value, GUILayout.Width(numWidth));
            EditorGUILayout.EndHorizontal();
        }
        foreach (var ingredientItem in ingredientDataList)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("アイテム種別: " + ingredientItem.Key, GUILayout.Width(typeWidth));
            EditorGUILayout.LabelField("所持数: " + ingredientItem.Value, GUILayout.Width(numWidth));
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddSingleItemToStorage()
    {
        uint itemID = 0;

        // 選択されたアイテムIDを取得
        if (selectedItemTypeID == ItemTypeID.Ingredient)
        {
            itemID = (uint)selectedIngredientID;
        }
        else if (selectedItemTypeID == ItemTypeID.Food)
        {
            itemID = (uint)selectedFoodID;
        }
        else
        {
            Debug.LogWarning("不明なアイテム種類です。アイテムは追加されませんでした。");
            return;
        }

        // アイテムを追加
        for (int i = 0; i < selectedNum; i++)
        {
            storageManager.AddItem(selectedItemTypeID, itemID);
        }

        Debug.Log($"アイテムが追加されました: 種類: {selectedItemTypeID}, ID: {itemID}, 数量: {selectedNum}");
    }
}
