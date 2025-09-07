using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementInfo : MonoBehaviour
{
    private TextMeshProUGUI achievementInfo;
    private Achievement achievement;
    public int id;
    public void Awake()
    {
        achievementInfo=GetComponent<TextMeshProUGUI>();
    }
    public void Init(Achievement achievement)
    {
        id=achievement.ID;
        achievementInfo.text = achievement.title+"\n"+achievement.description;
    }
    public void Clear()
    {
        StartCoroutine(ClearEffect());
    }

    private IEnumerator ClearEffect()
    {
      
        string originalText = achievementInfo.text;
        achievementInfo.text = $"<s>{originalText}</s>";

       
        yield return new WaitForSeconds(0.3f);

        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            achievementInfo.alpha = Mathf.Lerp(1, 0, elapsed / duration);
       
            yield return null;
        }

        Destroy(gameObject);
    }
}
