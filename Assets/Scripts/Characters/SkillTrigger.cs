using System;
using UnityEngine;

namespace Characters
{
    public class SkillTrigger : MonoBehaviour
    {

        private GameObject _playerObject;
        public GameObject GetPlayerObject() => _playerObject;
        private bool _inTrigger = false;
        public bool IsPlayerInTrigger() => _inTrigger;

        public void PlayerEnteredTrigger(GameObject playerObject)
        {
            Debug.Log("Entered VFX trigger");
            _inTrigger = true;
            _playerObject = playerObject;
        }

        public void PlayerLeftTrigger() => _inTrigger = false;
    }
}