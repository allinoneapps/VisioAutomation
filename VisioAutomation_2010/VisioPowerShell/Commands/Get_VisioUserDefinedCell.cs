using System.Management.Automation;
using VisioAutomation.Scripting.Models;
using VisioPowerShell.Models;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, VisioPowerShell.Nouns.VisioUserDefinedCell)]
    public class Get_VisioUserDefinedCell : VisioCmdlet
    {
        [Parameter(Mandatory = false)]
        public IVisio.Shape[] Shapes;

        [Parameter(Mandatory = false)]
        public SwitchParameter GetCells;

        protected override void ProcessRecord()
        {
            var targets = new TargetShapes(this.Shapes);
            var dic = this.Client.UserDefinedCell.Get(targets);

            if (this.GetCells)
            {
                this.WriteObject(dic);
                return;
            }

            foreach (var kv in dic)
            {
                int shapeid = kv.Key.ID;
                foreach (var udc in kv.Value)
                {
                    var udcell_vals = new UserDefinedCell(shapeid, udc.Name, udc.Value.Formula.Value,udc.Prompt.Formula.Value);
                    this.WriteObject(udcell_vals);
                }
            }
        }
    }
}