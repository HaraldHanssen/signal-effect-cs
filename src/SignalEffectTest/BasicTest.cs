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
}