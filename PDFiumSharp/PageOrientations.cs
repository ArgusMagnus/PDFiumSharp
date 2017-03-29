#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFiumSharp
{
    public enum PageOrientations : int
    {
		Normal = 0,
		Rotated90CW = 1,
		Rotated180 = 2,
		Rotated90CCW = 3,
    }
}
