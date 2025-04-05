using System;

public class MiscEvents
{
    public event Action onCoinCollected;
    public void CoinCollected()
    {
        if (onCoinCollected != null)
        {
            onCoinCollected();
        }
    }

    public event Action onGemCollected;
    public void GemCollected()
    {
        if (onGemCollected != null)
        {
            onGemCollected();
        }
    }

    public event Action onPuzzleSolved;
    public void PuzzleSolved()
    {
        if (onPuzzleSolved != null)
        {
            onPuzzleSolved();
        }
    }

    public event Action<string> onNPCTalked;
    public void NPCTalked(string npcName)
    {
        if (onNPCTalked != null)
        {
            onNPCTalked(npcName);
        }
    }

    public event Action onDestinationReached;
    public void DestinationReached()
    {
        if (onDestinationReached != null)
        {
            onDestinationReached();
        }
    }

}