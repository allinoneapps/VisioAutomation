using System.Management.Automation;
using VisioAutomation.Models.Layouts.DirectedGraph;
using VA = VisioAutomation;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.New, VisioPowerShell.Nouns.VisioModelDirectedGraph)]
    public class New_VisioModelDirectedGraph : VisioCmdlet
    {
        protected override void ProcessRecord()
        {
            var dg_model = new DirectedGraphLayout();
            this.WriteObject(dg_model);
        }
    }
}