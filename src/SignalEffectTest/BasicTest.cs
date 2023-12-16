using SignalEffect;

namespace SignalEffectTest;

[TestClass]
public class BasicTest
{
    private static readonly Scope Q = Scope.Default;

    [TestMethod]
    public void GetSignalValue()
    {
        var s = Q.Signal(42);
        Assert.AreEqual(42, s.Get());
    }

    [TestMethod]
    public void SetSignalValue()
    {
        var s = Q.Signal(42);
        s.Set(43);
        Assert.AreEqual(43, s.Get());
    }

    [TestMethod]
    public void GetReadonlyValue()
    {
        var s = Q.Signal(42);
        var r = Q.Readonly(s);
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
        var theObject = Q.Signal(new ModifySignalValueType { Meaning = 4 });
        var theArray = Q.Signal(new List<int> { 4 });

        Q.Modify(theObject, x => x.Meaning += 38);
        Q.Modify(theArray, x => x.Add(2));

        Assert.AreEqual(42, theObject.Get().Meaning);
        Assert.AreEqual(2, theArray.Get().Count);
        Assert.AreEqual(4, theArray.Get()[0]);
        Assert.AreEqual(2, theArray.Get()[1]);
    }

    [TestMethod]
    public void CalcDerivedValueOf1Signal()
    {
        var calculated = 0;
        var s = Q.Signal(42);
        var d = Q.Derived(s, (x) =>
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
        var (s, t, _) = Q.Signals(42, 4);
        var d = Q.Derived(s, t, (x, y) =>
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
        var (s, t, _) = Q.Signals(42, 4);
        var d = Q.Derived([s, t], (a) =>
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
}