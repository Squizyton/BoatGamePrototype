using FishStuff.Complex_AI;

namespace FishStuff.States
{
    public abstract class State
    {
        protected FishAI fish;

        protected AIData aiData;

        public ContextSolver contextSolver;
        public abstract void OnInitialized(FishAI passedFish, AIData passedData);

        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void OnExit();
    }
}