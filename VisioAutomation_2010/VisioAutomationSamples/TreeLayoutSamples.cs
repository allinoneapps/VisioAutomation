﻿using VisioAutomation.DOM;
using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace VisioAutomationSamples
{
    public static class TreeLayoutSamples
    {

        public static void TreeWithTwoPassLayoutAndFormatting()
        {
            var doc = SampleEnvironment.Application.ActiveDocument;
            var page1 = doc.Pages.Add();

            var t = new VA.Layout.Models.Tree.Drawing();

            t.Root = new VA.Layout.Models.Tree.Node("Root");

            var na = new VA.Layout.Models.Tree.Node("A");
            var nb = new VA.Layout.Models.Tree.Node("B");

            var na1 = new VA.Layout.Models.Tree.Node("A1");
            var na2 = new VA.Layout.Models.Tree.Node("A2");

            var nb1 = new VA.Layout.Models.Tree.Node("B1");
            var nb2 = new VA.Layout.Models.Tree.Node("B2");

            t.Root.Children.Add(na);
            t.Root.Children.Add(nb);

            na.Children.Add(na1);
            na.Children.Add(na2);

            nb.Children.Add(nb1);
            nb1.Children.Add(nb2);

            var fontname = "Segoe UI";
            var font = doc.Fonts[fontname];

            foreach (var tn in t.Nodes)
            {
                var cells = new ShapeCells();
                tn.Cells = cells;

                cells.HAlign = 0; // align text to left
                cells.VerticalAlign = 0; // align text block to top
                cells.CharFont = font.ID;
                cells.CharSize = "10pt";
                cells.FillForegnd = "rgb(255,250,200)";
                cells.CharColor = "rgb(255,0,0)";
            }
        }
    }

}