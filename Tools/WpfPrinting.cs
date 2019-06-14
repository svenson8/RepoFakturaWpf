using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Controls.Primitives;
using System.Windows.Xps.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace FakturaWpf.Tools
{
    public class WpfPrinting
    {
        public const double cm = 37;
        public double Margin = 0.5 * cm;
        public double PageWidth = 21 * cm;
        public double PageHeight = 29 * cm;
        public double PageWidthLand = 29 * cm;
        public double PageHeightLand = 21 * cm;
        public double RowHeight = 0.7 * cm;
        public bool PageNumberVisibility = true;
        public bool DateVisibility = true;
        public double FontSize = 14;
        public double HeaderFontSize = 14;
        public bool IsBold = false;
        public static bool Landscape;
        public static bool myfitToPage;
        public void PrintDataGrid(FrameworkElement header, DataGrid grid, FrameworkElement footer, PrintDialog printDialog, bool land, bool print, bool fitToPage)
        {
            if (header == null) { header = new FrameworkElement(); header.Width = 1; header.Height = 1; }
            if (footer == null) { footer = new FrameworkElement(); footer.Width = 1; footer.Height = 1; }
            if (grid == null) return;

            myfitToPage = fitToPage;
            Size pageSize;
            if (land == true)
            {
                Landscape = true;
                pageSize = new Size(PageWidthLand, PageHeightLand);
            }
            else
            {
                Landscape = false;
                pageSize = new Size(PageWidth, PageHeight);
            }



            FixedDocument fixedDoc = new FixedDocument();
            fixedDoc.DocumentPaginator.PageSize = pageSize;

            double GridActualWidth = grid.ActualWidth == 0 ? grid.Width : grid.ActualWidth;

            double PageWidthWithMargin = pageSize.Width - Margin * 2;
            double PageHeightWithMargin = pageSize.Height - Margin * 2;



            // scale the header  
            double headerScale = (header?.Width ?? 0) / PageWidthWithMargin;
            double headerWidth = PageWidthWithMargin;
            double headerHeight = (header?.Height ?? 0) * headerScale;
            header.Height = headerHeight;
            header.Width = headerWidth;
            // scale the footer  
            double footerScale = (footer?.Width ?? 0) / PageWidthWithMargin;
            double footerWidth = PageWidthWithMargin;
            double footerHeight = (footer?.Height ?? 0) * footerScale;
            footer.Height = footerHeight;
            footer.Width = footerWidth;

            int pageNumber = 1;
            string Now = DateTime.Now.ToShortDateString();

            //add the header 
            FixedPage fixedPage = new FixedPage();
            fixedPage.Background = Brushes.White;
            fixedPage.Width = pageSize.Width;
            fixedPage.Height = pageSize.Height;

            FixedPage.SetTop(header, Margin);
            FixedPage.SetLeft(header, Margin);

            fixedPage.Children.Add(header);
            // its like cursor for current page Height to start add grid rows
            double CurrentPageHeight = headerHeight + 1 * cm;
            int lastRowIndex = 0;
            bool IsFooterAdded = false;

            for (; ; )
            {
                int AvaliableRowNumber;


                var SpaceNeededForRestRows = (CurrentPageHeight + (grid.Items.Count - lastRowIndex) * RowHeight);

                //To avoid printing the footer in a separate page
                if (SpaceNeededForRestRows > (pageSize.Height - footerHeight - Margin) && (SpaceNeededForRestRows < (pageSize.Height - Margin)))
                    AvaliableRowNumber = (int)((pageSize.Height - CurrentPageHeight - Margin - footerHeight) / RowHeight);
                // calc the Avaliable Row acording to CurrentPageHeight
                else AvaliableRowNumber = (int)((pageSize.Height - CurrentPageHeight - Margin) / RowHeight);

                // create new page except first page cause we created it prev
                if (pageNumber > 1)
                {
                    fixedPage = new FixedPage();
                    fixedPage.Background = Brushes.White;
                    fixedPage.Width = pageSize.Width;
                    fixedPage.Height = pageSize.Height;
                }

                // create new data grid with  columns width and binding
                DataGrid gridToAdd;
                gridToAdd = GetDataGrid(grid, GridActualWidth, PageWidthWithMargin);


                FixedPage.SetTop(gridToAdd, CurrentPageHeight); // top margin
                FixedPage.SetLeft(gridToAdd, Margin); // left margin

                // add the avaliable rows to the cuurent grid 
                for (int i = lastRowIndex; i < grid.Items.Count && i < AvaliableRowNumber + lastRowIndex; i++)
                {
                    gridToAdd.Items.Add(grid.Items[i]);
                }
                lastRowIndex += gridToAdd.Items.Count + 1;

                // add date
                TextBlock dateText = new TextBlock();
                if (DateVisibility) dateText.Visibility = Visibility.Visible;
                else dateText.Visibility = Visibility.Hidden;
                dateText.Text = Now;

                // add page number
                TextBlock PageNumberText = new TextBlock();
                if (PageNumberVisibility) PageNumberText.Visibility = Visibility.Visible;
                else PageNumberText.Visibility = Visibility.Hidden;
                PageNumberText.Text = "Page : " + pageNumber;

                FixedPage.SetTop(dateText, PageHeightWithMargin);
                FixedPage.SetLeft(dateText, Margin);

                FixedPage.SetTop(PageNumberText, PageHeightWithMargin);
                FixedPage.SetLeft(PageNumberText, PageWidthWithMargin - PageNumberText.Text.Length * 10);

                fixedPage.Children.Add(gridToAdd);
                fixedPage.Children.Add(dateText);
                fixedPage.Children.Add(PageNumberText);

                // calc Current Page Height to know the rest Height of this page
                CurrentPageHeight += gridToAdd.Items.Count * RowHeight;

                // all grid rows added
                if (lastRowIndex >= grid.Items.Count)
                {
                    // if footer have space it will be added to the same page
                    if (footerHeight < (PageHeightWithMargin - CurrentPageHeight))
                    {
                        FixedPage.SetTop(footer, CurrentPageHeight + Margin);
                        FixedPage.SetLeft(footer, Margin);

                        fixedPage.Children.Add(footer);
                        IsFooterAdded = true;
                    }
                }

                fixedPage.Measure(pageSize);
                fixedPage.Arrange(new Rect(new Point(), pageSize));
                fixedPage.UpdateLayout();

                PageContent pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);

                pageNumber++;
                // go to start position : New page Top
                CurrentPageHeight = Margin;

                // this mean that lastRowIndex >= grid.Items.Count  and the footer dont have enough space
                if (lastRowIndex >= grid.Items.Count && !IsFooterAdded)
                {
                    FixedPage ffixedPage = new FixedPage();

                    ffixedPage.Background = Brushes.White;
                    ffixedPage.Width = pageSize.Width;
                    ffixedPage.Height = pageSize.Height;

                    FixedPage.SetTop(footer, Margin);
                    FixedPage.SetLeft(footer, Margin);

                    TextBlock fdateText = new TextBlock();
                    if (DateVisibility) fdateText.Visibility = Visibility.Visible;
                    else fdateText.Visibility = Visibility.Hidden;
                    dateText.Text = Now;

                    TextBlock fPageNumberText = new TextBlock();
                    if (PageNumberVisibility) fPageNumberText.Visibility = Visibility.Visible;
                    else fPageNumberText.Visibility = Visibility.Hidden;
                    fPageNumberText.Text = "Page : " + pageNumber;

                    FixedPage.SetTop(fdateText, PageHeightWithMargin);
                    FixedPage.SetLeft(fdateText, Margin);

                    FixedPage.SetTop(fPageNumberText, PageHeightWithMargin);
                    FixedPage.SetLeft(fPageNumberText, PageWidthWithMargin - PageNumberText.ActualWidth);

                    ffixedPage.Children.Add(footer);
                    ffixedPage.Children.Add(fdateText);
                    ffixedPage.Children.Add(fPageNumberText);

                    ffixedPage.Measure(pageSize);
                    ffixedPage.Arrange(new Rect(new Point(), pageSize));
                    ffixedPage.UpdateLayout();



                    PageContent fpageContent = new PageContent();
                    ((IAddChild)fpageContent).AddChild(ffixedPage);
                    fixedDoc.Pages.Add(fpageContent);
                    IsFooterAdded = true;
                }

                if (IsFooterAdded)
                {
                    break;
                }
            }


            if (print == true)
            {
                if (land == true)
                    printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                if (printDialog.ShowDialog() == true)
                    PrintFixedDocument(fixedDoc, printDialog);
            }
            else
            {
                printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                fixedDoc.PrintTicket = printDialog.PrintTicket;
                ShowPrintPreview(fixedDoc);
            }

        }
        private DataGrid GetDataGrid(DataGrid grid, double GridActualWidth, double PageWidthWithMargin)
        {
            DataGrid printed = new DataGrid();


            // styling the grid
            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.White));
            rowStyle.Setters.Add(new Setter(Control.FontSizeProperty, FontSize));
            if (IsBold) rowStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
            rowStyle.Setters.Add(new Setter(Control.HeightProperty, RowHeight));

            printed.RowStyle = rowStyle;

            printed.VerticalGridLinesBrush = Brushes.Black;
            printed.HorizontalGridLinesBrush = Brushes.Black;
            printed.FontFamily = new FontFamily("Arial");

            printed.RowBackground = Brushes.White;
            printed.Background = Brushes.White;
            printed.Foreground = Brushes.Black;
            // get the columns of grid 
            foreach (var column in grid.Columns)
            {
                if (column is DataGridCheckBoxColumn) continue;
                if (column.Visibility != Visibility.Visible) continue;
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = column.Header;
                if (myfitToPage == true)
                    textColumn.Width = column.ActualWidth / GridActualWidth * PageWidthWithMargin;
                else
                    textColumn.Width = column.ActualWidth;

                textColumn.CellStyle = GetCellStyle(column);
                textColumn.HeaderStyle = GetColumnStyle(column);
                textColumn.Binding = ((DataGridTextColumn)column).Binding;
                printed.Columns.Add(textColumn);
            }
            printed.BorderBrush = Brushes.Black;

            printed.RowHeaderWidth = 0;

            return printed;
        }

        private Style GetCellStyle(DataGridColumn column)
        {
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, GetCellAlignment(column)));
            return style;
        }

        private Style GetColumnStyle(DataGridColumn column)
        {
            Style columnStyle = new Style(typeof(DataGridColumnHeader));
            columnStyle.Setters.Add(new Setter(Control.FontSizeProperty, HeaderFontSize));
            columnStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, GetHeaderAlignment(column)));
            columnStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(1, 1, 1, 1)));
            columnStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.White));
            columnStyle.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.Black));
            columnStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.SemiBold));
            return columnStyle;
        }

        private HorizontalAlignment GetHeaderAlignment(DataGridColumn column)
        {
            HorizontalAlignment dd = HorizontalAlignment.Left;
            foreach (Setter s in column.CellStyle.Setters)
            {
                if (s.Property.Name == "TextAlignment")
                {
                    if (s.Value is TextAlignment.Left)
                        dd = HorizontalAlignment.Left;

                    if (s.Value is TextAlignment.Center)
                        dd = HorizontalAlignment.Center;

                    if (s.Value is TextAlignment.Right)
                        dd = HorizontalAlignment.Right;

                    return dd;
                }
            }
            return HorizontalAlignment.Left;
        }

        private TextAlignment GetCellAlignment(DataGridColumn column)
        {
            TextAlignment dd;
            foreach (Setter s in column.CellStyle.Setters)
            {
                if (s.Property.Name == "TextAlignment")
                {
                    dd = (TextAlignment)s.Value;
                    return dd;
                }
            }
            return TextAlignment.Left;
        }

        public void PrintFixedDocument(FixedDocument fixedDoc, PrintDialog printDialog)
        {
            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printDialog.PrintQueue);
            writer.Write(fixedDoc, printDialog.PrintTicket);
        }

        public void ShowPrintPreview(FixedDocument fixedDoc)
        {
            Window wnd = new Window();
            MyDocumentViewer viewer = new MyDocumentViewer();
           
            viewer.Document = fixedDoc;            
            wnd.Content = viewer;
            wnd.ShowDialog();
        }

        public class MyDocumentViewer : DocumentViewer
        {
            protected override void OnPrintCommand()
            {
                PrintDialog dialog = new PrintDialog();
                dialog.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                dialog.PrintTicket = dialog.PrintQueue.DefaultPrintTicket;
                if (Landscape == true)
                  dialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

                if (dialog.ShowDialog() == true)
                {
                    XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(dialog.PrintQueue);
                    writer.WriteAsync(this.Document as FixedDocument, dialog.PrintTicket);

                }

            }
        }
    }   
}
