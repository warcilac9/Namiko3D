using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    interface IInteract
    {
        void Interaction();
    }
    class Collect : IInteract
    {
        public void Interaction()
        {
            
        }
    }
    class Dialogue : IInteract
    {
        public void Interaction()
        {

        }
    }
    class Minigame : IInteract
    {
        public void Interaction()
        {

        }
    }
}
