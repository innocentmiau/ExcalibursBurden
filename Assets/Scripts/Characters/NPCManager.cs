using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    
    public class NPCManager : MonoBehaviour
    {

        [SerializeField] private string npcTalksFileName;
        [SerializeField] private Character npcObject;
        private HealthSystem _healthSystem;


        private NPCData _npcData;
        
        private void Start()
        {
            gameObject.tag = npcObject.tag;
            if (npcObject.attackable)
            {
                _healthSystem = GetComponent<HealthSystem>();
                if (_healthSystem != null) _healthSystem.Setup(npcObject);
            }

            if (npcTalksFileName == null || npcTalksFileName.Length == 0) return;
            TextAsset jsonData = Resources.Load<TextAsset>("NpcTalks/" + npcTalksFileName);
            if (jsonData == null)
            {
                Debug.LogWarning("Couldn't find npc talks json.");
                return;
            }
            NPCData data = JsonUtility.FromJson<NPCData>(jsonData.text);
            if (data == null)
            {
                Debug.LogError("Failed to parse JSON");
                return;
            }

            _npcData = data;

            (int id, int maintalk, Dictionary<int, int> answers) talk = GetTalkStuff(1);
            Debug.Log($"{talk.id} {talk.maintalk} {TransformDicIntoString(talk.answers)}");
        }

        // testing purposes:
        private string TransformDicIntoString(Dictionary<int, int> dic)
        {
            string text = "";
            foreach (int key in dic.Keys)
            {
                text += $"{key}:{dic[key]}";
            }
            return text;
        }

        public (int, int, Dictionary<int, int>) GetTalkStuff(int talkID)
        {
            if (talkID == -1)
            {
                return (-2, -2, null);
            }
            string allData = _npcData.talks[talkID];
            string[] datas = allData.Split(" ");
            int mainTalk = int.Parse(datas[1]);
            Dictionary<int, int> answers = new Dictionary<int, int>();
            if (datas.Length > 2)
            {
                if (datas[2].Contains(","))
                {
                    foreach (string each in datas[2].Split(","))
                    {
                        string[] values = each.Split("-");
                        int answer = values.Length > 1 ? int.Parse(values[1]) : -1;
                        answers.Add(int.Parse(values[0]), answer);
                    }
                }
                else
                {
                    string[] values = datas[2].Split("-");
                    int answer = values.Length > 1 ? int.Parse(values[1]) : -1;
                    answers.Add(int.Parse(values[0]), answer);
                }
            }
            else
            {
                Debug.Log("wow, no answer here, close dialogue.");
                return (talkID, mainTalk, null);
            }
            Debug.Log($"{talkID} {mainTalk} {TransformDicIntoString(answers)}");
            return (talkID, mainTalk, answers);
        }

        private float _lastAttacked = 0f;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.tag.Equals("Weapon") || _healthSystem == null) return;
            if (collider.TryGetComponent(out SwordManager swordManager) && swordManager.StartAttack > _lastAttacked)
            {
                _lastAttacked = swordManager.StartAttack;
                _healthSystem.TakeDamage(swordManager.Damage);
                if (transform.TryGetComponent(out EnemyManager enemyManager))
                {
                    enemyManager.TookDamage();
                }
            }
        }
    }
    
}
