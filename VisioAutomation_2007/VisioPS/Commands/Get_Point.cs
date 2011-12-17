using System.Linq;
using VA = VisioAutomation;
using SMA = System.Management.Automation;


namespace VisioPS.Commands
{
    [SMA.Cmdlet("Get", "Point")]
    public class Get_Point : VisioPS.VisioPSCmdlet
    {
        [SMA.Parameter(Position = 0, Mandatory = true)]
        public double[] Doubles { get; set; }

        protected override void ProcessRecord()
        {
            var points = VA.Drawing.Point.FromDoubles(this.Doubles).ToList();
            this.WriteObject(points);
        }
    }
}