﻿using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace VisioAutomationSamples
{
    public static class TextSamples
    {
        private static string text1 =
            @"
<text font=""Calibri"" size=""15"" color=""#ff0000"">

    <text>Hello World [Calibri 15pt red]</text>
    <br/><br/><br/><br/>
    <text font=""Segoe UI"" size=""20"" color=""#0000ff"">
        Hello World [Segoe UI 20 pt blue]
        <text italic=""1"" bold=""1"" halign=""left"" color=""#505050"">
            Hello World [ left italic gray ]
            <text bold=""1"" italic=""1"" halign=""right"">
                Hello World [ bold,italic right]
            </text>
            <text bold=""0"" italic=""0"" halign=""center"">
                Hello World [ nobold,noitalic,center]
            </text>
            Hello World [ left italic gray ]
        </text>
        Hello World [Segoe UI 20 pt blue]
    </text>
    Hello World [Calibri 15pt red]
</text>";

        private static string text2 =
            @"
    <text size=""20"" font=""Segoe UI""> The lines underneath should be bulleted 

<text bullets=""1"" halign=""left""> This a demonstration of <text bold=""1"">bold</text> text
A demonstration of <text italic=""1"">italic</text> text
CellsPackage can be combined to form <text bold=""1"" italic=""1"">bold italic</text>
This word is <text underline=""1"" >under<text underline=""0"" >lined</text></text>
This word is <text smallcaps=""1"" >smallcaps</text>
</text>

The bullets have ended.
</text> ";

        private static string text3 =
            @"
    <text size=""20"" font=""Segoe UI"" halign=""left""> The lines below should be indented. And they should get increasingly more transparent.

<text indent=""25"" transparency=""25"">
indent 25
</text>
<text indent=""50"" transparency=""45"">
indent 50
</text>
<text indent=""75"" transparency=""65"">
indent 75
</text>
<text indent=""100"" transparency=""85"">
indent 100
</text>

The indenting has ended.


</text> ";

        private static string text4 =
            "<text size=\"30\"> This tests special characters <text> Carriage return [\r] </text> <text> Line feed [\n] </text> </text> ";

        public static void NonRotatingText()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();
            var s0 = page.DrawRectangle(1, 1, 4, 4);
            s0.Text = "Hello World";

            s0.GetCell(VA.ShapeSheet.SRCConstants.TxtAngle).Formula = "-Angle";
        }

        public static void TextFields()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();
            var s0 = page.DrawRectangle(1, 1, 4, 4);

            VA.Text.TextHelper.SetTextFormatFields(s0, "{0} ({1} of {2})", 
                VA.Text.Markup.Fields.NumberOfPages,
                VA.Text.Markup.Fields.PageNumber,
                VA.Text.Markup.Fields.PageName);
        }

        public static void TextMarkup1()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            // Create the Shapes that will hold the text
            var s1 = page.DrawRectangle(0, 0, 8, 8);
            var s2 = page.DrawRectangle(8, 0, 16, 8);
            var s3 = page.DrawRectangle(0, 8, 8, 16);
            var s4 = page.DrawRectangle(8, 8, 16, 16);
            var shapes = new[] { s1, s2, s3, s4 };

            // Create Text Markup XML documents for each string
            var markup_strings = new[] {text1, text2, text3, text4};
            
            // Set the Text Markup for each shape
            for (int i = 0; i < shapes.Length; i++)
            {
                var shape = shapes[i];
                var markup_string = markup_strings[i];
                var markup_dom = VA.Text.Markup.TextElement.FromXml(markup_string, true);
                markup_dom.SetShapeText(shape);
            }
        }

        public static void TextMarkup2()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();
            var s0 = page.DrawRectangle(1, 1, 4, 4);

            page.DrawRectangle(1, 1, 4, 4);

            var tokens = new[] {"The ", "Quick ", "Brown ", "Fox"};
            var e1 = new VA.Text.Markup.TextElement();
            foreach (var token in tokens)
            {
                e1.AppendText(token);
            }
            //vi.Text.Markup = e1;
        }

        public static void TextSizing()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var s0 = page.DrawRectangle(0, 0, 4, 4);

            // Alignment Box fits to accomodate text
            s0.Text = "Alignment Box fits to accomodate text";

            s0.GetCell(VA.ShapeSheet.SRCConstants.Width).Formula = "2.0";
            s0.GetCell(VA.ShapeSheet.SRCConstants.Height).Formula = "GUARD(TxtHeight)";
            s0.GetCell(VA.ShapeSheet.SRCConstants.TxtWidth).Formula = "Width*1";
            s0.GetCell(VA.ShapeSheet.SRCConstants.TxtHeight).Formula = "TEXTHEIGHT(TheText,TxtWidth)";

            // Text Scales Proportional to Shape Height
            var s1 = page.DrawRectangle(0, 4, 8, 8);
            s1.Text = "Text Scales Proportional to Shape Height";
            s0.GetCell(VA.ShapeSheet.SRCConstants.Char_Size).Formula = "Height*0.25";

            // Text scales smaller to fit more text
            var s2 = page.DrawRectangle(4, 0, 8, 4);
            s2.Text = "Text scales smaller to fit more text";
            s2.GetCell(VA.ShapeSheet.SRCConstants.Char_Size).Formula =
                "11pt * 10/SQRT(LEN(SHAPETEXT(TheText)))";
        }

        public static void FontChart()
        {
            var stencil = SampleEnvironment.Application.Documents.OpenStencil("basic_u.vss");
            var master = stencil.Masters["Rectangle"];
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            var fonts = new[] {"Segoe UI", "Calibri", "Arial"};
            var sizes = new[] {"28.0pt", "18.0pt", "14.0pt", "12.0pt", "10.0pt"};
            var fontids = fonts.Select(f => page.Document.Fonts[f].ID).ToList();

            var layout = new VA.Layout.Grid.GridLayout(sizes.Length, fonts.Length, new VA.Drawing.Size(3.0, 0.5), master);
            layout.Origin = new VA.Drawing.Point(0, VA.Pages.PageHelper.GetSize(page).Height);
            layout.CellSpacing = new VA.Drawing.Size(0.5, 0.5);
            layout.RowDirection = VA.Layout.Grid.RowDirection.TopToBottom;
            
            layout.PerformLayout();
            
            layout.Render(page);

            page.ResizeToFitContents(1.0,1.0);
            var nodes = layout.Nodes.ToList();

            var items = from fi in Enumerable.Range(0, fonts.Count())
                        from size in sizes
                        select new {font = fonts[fi], size = size, fontid = fontids[fi]};

            var update = new VA.ShapeSheet.Update.SIDSRCUpdate();

            var charcells = new VA.Text.CharacterFormatCells();
            var fmt = new VA.Format.ShapeFormatCells();
            int i = 0;
            foreach (var item in items)
            {
                var shape = nodes[i].Shape;
                shape.Text = item.font + " " + item.size;
                var shapeid = nodes[i].ShapeID;
                charcells.Size = item.size;
                charcells.Font = item.fontid;
                charcells.Apply(update, shapeid, 0);

                fmt.FillForegnd = "rgb(250,250,250)";
                fmt.LinePattern = 0;
                fmt.LineWeight = 0;
                fmt.Apply(update,shapeid);
                i++;
            }

            update.Execute(page);
        }
    }
}