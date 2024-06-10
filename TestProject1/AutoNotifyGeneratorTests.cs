using System.ComponentModel;

namespace Avaruz.Framework.SourceGenerators.Tests
{
  [TestFixture]
  public class AutoNotifyGeneratorTests
  {
    [Test]
    public void TestNotifyPropertyChanged()
    {
      var testClass = new TestClass();
      bool propertyChangedEventFired = false;
      testClass.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == "TestProperty")
        {
          propertyChangedEventFired = true;
        }
      };
      testClass.TestProperty = "new value";
      Assert.IsTrue(propertyChangedEventFired);
    }

    [NotifyPropertyChanged]
    private class TestClass : INotifyPropertyChanged
    {
      private string _testProperty;
      public string TestProperty
      {
        get => _testProperty;
        set
        {
          if (!Equals(_testProperty, value))
          {
            _testProperty = value;
            OnPropertyChanged(nameof(TestProperty));
          }
        }
      }

      public event PropertyChangedEventHandler? PropertyChanged;
      private void OnPropertyChanged(string propertyName)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
