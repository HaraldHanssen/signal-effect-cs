namespace SignalEffect;

internal abstract class EffectNode : DependentNode
{
    protected EffectNode(ICallTrack track)
    {
        In = [];
        Track = track;
    }

    protected ICallTrack Track { get; }

    protected Effect AsEffect() {
        return new Effect(Id, this, Fun);
    }

    protected abstract void Do(SequenceNumber check);

    private void Fun()
    {
        var check = CurrN();
        if (Visited) throw new ReentryException(ReentryException.ERR_LOOP);
        //TODO if (track.nocall) throw new ReentryException(ReentryException.ERR_CALL);
        if (!Dropped && check > Checked)
        {
            if (Dirty)
            {
                try
                {
                    //TODO enter();
                    Do(check);
                    Dirty = false;
                }
                finally
                {
                    //exit();
                }
            }

            Checked = check;
        }
    }
}
