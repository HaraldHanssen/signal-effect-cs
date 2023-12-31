using SignalEffect;

namespace SignalEffectTest;

[TestClass]
public class BasicTest
{
    private static readonly Scope<ManualExecution> M = Scope.DefaultManual;

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

    [TestMethod]
    public void UpdateOfDerivedWillOnlyTriggerOncePerProvidedElement()
    {
        var calculated = 0;
        var (s, t, u, _) = M.Signals(42, 4, 2);
        var a = M.Derived(s, t, u, (x, y, z) =>
        {
            calculated++;
            return $"{x}:{y}:{z}";
        });
        var b = M.Derived(s, t, u, (x, y, z) =>
        {
            calculated++;
            return x + y + z;
        });
        var c = M.Derived(s, t, u, (x, y, z) =>
        {
            calculated++;
            return $"{z}:{y}:{x}";
        });
        var d = M.Derived(a, b, c, (x, y, z) =>
        {
            calculated++;
            return $"{x}:{y}:{z}";
        });
        var e = M.Derived(b, d, (x, y) =>
        {
            calculated++;
            return $"{x}:{y}";
        });
        Assert.AreEqual(0, calculated);
        M.Update([a, b, c, d, e]);
        Assert.AreEqual(5, calculated);
        Assert.AreEqual("42:4:2", a.Get());
        Assert.AreEqual(48, b.Get());
        Assert.AreEqual("2:4:42", c.Get());
        Assert.AreEqual("42:4:2:48:2:4:42", d.Get());
        Assert.AreEqual("48:42:4:2:48:2:4:42", e.Get());
        M.Update([a, b, c, d, e]);
        Assert.AreEqual(5, calculated); // not called
        Assert.AreEqual("42:4:2", a.Get());
        Assert.AreEqual(48, b.Get());
        Assert.AreEqual("2:4:42", c.Get());
        Assert.AreEqual("42:4:2:48:2:4:42", d.Get());
        Assert.AreEqual("48:42:4:2:48:2:4:42", e.Get());
    }

    [TestMethod]
    public void UpdateOfDerivedWillTriggerForTransitiveDependencyChange()
    {
        var calculated = 0;
        var (s, t, u, _) = M.Signals(42, 4, 2);
        var a = M.Derived(s, t, (x, y) =>
        {
            calculated++;
            return x + y;
        });
        var b = M.Derived(u, (x) =>
        {
            calculated++;
            return 2 * x;
        });
        var c = M.Derived(a, b, (x, y) =>
        {
            calculated++;
            return x + y;
        });
        Assert.AreEqual(0, calculated);
        M.Update([c]);
        Assert.AreEqual(3, calculated);
        Assert.AreEqual(42 + 4, a.Get());
        Assert.AreEqual(2 * 2, b.Get());
        Assert.AreEqual(42 + 4 + 2 * 2, c.Get());
        u.Set(3);
        M.Update([c]);
        Assert.AreEqual(5, calculated); // a is not recalculated
        Assert.AreEqual(42 + 4 + 2 * 3, c.Get());
        Assert.AreEqual(2 * 3, b.Get());
        Assert.AreEqual(42 + 4, a.Get());
    }

    [TestMethod]
    public void UpdateOfEffectWillOnlyTriggerOncePerProvidedElement()
    {
        var acted = 0;
        var calculated = 0;
        var r1 = "";
        var r2 = "";
        var (s, t, u, _) = M.Signals(42, 4, 2);
        var a = M.Derived(s, t, u, (x, y, z) => {
            calculated++;
            return $"{x}:{y}:{z}";
        });
        var b = M.Derived(s, t, u, (x, y, z) => {
            calculated++;
            return x + y + z;
        });
        var c = M.Derived(s, t, u, (x, y, z) => {
            calculated++;
            return $"{z}:{y}:{x}";
        });
        var d = M.Effect(a, b, c, (x, y, z) => {
            acted++;
            r1 = $"{x}:{y}:{z}";
        });
        var e = M.Effect(a, b, (x, y) => {
            acted++;
            r2 = $"{x}:{y}";
        });
        Assert.AreEqual(0, acted);
        Assert.AreEqual(0, calculated);
        M.Update([d, e]);
        Assert.AreEqual(2, acted);
        Assert.AreEqual(3, calculated);
        Assert.AreEqual("42:4:2:48:2:4:42", r1);
        Assert.AreEqual("42:4:2:48", r2);
        M.Update([d, e]);
        Assert.AreEqual(2, acted); // not called
        Assert.AreEqual(3, calculated); // not called
        Assert.AreEqual("42:4:2:48:2:4:42", r1);
        Assert.AreEqual("42:4:2:48", r2);
    }

    [TestMethod]
    public void UpdateOfEffectWillTriggerForTransitiveDependencyChange()
    {
        var acted = 0;
        var calculated = 0;
        var r1 = 0;
        var (s, t, u, _) = M.Signals(42, 4, 2);

        var a = M.Derived(s, t, (x, y) => {
            calculated++;
            return x + y;
        });
        var b = M.Derived(u, (x) => {
            calculated++;
            return 2 * x;
        });
        var c = M.Effect(a, b, (x, y) => {
            acted++;
            r1 = x + y;
        });
        Assert.AreEqual(0, calculated);
        M.Update([c]);
        Assert.AreEqual(1, acted);
        Assert.AreEqual(2, calculated);
        Assert.AreEqual(42 + 4, a.Get());
        Assert.AreEqual(2 * 2, b.Get());
        Assert.AreEqual(42 + 4 + 2 * 2, r1);
        u.Set(3);
        M.Update([c]);
        Assert.AreEqual(3, calculated); // a is not recalculated
        Assert.AreEqual(42 + 4 + 2 * 3, r1);
        Assert.AreEqual(2 * 3, b.Get());
        Assert.AreEqual(42 + 4, a.Get());
    }
}