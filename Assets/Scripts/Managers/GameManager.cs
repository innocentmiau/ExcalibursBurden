using Important;
using UnityEngine;

namespace Managers
{
    
    public class GameManager : MonoBehaviour
    {

        private Data _data;
    
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            _data = new Data();
            
        }

        void Update()
        {
        
        }
    }
    
}

