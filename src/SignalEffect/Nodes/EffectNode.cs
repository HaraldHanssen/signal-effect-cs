namespace SignalEffect;

internal abstract class EffectNode : DependentNode
{
    protected EffectNode(CallTrack track) : base(track)
    {
        In = [];
    }

    public IEffect AsEffect() {
        return new Effect(Id, this, Fun);
    }

    protected abstract void Do(SequenceNumber check);

    private void Fun()
    {
        var check = CurrN();
        if (Visited) throw new ReentryException(ReentryException.ERR_LOOP);
        if (Track.State.NoCall) throw new ReentryException(ReentryException.ERR_CALL);
        if (!Dropped && check > Checked)
        {
            if (Dirty)
            {
                try
                {
                    Track.Enter();
                    Do(check);
                    Dirty = false;
                }
                finally
                {
                    Track.Exit();
                }
            }

            Checked = check;
        }
    }
}
