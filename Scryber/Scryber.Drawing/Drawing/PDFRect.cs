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
    public struct PDFRect : IEquatable<PDFRect>, IComparable<PDFRect>, ICloneable
    {

        public static PDFRect Empty
        {
            get { return new PDFRect(); }
        }


        private PDFUnit _x;

        public PDFUnit X
        {
            get { return _x; }
            set { _x = value; }
        }

        private PDFUnit _y;

        public PDFUnit Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private PDFUnit _w;

        public PDFUnit Width
        {
            get { return _w; }
            set { _w = value; }
        }

        private PDFUnit _h;

        public PDFUnit Height
        {
            get { return _h; }
            set { _h = value; }
        }

        public PDFPoint Location
        {
            get { return new PDFPoint(this.X, this.Y); }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public PDFSize Size
        {
            get { return new PDFSize(this.Width, this.Height); }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.Equals(PDFRect.Empty);
            }
        }

        public PDFRect(double x, double y, double width, double height)
            : this((PDFUnit)x, (PDFUnit)y, (PDFUnit)width, (PDFUnit)height)
        {
        }

        public PDFRect(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            this._x = x;
            this._y = y;
            this._w = width;
            this._h = height;
        }

        public PDFRect(PDFPoint location, PDFSize size)
        {
            this._x = location.X;
            this._y = location.Y;
            this._w = size.Width;
            this._h = size.Height;
        }

        public override bool Equals(object obj)
        {
            if ((obj is PDFRect) == false)
                return false;
            else
                return Equals((PDFRect)obj);
        }

        public bool Equals(PDFRect rect)
        {
            return Equal(this, rect);
        }

        public static bool Equal(PDFRect one, PDFRect two)
        {
            return (one.X == two.X) && (one.Y == two.Y) && (one.Width == two.Width) && (one.Height == two.Height);
        }

        public static bool operator ==(PDFRect left, PDFRect right)
        {
            return Equal(left, right);
        }

        public static bool operator !=(PDFRect left, PDFRect right)
        {
            return Equal(left, right) == false;
        }

        public override int GetHashCode()
        {
            int x = this.X.GetHashCode();
            int y = this.Y.GetHashCode();
            int w = this.Width.GetHashCode();
            int h = this.Height.GetHashCode();

            return (((x ^ ((y << 13) | (y >> 0x13))) ^ ((w << 0x1a) | (w >> 6))) ^ ((h << 7) | (h >> 0x19)));
        }

        public bool Contains(PDFUnit x, PDFUnit y)
        {
            return (this.X <= x) && (x < (this.X + this.Width)) && (this.Y <= y) && (y < (this.Y + this.Height));
        }

        public bool Contains(PDFPoint point)
        {
            return this.Contains(point.X, point.Y);
        }

        public void Inflate(PDFUnit width, PDFUnit height)
        {
            this.X -= width;
            this.Y -= height;
            this.Width += new PDFUnit(2.0 * width.ToPoints().Value, PageUnits.Points);
            this.Height += new PDFUnit(2.0 * height.ToPoints().Value, PageUnits.Points);
        }

        /// <summary>
        /// Insets this rectangle by the specified thickness. Offsetting the location by Top and Left values
        /// and reducing the size by all values
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public PDFRect Inset(PDFThickness thickness)
        {
            PDFRect r = this.Clone();
            r.X += thickness.Left;
            r.Y += thickness.Top;

            r.Width -= thickness.Left + thickness.Right;
            r.Height -= thickness.Top + thickness.Bottom;

            return r;
        }

        /// <summary>
        /// Outsets this rectangle by adding the thickness values and subtacting from the location
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public PDFRect Outset(PDFThickness thickness)
        {
            PDFRect r = this.Clone();
            r.X -= thickness.Left;
            r.Y -= thickness.Top;

            r.Width += thickness.Left + thickness.Right;
            r.Height += thickness.Top + thickness.Bottom;

            return r;
        }

        public void Inflate(PDFSize size)
        {
            this.Inflate(size.Width, size.Height);
        }

        public static PDFRect Inflate(PDFRect rect, PDFUnit x, PDFUnit y)
        {
            PDFRect rect2 = rect.Clone();
            rect2.Inflate(x, y);
            return rect2;
        }

        public void Intersect(PDFRect rect)
        {
            PDFRect rect2 = Intersect(rect, this);
            this.X = rect2.X;
            this.Y = rect2.Y;
            this.Width = rect2.Width;
            this.Height = rect2.Height;
        }

        public bool IntersectsWith(PDFRect rect)
        {
            return ((((rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width))) && (rect.Y < (this.Y + this.Height))) && (this.Y < (rect.Y + rect.Height)));
        }

        public static PDFRect Intersect(PDFRect a, PDFRect b)
        {
            PDFUnit x1 = PDFUnit.Max(a.X, b.X);
            PDFUnit x2 = PDFUnit.Min(a.X + a.Width, b.X + b.Width);
            PDFUnit y1 = PDFUnit.Max(a.Y, b.Y);
            PDFUnit y2 = PDFUnit.Min(a.Y + a.Height, b.Y + b.Height);

            if ((x2 >= x1) && (y2 >= y1))
                return new PDFRect(x1, y1, x2 - x1, y2 - y1);
            else
                return PDFRect.Empty;
        }

        public static PDFRect Union(PDFRect a, PDFRect b)
        {
            PDFUnit x = PDFUnit.Min(a.X, b.X);
            PDFUnit x2 = PDFUnit.Max(a.X + a.Width, b.X + b.Width);
            PDFUnit y = PDFUnit.Min(a.Y, b.Y);
            PDFUnit y2 = PDFUnit.Max(a.Y + a.Height, b.Y + b.Height);

            return new PDFRect(x, y, x2 - x, y2 - y);
        }

        public void Offset(PDFUnit x, PDFUnit y)
        {
            this.X += x;
            this.Y += y;
        }

        public void Offset(PDFPoint pt)
        {
            this.Offset(pt.X, pt.Y);
        }

        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + ", " + this.Width.ToString() + ", " + this.Height.ToString() + "]";
        }


        #region IComparable<PDFRectangle> Members

        public int CompareTo(PDFRect other)
        {
            int i = this.Location.CompareTo(other.Location);
            if (i == 0)
                i = this.Size.CompareTo(other.Size);
            return i;
        }

        #endregion

        #region ICloneable<PDFRectangle> Members

        public PDFRect Clone()
        {
            return (PDFRect)this.MemberwiseClone();
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

       
    }
}
