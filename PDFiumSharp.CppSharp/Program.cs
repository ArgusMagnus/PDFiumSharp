using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Generators.CSharp;
using CppSharp.Types;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PDFiumSharp.CppSharp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var inputDir = args[0];
            var outputDir = args[1];
            ConsoleDriver.Run(new PDFiumLibrary(inputDir, outputDir));
            foreach (var file in Directory.EnumerateFiles(outputDir, "*.cpp", SearchOption.TopDirectoryOnly))
                File.Delete(file);
        }

        sealed class PDFiumLibrary : ILibrary
        {
            readonly string _inputDir;
            readonly string _outputDir;

            public PDFiumLibrary(string inputDir, string outputDir)
            {
                _inputDir = inputDir;
                _outputDir = outputDir;
            }

            public override void Postprocess(Driver driver, ASTContext ctx)
            {
                // Fix for generating code which will not compile.
                ctx.FindClass("FPDF_LIBRARY_CONFIG_").First().Properties.First(f => f.OriginalName == "m_pUserFontPaths").Ignore = true;
                
            }

            public override void Preprocess(Driver driver, ASTContext ctx)
            {
            }

            public override void Setup(Driver driver)
            {
                driver.Options.GeneratorKind = GeneratorKind.CSharp;
                driver.Options.OutputDir = _outputDir;
                driver.Options.CommentKind = CommentKind.C;
                driver.ParserOptions.SetupMSVC(VisualStudioVersion.Latest);
                var module = driver.Options.AddModule("PDFiumSharp");
                module.OutputNamespace = "PDFiumSharp.Native";
                module.SharedLibraryName = "pdfium";
                var includeDir = Path.Combine(_inputDir, "include");

                // Workaround: Redefine typedefs for better CppSharp output
                {
                    var path = Path.Combine(includeDir, "fpdfview.h");
                    File.WriteAllText(path, File.ReadAllText(path)
                        .Replace("typedef unsigned short FPDF_WCHAR;", "typedef wchar_t FPDF_WCHAR;")
                        .Replace("typedef const unsigned short* FPDF_WIDESTRING;", "typedef const wchar_t* FPDF_WIDESTRING;"));
                }

                module.IncludeDirs.Add(includeDir);
                module.Headers.AddRange(Directory.EnumerateFiles(includeDir, "*.h", SearchOption.AllDirectories).Select(x => x.Substring(includeDir.Length + 1)));
                var libDir = Path.Combine(_inputDir, "x86", "lib");
                module.LibraryDirs.Add(libDir);
                module.Libraries.AddRange(Directory.EnumerateFiles(libDir, "*.lib", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x)));
            }

            public override void SetupPasses(Driver driver)
            {
                //driver.AddTranslationUnitPass(new CppSharp.Passes.FunctionToInstanceMethodPass());
                //driver.AddTranslationUnitPass(new global::CppSharp.Passes.DelegatesPass());
                //driver.AddTranslationUnitPass(new global::CppSharp.Passes.MarshalPrimitivePointersAsRefTypePass());
            }

            [TypeMap("FPDF_BOOL", GeneratorKind = GeneratorKind.CSharp)]
            public sealed class BooleanTypeMap : TypeMap
            {
                public override global::CppSharp.AST.Type CSharpSignatureType(TypePrinterContext ctx)
                {
                    //return new CILType(
                    //    ctx.Kind == TypePrinterContextKind.Managed
                    //        ? typeof(bool)
                    //        : typeof(int));
                    return new CILType(typeof(bool));
                }
            }
        }
    }
}
