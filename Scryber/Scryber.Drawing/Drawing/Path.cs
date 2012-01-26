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
using System.Text;

namespace Scryber.Drawing
{
    #region private class Path

    internal class Path
    {
        private bool _closed;
        private List<PathData> _data;

        internal bool Closed
        {
            get { return _closed; }
            set { _closed = value; }
        }

        internal IEnumerable<PathData> Operations
        {
            get { return _data; }
        }

        internal Path()
        {
            this._closed = false;
            this._data = new List<PathData>();
        }

        /// <summary>
        /// Adds a new path data operation to this path
        /// </summary>
        /// <param name="data"></param>
        internal void Add(PathData data)
        {
            _data.Add(data);
        }

        /// <summary>
        /// Gets the number of path data ops in this path
        /// </summary>
        internal int Count
        {
            get { return _data.Count; }
        }

        /// <summary>
        /// Gets the total number of path data ops (including sub paths)
        /// </summary>
        internal int TotalCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    PathData data = _data[i];
                    if (data.Type == PathDataType.SubPath)
                    {
                        count = ((PathSubPathData)data).InnerPath.TotalCount;
                    }
                    else
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Removes the last path data from this Path
        /// </summary>
        internal void Remove()
        {
            this._data.RemoveAt(this._data.Count - 1);
        }
    }

    #endregion
}
