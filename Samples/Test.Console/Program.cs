using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFiumSharp;
using System.IO;
using System.Text.RegularExpressions;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var doc = new PdfDocument("TestDoc.pdf", "password"))
			{
                using (var page = doc.Pages.First())
                using (var textPage = page.GetTextPage())
                {
                    if (textPage.TryGetLooseCharBox(0, out var charBounds))
                    {
                        var a = charBounds;
                    }
                }

                    var viewsPerPage = new Dictionary<int, List<PdfDestination.View>>();
                foreach (var bmarkList in doc.Bookmarks)
                {
                    if (bmarkList.Title == "Betriebsmittelliste")
                    {
                        foreach (var bmark in bmarkList.Children)
                        {
                            //if (!Regex.IsMatch(bmark.Title, @"^K\d+\.?\d+:\d+$"))
                            //	continue;

                            var view = bmark.Destination.GetView();
                            if (view.Type == PdfDestination.ViewFitTypes.Unkown || view.Type == PdfDestination.ViewFitTypes.XYZ)
                                continue;
                            if (!viewsPerPage.TryGetValue(bmark.Destination.PageIndex, out var list))
                            {
                                list = new List<PdfDestination.View>();
                                viewsPerPage.Add(bmark.Destination.PageIndex, list);
                            }
                            list.Add(view);
                        }
                        break;
                    }
                }

				var dict = new Dictionary<string, List<string>>();

				foreach (var pair in viewsPerPage)
				{
					using (var page = doc.Pages[pair.Key])
					using (var textPage = page.GetTextPage())
					{
						foreach (var link in page.Links)
						{
							var dst = link.Destination;
							if (dst == null && link.Action.Type == ActionTypes.GoTo)
								dst = link.Action.Destination;
							if (dst != null)
							{
								var view = dst.GetView();
							}
						}
						Console.WriteLine($"Page {page.Index}");
						var allText = textPage.GetText();
						var symMatches = Regex.Matches(allText, @"^Sym\s*$", RegexOptions.Multiline);
						var adrMatches = Regex.Matches(allText, @"^Adr\s*$", RegexOptions.Multiline);
						foreach (var view in pair.Value)
						{
                            var boundText = textPage.GetBoundedText(new(view.Left, view.Top, view.Right, view.Bottom));
							if (!Regex.IsMatch(boundText, @"^Sym\s*$", RegexOptions.Multiline) || !Regex.IsMatch(boundText, @"^Adr\s*$", RegexOptions.Multiline))
								continue;

							CoordinatesDouble symPos = new(double.NaN, double.NaN);
							foreach (Match match in symMatches)
							{
                                if (!textPage.TryGetCharOrigin(match.Index, out var pos))
                                    continue;
								if (pos.X > view.Left && pos.X < view.Right && pos.Y > view.Top && pos.Y < view.Bottom)
								{
									symPos = pos;
									break;
								}
							}
							if (double.IsNaN(symPos.X))
								continue;

							CoordinatesDouble adrPos = new(double.NaN, double.NaN);
							foreach (Match match in adrMatches)
                            {
                                if (!textPage.TryGetCharOrigin(match.Index, out var pos))
                                    continue;
                                if (pos.X > view.Left && pos.X < view.Right && pos.Y > view.Top && pos.Y < view.Bottom)
								{
									adrPos = pos;
									break;
								}
							}
							if (double.IsNaN(adrPos.X))
								continue;

							string symbol = null;
							string address = null;
							foreach (var line in boundText.Split('\n').Select(x=>x.Trim()).Where(x=>x != "Sym" && x != "Adr"))
							{
								foreach (Match match in Regex.Matches(allText, $@"^{Regex.Escape(line)}\s*$", RegexOptions.Multiline))
                                {
                                    if (!textPage.TryGetCharOrigin(match.Index, out var pos))
                                        continue;
                                    if (pos.X > symPos.X && pos.X < view.Right && Math.Round(pos.Y) == Math.Round(symPos.Y))
										symbol = line;
									else if (pos.X > adrPos.X && pos.X < view.Right && Math.Round(pos.Y) == Math.Round(adrPos.Y))
										address = line;
									else if (symbol != null && address != null)
										break;
								}
							}

							Console.WriteLine($"\t({view.Left:F0}, {view.Top:F0}, {view.Right:F0}, {view.Bottom:F0})");
							Console.WriteLine($"\t\t{symbol} - {address}");
						}
					}

				}

				foreach (var pair in dict)
				{
					Console.WriteLine(pair.Key);
					foreach (var view in pair.Value)
						Console.WriteLine(view);
				}
			}
			Console.ReadKey();
		}

		static void PrintBookmarks(PdfBookmark bookmark, string prefix)
		{
			foreach (var child in bookmark.Children)
			{
				Console.WriteLine(prefix + child.Title);
				PrintBookmarks(child, prefix + "  ");
			}
		}
	}
}
