using UnityEngine;
using UnityEngine.UI;


public class UnitUI : MonoBehaviour
{
    [Header("UI패널")]
    public GameObject unitInfoPanel; // 유닛 정보 패널

    [Header("텍스트(정보 모음)")]
    public Text unitNameText; // 유닛 이름 텍스트
    public Text unitHealthText; // 유닛 체력 텍스트
    public Text unitMentalText; // 유닛 정신력 텍스트
    public Text unitBehaviorText; // 유닛 행동 텍스트
    public Text unitTargetText; // 유닛 타겟 텍스트

    public bool oneUnitSelected = false; // 단일 유닛 선택 여부
    public bool multipleUnitsSelected = false; // 다중 유닛 선택 여부

    private void Start()
    {
        // 유닛 정보 패널과 선택 박스를 비활성화
        unitInfoPanel.SetActive(false);
    }
    private void Update()
    {
        ShowUnitInfoPanel(); // 매 프레임마다 유닛 정보 패널 업데이트
    }

    public void ShowUnitInfoPanel()
    {
        // 선택된 유닛이 있는지 확인
        if (UnitSelectionManager.instance.selectedPlayers.Count == 0 && UnitSelectionManager.instance.selectedEnemy == null)
        {
            unitInfoPanel.SetActive(false);
            return;
        }
        // 단일 유닛 선택 여부 확인
        if (UnitSelectionManager.instance.selectedPlayers.Count == 1 && UnitSelectionManager.instance.selectedEnemy == null)
        {
            oneUnitSelected = true;
            multipleUnitsSelected = false;
            Player selectedPlayer = UnitSelectionManager.instance.selectedPlayers[0];
            unitNameText.text = selectedPlayer.unitName;
            unitHealthText.text = "Health: " + selectedPlayer.maxHp.ToString() + " / " + selectedPlayer.currentHp.ToString();
            unitMentalText.text = "Mental: " + selectedPlayer.maxMental.ToString() + " / " + selectedPlayer.currentMental.ToString();
            unitBehaviorText.text = "Behavior: " + selectedPlayer.behavior.ToString();
            unitTargetText.text = "Target: " + (selectedPlayer.target != null ? selectedPlayer.target.name : "None");
        }

        /*// 다중 유닛 선택 여부 확인
        if (UnitSelectionManager.instance.selectedPlayers.Count > 1 && UnitSelectionManager.instance.selectedEnemy == null)
        {
            oneUnitSelected = false;
            multipleUnitsSelected = true;
            unitNameText.text = "Multiple Units Selected";
            unitHealthText.text = "Total Health: " + UnitSelectionManager.instance.selectedPlayers.Count.ToString() + " units";
            unitMentalText.text = "Total Mental: " + UnitSelectionManager.instance.selectedPlayers.Count.ToString() + " units";
            unitBehaviorText.text = "Behavior: Multiple Units";
            unitTargetText.text = "Targets: Multiple Units";
        }*/

        // 적 유닛 선택 여부 확인
        if (UnitSelectionManager.instance.selectedEnemy != null)
        {
            oneUnitSelected = true;
            multipleUnitsSelected = false;
            Enemy selectedEnemy = UnitSelectionManager.instance.selectedEnemy;
            unitNameText.text = selectedEnemy.unitName;
            unitHealthText.text = "Health: " + selectedEnemy.maxHp.ToString() + " / " + selectedEnemy.currentHp.ToString();
            unitMentalText.text = "";
            unitBehaviorText.text = "Behavior: " + selectedEnemy.behavior.ToString();
            unitTargetText.text = "Target: " + (selectedEnemy.target != null ? selectedEnemy.target.name : "None");
        }

        // 유닛 정보 패널 활성화 및 업데이트
        unitInfoPanel.SetActive(true);
    }
}