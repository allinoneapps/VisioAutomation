using System;
using System.Collections.Generic;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.Scripting.Commands
{
    public class PageCommands : CommandSet
    {
        public PageCommands(Session session) :
            base(session)
        {

        }

        public IVisio.Page Get()
        {
            if (!this.Session.HasActiveDrawing)
            {
                throw new AutomationException("No Drawing available");
            }

            var application = this.Session.VisioApplication;
            return application.ActivePage;
        }

        public VA.Drawing.Size GetSize()
        {
            if (!this.Session.HasActiveDrawing)
            {
                throw new AutomationException("No Drawing available");
            }

            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;
            return active_page.GetSize();
        }

        [Obsolete]
        public string GetName()
        {
            return Get().NameU;
        }

        public void SetName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name.Length < 1)
            {
                throw new ArgumentException("name must have at least one character");
            }

            var application = this.Session.VisioApplication;
            var active_document = application.ActiveDocument;
            var pages = active_document.Pages;
            var pagenames = new HashSet<string>(pages.GetNamesU());
            if (pagenames.Contains(name))
            {
                throw new AutomationException("Page already exists with this name");
            }

            var page = Get();
            page.NameU = name;
        }

        public IVisio.Page New(VA.Drawing.Size? size, bool isbackgroundpage)
        {
            IVisio.Page page;
            var application = this.Session.VisioApplication;
            var active_document = application.ActiveDocument;
            var pages = active_document.Pages;
            page = pages.Add();

            using (var undoscope = application.CreateUndoScope())
            {
                if (size.HasValue)
                {
                    this.Session.Write(OutputStream.Verbose,"Setting page size to {0}", size.Value);
                    page.SetSize(size.Value);
                }

                if (isbackgroundpage)
                {
                    page.Background = 1;
                }
            }

            return page;
        }

        public void SetBackgroundPage(string background_page_name)
        {
            if (background_page_name == null)
            {
                throw new ArgumentNullException("background_page_name");
            }

            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            var active_document = application.ActiveDocument;
            var pages = active_document.Pages;
            var names = new HashSet<string>(pages.GetNamesU());
            if (!names.Contains(background_page_name))
            {
                string msg = String.Format("Could not find page with name \"{0}\"", background_page_name);
                throw new AutomationException(msg);
            }

            var bgpage = pages.ItemU[background_page_name];
            var fgpage = application.ActivePage;

            using (var undoscope = application.CreateUndoScope())
            {
                VA.Pages.PageHelper.SetBackgroundPage(fgpage, bgpage);
            }
        }

        public void Duplicate()
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            using (var undoscope = application.CreateUndoScope())
            {
                VA.Pages.PageHelper.Duplicate(application.ActivePage);
            }
        }

        public void DuplicateToNewDocument()
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;
            var page_to_dupe = active_page;
            var documents = application.Documents;
            var dest_doc = documents.Add(String.Empty);
            page_to_dupe.Activate();
            string page_name = page_to_dupe.NameU;
            var destpages = dest_doc.Pages;
            var dest_page = destpages[1];
            VA.Pages.PageHelper.DuplicateToDocument(active_page, dest_doc, dest_page, page_name, true);
            dest_doc.Activate();
            dest_page.Activate();
        }

        public VA.Pages.PrintPageOrientation GetOrientation()
        {
            if (!this.Session.HasActiveDrawing)
            {
                throw new AutomationException("No active page");
            }

            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;
            return VA.Pages.PageHelper.GetOrientation(active_page);
        }

        public void SetOrientation(VA.Pages.PrintPageOrientation orientation)
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            using (var undoscope = application.CreateUndoScope())
            {
                var active_page = application.ActivePage;
                VA.Pages.PageHelper.SetOrientation(active_page, orientation);
            }
        }

        public void ResizeToFitContents(VA.Drawing.Size bordersize, bool zoom_to_page)
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            using (var undoscope = application.CreateUndoScope())
            {
                var active_page = application.ActivePage;
                active_page.ResizeToFitContents(bordersize);
                if (zoom_to_page)
                {
                    this.Session.View.Zoom(VA.Scripting.Zoom.ToPage);
                }
            }
        }

        public void ResetOrigin()
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            using (var undoscope = application.CreateUndoScope())
            {
                var active_page = application.ActivePage;
                VA.Pages.PageHelper.ResetOrigin(active_page);
            }
        }

        public void SetSize(VA.Drawing.Size new_size)
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            var application = this.Session.VisioApplication;
            using (var undoscope = application.CreateUndoScope())
            {
                var active_page = application.ActivePage;
                active_page.SetSize(new_size);
            }
        }

        public void SetSize(double? width, double? height)
        {
            if (!this.Session.HasActiveDrawing)
            {
                return;
            }

            if (!width.HasValue && !height.HasValue)
            {
                // nothing to do
                return;
            }

            var application = this.Session.VisioApplication;
            var active_page = application.ActivePage;
            var old_size = active_page.GetSize();
            var w = width.GetValueOrDefault(old_size.Width);
            var h = height.GetValueOrDefault(old_size.Height);
            var new_size = new VA.Drawing.Size(w, h);
            SetSize(new_size);
        }

        public void SetHeight(double height)
        {
            SetSize(null, height);
        }

        public void SetWidth(double width)
        {
            SetSize(width, null);
        }

        public void GoTo(Pages.PageNavigation flags)
        {
            var application = this.Session.VisioApplication;
            var active_document = application.ActiveDocument;
            var docpages = active_document.Pages;
            if (docpages.Count < 2)
            {
                return;
            }

            var pages = docpages;
            VA.Pages.PageHelper.NavigateTo(pages, flags);
        }
    }
}