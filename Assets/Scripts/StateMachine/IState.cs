public interface Istate
{
    void Enter(IStateMachine machine);
    void Tick(float delta);
    void FixedTick();
    void Leave();
}