using Microsoft.VisualStudio.TestTools.UnitTesting;
using VA = VisioAutomation;

namespace TestVisioAutomation
{
    [TestClass]
    public class ScriptingControlTests : VisioAutomationTest
    {
        [TestMethod]
        public void Scripting_Controls_Scenario_0()
        {
            var ss = GetScriptingSession();
            ss.Document.New();
            ss.Page.New(new VA.Drawing.Size(4, 4), false);

            var s1 = ss.Draw.Rectangle(1, 1, 1.5, 1.5);

            var s2 = ss.Draw.Rectangle(2, 3, 2.5, 3.5);

            var s3 = ss.Draw.Rectangle(1.5, 3.5, 2, 4.0);

            ss.Selection.SelectNone();
            ss.Selection.Select(s1);
            ss.Selection.Select(s2);
            ss.Selection.Select(s3);

            var controls0 = ss.Control.Get(null);
            int found_controls = controls0.Count;
            Assert.AreEqual(3, controls0.Count);
            Assert.AreEqual(0, controls0[s1].Count);
            Assert.AreEqual(0, controls0[s2].Count);
            Assert.AreEqual(0, controls0[s3].Count);

            var ctrl = new VA.Controls.ControlCells();
            ctrl.X = "Width*0.5";
            ctrl.Y = "0";
            ss.Control.Add(null,ctrl);

            var controls1 = ss.Control.Get(null);
            Assert.AreEqual(3, controls1.Count);
            Assert.AreEqual(1, controls1[s1].Count);
            Assert.AreEqual(1, controls1[s2].Count);
            Assert.AreEqual(1, controls1[s3].Count);

            ss.Control.Delete(null,0);
            var controls2 = ss.Control.Get(null);
            Assert.AreEqual(3, controls0.Count);
            Assert.AreEqual(0, controls2[s1].Count);
            Assert.AreEqual(0, controls2[s2].Count);
            Assert.AreEqual(0, controls2[s3].Count);

            ss.Document.Close(true);
        }
    }
}