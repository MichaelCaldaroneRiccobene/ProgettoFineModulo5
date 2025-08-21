using UnityEngine;

public abstract class AbstractState : MonoBehaviour
{
    protected FSM_Controller controller;

    protected AbstractTransition[] transitions;

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();


    public virtual void SetUp(FSM_Controller controller)
    {
        this.controller = controller;
        transitions = GetComponents<AbstractTransition>();
    }

    public AbstractState EvaluateTransition()
    {
        foreach (AbstractTransition transition in transitions)
        {
            if(transition.IsConditionMet(controller,this)) return transition.GetTargetState();
        }
        return null;
    }
}
