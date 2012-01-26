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
using Scryber.Native;

namespace Scryber.Drawing
{
    public partial class PDFGraphics
    {
        
        public void DrawElipse(PDFPen pen, PDFRect rect)
        {
            this.DrawElipse(pen, rect.Location, rect.Size);
        }

        public void DrawElipse(PDFPen pen, PDFPoint pos, PDFSize size)
        {
            this.DrawElipse(pen, pos.X, pos.Y, size.Width, size.Height);
        }

        public void DrawElipse(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            PDFRect bounds = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);

            OutputElipsePoints(x, y, width, height);
            this.RenderCloseStrokePathOp();

            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
            
        }

        public void FillElipse(PDFBrush brush, PDFRect rect)
        {
            this.FillElipse(brush, rect.Location, rect.Size);
        }

        public void FillElipse(PDFBrush brush, PDFPoint pos, PDFSize size)
        {
            this.FillElipse(brush, pos.X, pos.Y, size.Width, size.Height);
        }

        public void FillElipse(PDFBrush brush, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            PDFRect bounds = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);

            OutputElipsePoints(x, y, width, height);
            this.RenderFillPathOp();

            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();

        }

        private void OutputElipsePoints(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            this.RenderMoveTo(left, vcentre);
            //top left quadrant
            this.RenderArcTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
            //top right quadrant
            this.RenderArcTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
            //bottom right quadrant
            this.RenderArcTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
            //bottom left quadrant
            this.RenderArcTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
        }

        public void DrawQuadrants(PDFPen pen, PDFRect rect, Quadrants sides)
        {
            this.DrawQuadrants(pen, rect.Location, rect.Size, sides);
        }

        public void DrawQuadrants(PDFPen pen, PDFPoint pos, PDFSize size, Quadrants sides)
        {
            this.DrawQuadrants(pen, pos.X, pos.Y, size.Width, size.Height, sides);
        }

        public void DrawQuadrants(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Quadrants sides)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            PDFRect bounds = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);

            OutputQuadrantPoints(x, y, width, height, sides);
            this.RenderStrokePathOp();

            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        private void OutputQuadrantPoints(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Quadrants sides)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            bool requiresmove;
            if ((sides & Quadrants.TopLeft) > 0)
            {
                this.RenderMoveTo(left, vcentre);
                //top left quadrant
                this.RenderArcTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.TopRight) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(hcentre, top);
                //top right quadrant
                this.RenderArcTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.BottomRight) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(right, vcentre);
                //bottom right quadrant
                this.RenderArcTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.BottomLeft) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(hcentre, bottom);
                //bottom left quadrant
                this.RenderArcTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
            }
        }

        public void FillQuadrants(PDFBrush brush, PDFRect rect, Quadrants sides)
        {
            this.FillQuadrants(brush, rect.Location, rect.Size, sides);
        }

        public void FillQuadrants(PDFBrush brush, PDFPoint pos, PDFSize size, Quadrants sides)
        {
            this.FillQuadrants(brush, pos.X, pos.Y, size.Width, size.Height, sides);
        }

        public void FillQuadrants(PDFBrush brush, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Quadrants sides)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            PDFRect bounds = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);

            OutputQuadrantShapes(x, y, width, height, sides);
            this.RenderFillPathOp();

            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        private void OutputQuadrantShapes(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Quadrants sides)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            if ((sides & Quadrants.TopLeft) > 0)
            {
                this.RenderMoveTo(left, vcentre);
                //top left quadrant
                this.RenderArcTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
                this.DoOutputContinuationLine(hcentre, vcentre);
                this.DoOutputContinuationLine(left, vcentre);
            }
            

            if ((sides & Quadrants.TopRight) > 0)
            {
                this.RenderMoveTo(hcentre, top);
                //top right quadrant
                this.RenderArcTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
                this.DoOutputContinuationLine(hcentre, vcentre);
                this.DoOutputContinuationLine(hcentre, top);
            }
            
            if ((sides & Quadrants.BottomRight) > 0)
            {
                this.RenderMoveTo(right, vcentre);
                //bottom right quadrant
                this.RenderArcTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
                this.DoOutputContinuationLine(hcentre, vcentre);
                this.DoOutputContinuationLine(right, vcentre);
            }

            if ((sides & Quadrants.BottomLeft) > 0)
            {
                this.RenderMoveTo(hcentre, bottom);
                //bottom left quadrant
                this.RenderArcTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
                this.DoOutputContinuationLine(hcentre, vcentre);
                this.DoOutputContinuationLine(hcentre, bottom);
            }
        }


        public void DrawCurve(PDFPoint start, PDFPoint end, PDFPoint starthandle, PDFPoint endhandle)
        {
            this.RenderMoveTo(start.X, start.Y);
            this.RenderArcTo(end.X, end.Y, starthandle.X, starthandle.Y, endhandle.X, endhandle.Y);
        }

        public void DrawContinuationCurve(PDFPoint end, PDFPoint starthandle, PDFPoint endhandle)
        {
            this.RenderArcTo(end.X, end.Y, starthandle.X, starthandle.Y, endhandle.X, endhandle.Y);
        }


        public void DrawLine(PDFUnit x1, PDFUnit y1, PDFUnit x2, PDFUnit y2)
        {
            this.RenderMoveTo(x1, y1);
            this.RenderLineTo(x2, y2);
            this.RenderStrokePathOp();
        }

        public void DrawLine(PDFPoint start, PDFPoint end)
        {
            this.RenderMoveTo(start.X, start.Y);
            this.RenderLineTo(end.X, end.Y);
            this.RenderStrokePathOp();
        }


        public void RenderPath(PDFBrush brush, PDFPen pen, PDFPoint location, PDFGraphicsPath path)
        {
            PDFRect bounds = new PDFRect(path.Bounds.X + location.X, path.Bounds.Y + location.Y, path.Bounds.Width, path.Bounds.Height);
            if(null == path)
                throw new ArgumentNullException("path");
            if (null == brush && null == pen)
                throw new ArgumentNullException("brush / pen", "You must provide a brush, or a pen or both when rendering a path");
            else if (null != brush)
                brush.SetUpGraphics(this, bounds);
            if (null != pen)
                pen.SetUpGraphics(this, bounds);

            foreach (Path p in path.SubPaths)
            {
                RenderPathData(location, p);
            }

            if (null != brush && null != pen)
                this.RenderFillAndStrokePathOp(path.Mode == GraphicFillMode.EvenOdd);
            else if (null != brush)
                this.RenderFillPathOp(path.Mode == GraphicFillMode.EvenOdd);
            else if (null != pen)
                this.RenderStrokePathOp();

            if (null != brush)
                brush.ReleaseGraphics(this, bounds);
            if (null != pen)
                pen.ReleaseGraphics(this, bounds);
        }

        private void RenderPathData(PDFPoint location, Path p)
        {
            if (null == p)
                return;
            foreach (PathData data in p.Operations)
            {
                this.RenderPathOp(data, location);
            }

            if (p.Closed)
                RenderCloseStrokePathOp();
        }

        private void RenderPathOp(PathData data, PDFPoint location)
        {
            switch (data.Type)
            {
                case PathDataType.Move:
                    PathMoveData move = (PathMoveData)data;
                    this.RenderMoveTo(move.MoveTo.X + location.X, move.MoveTo.Y + location.Y);
                    break;
                case PathDataType.Line:
                    PathLineData line = (PathLineData)data;
                    this.RenderLineTo(line.LineTo.X + location.X, line.LineTo.Y + location.Y);
                    break;
                case PathDataType.Rect:
                    PathRectData rect = (PathRectData)data;
                    this.RenderRectangle(rect.Rect.X + location.X, rect.Rect.Y + location.Y, rect.Rect.Width, rect.Rect.Height);
                    break;
                case PathDataType.SubPath:
                    PathSubPathData sub = (PathSubPathData)data;
                    this.RenderPathData(location, sub.InnerPath);
                    break;
                case PathDataType.Bezier:
                    PathBezierCurveData bez = (PathBezierCurveData)data;
                    if (bez.HasStartHandle && bez.HasEndHandle)
                    {
                        this.RenderArcTo(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.StartHandle.X + location.X, bez.StartHandle.Y + location.Y,
                                         bez.EndHandle.X + location.X, bez.EndHandle.Y + location.Y);
                    }
                    else if (bez.HasStartHandle)
                    {
                        this.RenderArcToWithStartHandleOnly(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.StartHandle.X + location.X, bez.StartHandle.Y + location.Y);
                    }
                    else if (bez.HasEndHandle)
                    {
                        this.RenderArcToWithEndHandleOnly(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.EndHandle.X + location.X, bez.EndHandle.Y + location.Y);
                    }
                    else
                        this.RenderLineTo(bez.Points[2].X, bez.Points[2].Y);
                    break;
                case PathDataType.Close:
                    this.RenderClosePathOp();
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException("data.Type");

            }
        }
        
        
    }
}
