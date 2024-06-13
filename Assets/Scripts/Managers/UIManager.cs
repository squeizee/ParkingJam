using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI levelCompleteText;
        [SerializeField] private GameObject levelComplete;
        [SerializeField] private GameObject fireworksPrefab;


        private void OnEnable()
        {
            LevelManager.OnLevelStart += level => levelText.text = $"LEVEL {level.GetLevelNumber.ToString()}";
            GameManager.OnWin += Win;
        }

        private void OnDisable()
        {
            GameManager.OnWin -= Win;
        }

        private void Win()
        {
            var loopStep = 4;
            levelCompleteText.text = $"Next Level in {loopStep}..";
            levelCompleteText.transform.DOScale(Vector3.one, 0f);
            
            levelComplete.SetActive(true);
            
            var fireworks = Instantiate(fireworksPrefab, levelComplete.transform);
            Destroy(fireworks,4f);
            
            levelCompleteText.transform.DOScale(Vector3.one * .9f, 1f).SetLoops(5).OnStepComplete(() =>
            {
                levelCompleteText.text = $"Next Level in {loopStep}..";
                loopStep--;
                
            }).OnComplete(() =>
            {
                levelComplete.SetActive(false);
                GameManager.OnStart?.Invoke();
            });
        }
    }
}