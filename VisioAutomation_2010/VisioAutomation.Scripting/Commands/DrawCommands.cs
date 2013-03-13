using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using GRIDLAYOUT = VisioAutomation.Layout.Models.Grid;
using RADIALLAYOUT = VisioAutomation.Layout.Models.Radial;
using ORGCHARTLAYOUT = VisioAutomation.Layout.Models.OrgChart;

namespace VisioAutomation.Scripting.Commands
{
    public class DrawCommands : CommandSet
    {
        public DrawCommands(Session session) :
            base(session)
        {

        }

        public IList<IVisio.Shape> Table(System.Data.DataTable datatable,
                                          IList<double> widths,
                                          IList<double> heights,
                                          VA.Drawing.Size cellspacing)
        {
            if (datatable == null)
            {
                throw new System.ArgumentNullException("datatable");
            }

            if (widths == null)
            {
                throw new System.ArgumentNullException("widths");
            }

            if (heights == null)
            {
                throw new System.ArgumentNullException("heights");
            }

            if (datatable.Rows.Count < 1)
            {
                return new List<IVisio.Shape>(0);
            }
            
            string master = "Rectangle";
            string stencil = "basic_u.vss";
            var stencildoc = this.Session.Document.OpenStencil(stencil);
            var stencildoc_masters = stencildoc.Masters;
            var masterobj = stencildoc_masters.ItemU[master];

            var application = this.Session.VisioApplication;
            var active_document = application.ActiveDocument;
            var pages = active_document.Pages;

            var page = pages.Add();
            page.Background = 0; // ensure this is a foreground page

            var pagesize = this.Session.Page.GetSize();
            
            var layout = new GRIDLAYOUT.GridLayout(datatable.Columns.Count, datatable.Rows.Count, new VA.Drawing.Size(1, 1), masterobj);
            layout.Origin = new VA.Drawing.Point(0, pagesize.Height);
            layout.CellSpacing = cellspacing;
            layout.RowDirection = GRIDLAYOUT.RowDirection.TopToBottom;
            layout.PerformLayout();

            foreach (var i in Enumerable.Range(0, datatable.Rows.Count))
            {
                var row = datatable.Rows[i];

                for (int col_index = 0; col_index < row.ItemArray.Length; col_index++)
                {
                    var col = row.ItemArray[col_index];
                    var cur_label = (col != null) ? col.ToString() : string.Empty;
                    var node = layout.GetNode(col_index, i);
                    node.Text = cur_label;
                }
            }

            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Table"))
            {
                layout.Render(page);
                page.ResizeToFitContents();
            }

            var page_shapes = page.Shapes;
            var shapes = layout.Nodes.Select(n => n.Shape ).ToList();
            return shapes;

        }

        public void Grid(GRIDLAYOUT.GridLayout layout)
        {
            
            //Create a new page to hold the grid
            var application = this.Session.VisioApplication;
            var page = application.ActivePage;
            layout.PerformLayout();

            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Grid"))
            {
                layout.Render(page);
            }
        }

        public IVisio.Shape NURBSCurve(IList<VA.Drawing.Point> controlpoints,
                                    IList<double> knots,
                                    IList<double> weights, int degree)
        {

            // flags:
            // None = 0,
            // IVisio.VisDrawSplineFlags.visSpline1D

            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw NURBS Curve"))
            {

                var page = application.ActivePage;
                var shape = page.DrawNURBS(controlpoints, knots, weights, degree);
                return shape;
            }
        }

        public IVisio.Shape Rectangle(double x0, double y0, double x1, double y1)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Rectangle"))
            {
                var active_page = application.ActivePage;
                var shape = active_page.DrawRectangle(x0, y0, x1, y1);
                return shape;
            }
        }

        public IVisio.Shape Line(double x0, double y0, double x1, double y1)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Line"))
            {
                var active_page = application.ActivePage;
                var shape = active_page.DrawLine(x0, y0, x1, y1);
                return shape;
            }
        }

        public IVisio.Shape Oval(double x0, double y0, double x1, double y1)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Oval"))
            {
                var active_page = application.ActivePage;
                var shape = active_page.DrawOval(x0, y0, x1, y1);
                return shape;
            }
        }

        public IVisio.Shape Oval(VA.Drawing.Point center, double radius)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication, "Draw Oval"))
            {
                var A = center.Add(-radius, -radius);
                var B = center.Add(radius, radius);
                var rect = new VA.Drawing.Rectangle(A, B);
                var active_page = application.ActivePage;
                var shape = active_page.DrawOval(rect);
                return shape;
            }
        }

        public IVisio.Shape Bezier(IEnumerable<VA.Drawing.Point> points)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication,"Draw Bezier"))
            {
                var active_page = application.ActivePage;
                var shape = active_page.DrawBezier(points.ToList());
                return shape;
            }
        }

        public IVisio.Shape PolyLine(IList<VA.Drawing.Point> points)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication,"Draw PolyLine"))
            {
                var active_page = application.ActivePage;
                var shape = active_page.DrawPolyline(points);
                return shape;
            }
        }

        public IVisio.Shape PieSlice(VA.Drawing.Point center,
                                  double radius,
                                  double start_angle,
                                  double  end_angle)
        {
            var application = this.Session.VisioApplication;
            using (var undoscope = new VA.Application.UndoScope(this.Session.VisioApplication,"Draw Pie Slice"))
            {
                var active_page = application.ActivePage;
                var shape = DrawCommandsUtil.DrawPieSlice(active_page, center, radius, start_angle, end_angle);
                return shape;
            }
        }

        public IList<IVisio.Shape> PieSlices(VA.Drawing.Point center,
                                          double radius,
                                          IList<double> values)
        {
            if (!this.Session.HasActiveDrawing)
            {
                return null;
            }

            var application = this.Session.VisioApplication;
            var page = application.ActivePage;
            var slices = RADIALLAYOUT.PieSlice.GetSlicesFromValues(center, radius, values);
            var shapes = new List<IVisio.Shape>(slices.Count);
            foreach (var slice in slices)
            {
                var shape = slice.Render(page);
                shapes.Add(shape);
            }
            return shapes;
        }

        public void OrgChart(ORGCHARTLAYOUT.Drawing drawing)
        {
            this.Session.WriteVerbose( "Start OrgChart Rendering");
            var renderer = new ORGCHARTLAYOUT.OrgChartRenderer();
            var application = this.Session.VisioApplication;
            drawing.Render(application);
            var active_page = application.ActivePage;
            active_page.ResizeToFitContents();
            this.Session.WriteVerbose( "Finished OrgChart Rendering");
        }
    }
}