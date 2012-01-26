﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber
{

    /// <summary>
    /// Delegate that can accept a relative or absolute path and return the parsed component from the path
    /// </summary>
    /// <param name="path">The path to resolve and parse</param>
    /// <returns>The parsed component from the specified path</returns>
    public delegate IPDFComponent PDFReferenceResolver(string path);

    /// <summary>
    /// Event handler that is raised when the parser encounters an error.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void PDFCodeDomErrorHandler(object sender, PDFCodeDomErrorEventArgs args);

    public class PDFCodeDomErrorEventArgs : EventArgs
    {
        public IPDFCodeDomError Error { get; private set; }

        public PDFCodeDomErrorEventArgs(IPDFCodeDomError error)
        {
            this.Error = error;
        }
    }
}
