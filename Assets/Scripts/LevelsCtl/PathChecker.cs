using System;
using System.Collections.Generic;
using UnityEngine;

public class PathChecker
{
    private Island _startIsland;
    private List<Island> _islands;

    private Zenject.SignalBus _signalBus;

    [Zenject.Inject]
    private void Init(Zenject.SignalBus signalBus, IslandsProvider islandsProvider){
        _signalBus = signalBus;
        _islands = islandsProvider.Islands;
        _startIsland = _islands.Find(island => island.Type == Island.IslandType.Start);
    }

    public void CheckPath()
    {
        try
        {
            UnityEngine.Debug.LogError("IslandUpdatedSignal？？ Function()==CheckPath currIsland=" +
                                       (_startIsland != null));
            Island currentIsland = _startIsland;
            HashSet<Island> islandsWithoutEnergy = new HashSet<Island>(_islands);

            while (currentIsland != null)
            {
                if (currentIsland.TryGetNextIsland(out Island nextIsland))
                {

                    if (nextIsland.IsEnergyIsland == false)
                    {
                        Debug.LogError($"ret=1 curr={currentIsland.Type:g} next={nextIsland.Type:g}");
                        break;
                    }

                    if (Island.IsInputAndOutputCorrespond(nextIsland.GetInputDirection(),
                            currentIsland.GetOutputDirection()) == false)
                    {
                        Debug.LogError(
                            $"ret=2 curr={currentIsland.Type:g} oOutput={currentIsland.GetOutputDirection()}|next={nextIsland.Type:g} nInput={nextIsland.GetInputDirection()} nOutput={nextIsland.GetOutputDirection()}");
                        break;
                    }

                    islandsWithoutEnergy.Remove(nextIsland);
                    nextIsland.AcivateEnergy();

                    if (nextIsland.Type == Island.IslandType.Finish)
                    {
                        Debug.LogError($"ret=3(complete) curr={currentIsland.Type:g} next={nextIsland.Type:g}");
                        _signalBus.Fire<LevelCompletedSignal>(); //结算。。。。
                        break;
                    }

                    Debug.LogError($"ret=0 curr={currentIsland.Type:g} next={nextIsland.Type:g}");
                    currentIsland = nextIsland;
                }
                else
                {
                    Debug.LogError("ret=-1 not connect by Start(Island)");
                    break;
                }
            }

            foreach (Island island in islandsWithoutEnergy)
                island.DeactivateEnergy();
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
            throw;
        }
    }
}
