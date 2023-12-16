using SignalEffect;

namespace SignalEffectTest;

[TestClass]
public class BasicTest
{
    private static readonly Scope M = Scope.Default;

    [TestMethod]
    public void GetSignalValue()
    {
        var s = M.Signal(42);
        Assert.AreEqual(42, s.Get());
    }

    [TestMethod]
    public void SetSignalValue()
    {
        var s = M.Signal(42);
        s.Set(43);
        Assert.AreEqual(43, s.Get());
    }

    [TestMethod]
    public void GetReadonlyValue()
    {
        var s = M.Signal(42);
        var r = M.Readonly(s);
        Assert.AreEqual(42, r.Get());
        s.Set(43);
        Assert.AreEqual(43, r.Get());
    }

    public class ModifySignalValueType
    {
        public int Meaning { get; set; }
    }

    [TestMethod]
    public void ModifySignalValue()
    {
        var theObject = M.Signal(new ModifySignalValueType { Meaning = 4 });
        var theArray = M.Signal(new List<int> { 4 });

        M.Modify(theObject, x => x.Meaning += 38);
        M.Modify(theArray, x => x.Add(2));

        Assert.AreEqual(42, theObject.Get().Meaning);
        Assert.AreEqual(2, theArray.Get().Count);
        Assert.AreEqual(4, theArray.Get()[0]);
        Assert.AreEqual(2, theArray.Get()[1]);
    }

    [TestMethod]
    public void CalcDerivedValueOf1Signal()
    {
        var calculated = 0;
        var s = M.Signal(42);
        var d = M.Derived(s, (x) =>
        {
            calculated++;
            return 2 * x;
        });
        Assert.AreEqual(0, calculated);
        Assert.AreEqual(84, d.Get());
        Assert.AreEqual(1, calculated);
        s.Set(43);
        Assert.AreEqual(1, calculated);
        Assert.AreEqual(86, d.Get());
        Assert.AreEqual(2, calculated);
        Assert.AreEqual(86, d.Get());
        Assert.AreEqual(2, calculated);
    }

    [TestMethod]
    public void CalcDerivedValueOf2Signals()
    {
        var calculated = 0;
        var (s, t, _) = M.Signals(42, 4);
        var d = M.Derived(s, t, (x, y) =>
        {
            Assert.AreNotEqual(x, y);
            calculated++;
            return x + y;
        });
        Assert.AreEqual(46, d.Get());
        Assert.AreEqual(1, calculated);
        t.Set(3);
        Assert.AreEqual(45, d.Get());
        Assert.AreEqual(2, calculated);
        s.Set(41);
        Assert.AreEqual(44, d.Get());
        Assert.AreEqual(3, calculated);
        Assert.AreEqual(44, d.Get());
        Assert.AreEqual(3, calculated);
    }

    [TestMethod]
    public void CalcDerivedValueOfNSignals()
    {
        var calculated = 0;
        var (s, t, _) = M.Signals(42, 4);
        var d = M.Derived([s, t], (a) =>
        {
            var (x, y, _) = a;
            Assert.AreNotEqual(x, y);
            calculated++;
            return x + y;
        });
        Assert.AreEqual(46, d.Get());
        Assert.AreEqual(1, calculated);
        t.Set(3);
        Assert.AreEqual(45, d.Get());
        Assert.AreEqual(2, calculated);
        s.Set(41);
        Assert.AreEqual(44, d.Get());
        Assert.AreEqual(3, calculated);
        Assert.AreEqual(44, d.Get());
        Assert.AreEqual(3, calculated);
    }

    [TestMethod]
    public void CalcDerivedValueIsOnlyTriggeredByDependentSignals()
    {
        var calculated = 0;
        var (s, t, u, _) = M.Signals(42, 4, 2);
        var d = M.Derived(s, u, (x, y) =>
        {
            Assert.AreNotEqual(x, y);
            calculated++;
            return 2 * x * y;
        });
        Assert.AreEqual(168, d.Get());
        Assert.AreEqual(1, calculated);
        t.Set(3);
        Assert.AreEqual(168, d.Get());
        Assert.AreEqual(1, calculated); // not called
        s.Set(43);
        Assert.AreEqual(172, d.Get());
        Assert.AreEqual(2, calculated);
        u.Set(1);
        Assert.AreEqual(86, d.Get());
        Assert.AreEqual(3, calculated);
    }

    [TestMethod]
    public void ActEffectOf1Signal()
    {
        var acted = 0;
        var s = M.Signal(42);
        var e = M.Effect(s, (x) =>
        {
            acted++;
        });
        Assert.AreEqual(0, acted);
        e.Call();
        Assert.AreEqual(1, acted);
        s.Set(43);
        Assert.AreEqual(1, acted);
        e.Call();
        Assert.AreEqual(2, acted);
        e.Call();
        Assert.AreEqual(2, acted);
    }

    [TestMethod]
    public void ActEffectOf2Signals()
    {
        var acted = 0;
        var (s, t, _) = M.Signals(42, 4);
        var e = M.Effect(s, t, (x, y) =>
        {
            Assert.AreNotEqual(x, y);
            acted++;
        });
        Assert.AreEqual(0, acted);
        e.Call();
        Assert.AreEqual(1, acted);
        t.Set(3);
        Assert.AreEqual(1, acted);
        e.Call();
        Assert.AreEqual(2, acted);
        e.Call();
        Assert.AreEqual(2, acted);
    }

    [TestMethod]
    public void ActEffectOfNSignals()
    {
        var acted = 0;
        var (s, t, _) = M.Signals(42, 4);
        var e = M.Effect([s, t], (a) =>
        {
            var (x, y, _) = a;
            Assert.AreNotEqual(x, y);
            acted++;
        });
        Assert.AreEqual(0, acted);
        e.Call();
        Assert.AreEqual(1, acted);
        t.Set(3);
        Assert.AreEqual(1, acted);
        e.Call();
        Assert.AreEqual(2, acted);
        e.Call();
        Assert.AreEqual(2, acted);
    }


    [TestMethod]
    public void ActEffectIsOnlyTriggeredByDependentSignals()
    {
        var acted = 0;
        var (s, t, u, _) = M.Signals(42, 4, 2);
        var e = M.Effect(s, u, (x, y) =>
        {
            Assert.AreNotEqual(x, y);
            acted++;
        });
        e.Call();
        Assert.AreEqual(1, acted);
        t.Set(3);
        e.Call();
        Assert.AreEqual(1, acted); // not called
        s.Set(43);
        e.Call();
        Assert.AreEqual(2, acted);
        u.Set(1);
        e.Call();
        Assert.AreEqual(3, acted);
    }

    //TODO From line 220
}