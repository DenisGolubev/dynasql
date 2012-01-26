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

        #region DrawRectangle(PDFRectangle) + 3 overloads

        public void DrawRectangle(PDFPen pen, PDFRect rect)
        {
            this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(PDFPen pen, PDFRect rect, Sides sides)
        {
            this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, sides);
        }

        
        public void DrawRectangle(PDFPen pen, PDFPoint location, PDFSize size)
        {
            this.DrawRectangle(pen, location.X, location.Y, size.Width, size.Height);
        }

        public void DrawRectangle(PDFPen pen, PDFPoint location, PDFSize size, Sides sides)
        {
            this.DrawRectangle(pen, location.X, location.Y, size.Width, size.Height, sides);
        }

        #endregion

        

        #region DrawRectangle(X,Y,Width,Height)

        public void DrawRectangle(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            this.SaveGraphicsState();
            PDFRect rect = new PDFRect(x, y, width, height);
            pen.SetUpGraphics(this, rect);
            this.RenderRectangle(x, y, width, height);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, rect);
            this.RestoreGraphicsState();
        }

        public void DrawRectangle(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Sides sides)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            PDFRect rect = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, rect);

            //check to see if we are outputting all sides
            if (sides == (Sides.Top | Sides.Right | Sides.Left | Sides.Bottom))
                this.RenderRectangle(x, y, width, height);
            else
            {
                bool recalc = true; //flag to identifiy if the last op moved the cursor to the next correct position
                
                if ((sides & Sides.Top) > 0)
                {
                    this.DoOutputLine(x, y, x + width, y);
                    recalc = false;
                }
                else
                    recalc = true;

                if ((sides & Sides.Right) > 0)
                {
                    if (recalc == false)
                    {
                        this.DoOutputContinuationLine(x + width, y + height);
                    }
                    else
                    {
                        this.DoOutputLine(x + width, y, x + width, y + height);
                    }
                    recalc = false;
                }
                else
                    recalc = true;

                if ((sides & Sides.Bottom) > 0)
                {
                    if (recalc == false)
                        this.DoOutputContinuationLine(x, y + height);
                    else
                        this.DoOutputLine(x + width, y + height, x, y + height);
                    recalc = false;
                }
                else
                    recalc = true;

                if ((sides & Sides.Left) > 0)
                {
                    if (recalc == false)
                        this.DoOutputContinuationLine(x, y);
                    else
                        this.DoOutputLine(x, y + height, x, y);
                }
            }
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, rect);

            this.RestoreGraphicsState();
        }

        #endregion

        #region FillRectangle(PDFBrush brush, PDFRectangle) + 2 overloads

        public void FillRectangle(PDFBrush brush, PDFRect rect)
        {
            this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillRectangle(PDFBrush brush, PDFPoint location, PDFSize size)
        {
            this.FillRectangle(brush, location.X, location.Y, size.Width, size.Height);
        }

        public void FillRectangle(PDFBrush brush, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            this.SaveGraphicsState();
            PDFRect bounds = new PDFRect(x, y, width, height);
            brush.SetUpGraphics(this, bounds);
            
            this.RenderRectangle(x, y, width, height);
            this.RenderFillPathOp();
            
            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        #endregion

        #region DrawRoundRectangle()

        public void DrawRoundRectangle(PDFPen pen, PDFRect rect, PDFUnit cornerRadius)
        {
            this.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, PDFPoint pos, PDFSize size, PDFUnit cornerRadius)
        {
            this.DrawRoundRectangle(pen, pos.X, pos.Y, size.Width, size.Height, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit cornerRadius)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            this.SaveGraphicsState();
            PDFRect bounds = new PDFRect(x, y, width, height);

            pen.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangle(x, y, width, height, cornerRadius);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        public void DrawRoundRectangle(PDFPen pen, PDFRect rect, Sides sides, PDFUnit cornerRadius)
        {
            this.DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, sides, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, PDFPoint pos, PDFSize size, Sides sides, PDFUnit cornerRadius)
        {
            this.DrawRoundRectangle(pen, pos.X, pos.Y, size.Width, size.Height, sides, cornerRadius);
        }

        public void DrawRoundRectangle(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Sides sides, PDFUnit cornerRadius)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            PDFRect bounds = new PDFRect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangleWithSidesPath(x, y, width, height, cornerRadius, sides);
            this.RenderStrokePathOp();
            pen.ReleaseGraphics(this, bounds);
            this.RestoreGraphicsState();
        }

        #endregion

        #region FillRoundRectangle()

        public void FillRoundRectangle(PDFBrush brush, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit cornerRadius)
        {
            PDFRect rect = new PDFRect(x, y, width, height);
            this.FillRoundRectangle(brush, rect, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, PDFPoint pos, PDFSize size, PDFUnit cornerRadius)
        {
            PDFRect r = new PDFRect(pos, size);
            this.FillRoundRectangle(brush, r, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, PDFRect rect, PDFUnit cornerRadius)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            this.SaveGraphicsState();
            //PDFRect bounds = new PDFRect(x, y, width, height);
            
            brush.SetUpGraphics(this, rect);
            this.DoOutputRoundRectangle(rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);
            this.RenderFillPathOp();
            brush.ReleaseGraphics(this, rect);

            this.RestoreGraphicsState();
        }

        public void FillRoundRectangle(PDFBrush brush, PDFRect rect, Sides sides, PDFUnit cornerRadius)
        {
            this.FillRoundRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, sides, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, PDFPoint pos, PDFSize size, Sides sides, PDFUnit cornerRadius)
        {
            this.FillRoundRectangle(brush, pos.X, pos.Y, size.Width, size.Height, sides, cornerRadius);
        }

        public void FillRoundRectangle(PDFBrush brush, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, Sides sides, PDFUnit cornerRadius)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            PDFRect bounds = new PDFRect(x, y, width, height);
            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);
            this.DoOutputRoundRectangleWithSidesFill(x, y, width, height, cornerRadius, sides);
            this.RenderFillPathOp();
            brush.ReleaseGraphics(this, bounds);
            this.RestoreGraphicsState();
        }

        #endregion

        private void DoOutputRoundRectangle(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit cornerRadius)
        {
            PDFUnit handleoffset = (PDFUnit)(cornerRadius.PointsValue * CircularityFactor);
            this.RenderMoveTo(x, y + height - cornerRadius);
            //left vertical
            this.RenderLineTo(x, y + cornerRadius);
            //topleft arc
            this.RenderArcTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
            //top horizontal
            this.RenderLineTo(x + width - cornerRadius, y);
            //topright arc
            this.RenderArcTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
            //right vertical
            this.RenderLineTo(x + width, y + height - cornerRadius);
            //bottomright arc
            this.RenderArcTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
            //bottom line
            this.RenderLineTo(x + cornerRadius, y + height);
            //bottom left arc
            this.RenderArcTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);
        }

        private void DoOutputRoundRectangleWithSidesFill(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit cornerRadius, Sides sides)
        {
            PDFUnit handleoffset = (PDFUnit)(cornerRadius.PointsValue * CircularityFactor);
            
            //position cursor at left bottom
            if ((sides & Sides.Left) > 0 && (sides & Sides.Bottom) > 0)
            {
                //if we have a bottom edge then we are going to have an arc
                this.RenderMoveTo(x, y + height - cornerRadius);
            }
            else
                this.RenderMoveTo(x, y + height);

            //draw left vertical and optional arc
            if ((sides & Sides.Left) > 0 && (sides & Sides.Top) > 0)
            {
                //as we have a left and a top then we have an arc
                this.RenderLineTo(x, y + cornerRadius);
                //top left arc
                this.RenderArcTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
            }
            else   
                this.RenderLineTo(x, y);

            //draw top horizontal and optional arc
            if ((sides & Sides.Top) > 0 && (sides & Sides.Right) > 0)
            {
                //as we have a top and a right then we have an arc
                this.RenderLineTo(x + width - cornerRadius, y);
                //top right arc
                this.RenderArcTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
            }
            else
                this.RenderLineTo(x + width, y);


            //draw right vertical and optional arc
            if ((sides & Sides.Right) > 0 && (sides & Sides.Bottom) > 0)
            {
                //right vertical
                this.RenderLineTo(x + width, y + height - cornerRadius);
                //bottomright arc
                this.RenderArcTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
            }
            else
                this.RenderLineTo(x + width, y + height);

            //draw bottom horizontal and optional arc
            if ((sides & Sides.Bottom) > 0 && (sides & Sides.Left) > 0)
            {
                //bottom line
                this.RenderLineTo(x + cornerRadius, y + height);
                //bottom left arc
                this.RenderArcTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);
            }
            else
                this.RenderLineTo(x, y + height);
            
        }

        private void DoOutputRoundRectangleWithSidesPath(PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit cornerRadius, Sides sides)
        {
            PDFUnit handleoffset = (PDFUnit)(cornerRadius.PointsValue * CircularityFactor);
            
            bool requiresmove = false;

            if ((sides & Sides.Left) > 0)
            {
                if ((sides & Sides.Bottom) == 0)
                    this.RenderMoveTo(x, y + height);
                else
                    this.RenderMoveTo(x, y + height - cornerRadius);

                //left vertical
                if ((sides & Sides.Top) == 0) //no top line so extend full height
                    this.RenderLineTo(x,y);
                else
                    this.RenderLineTo(x, y + cornerRadius);
            }
            else
                requiresmove = true;

            if ((sides & Sides.Top) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Left) == 0)//no left side
                        RenderMoveTo(x, y);
                    else
                        RenderMoveTo(x + cornerRadius, y);
                }
                else
                {
                    //topleft arc
                    this.RenderArcTo(x + cornerRadius, y, x, y + cornerRadius - handleoffset, x + cornerRadius - handleoffset, y);
                }

                //top horizontal
                if ((sides & Sides.Right) == 0)
                    this.RenderLineTo(x + width, y);//no right line so extend full width
                else
                    this.RenderLineTo(x + width - cornerRadius, y);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Sides.Right) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Top) == 0)//no top side 
                        RenderMoveTo(x + width, y);//go to the top right corner
                    else
                        RenderMoveTo(x + width, y + cornerRadius);
                }
                else
                {
                    //topright arc
                    this.RenderArcTo(x + width, y + cornerRadius, x + width - cornerRadius + handleoffset, y, x + width, y + cornerRadius - handleoffset);
                }
                
                //right vertical
                if ((sides & Sides.Bottom) == 0)
                    this.RenderLineTo(x + width, y + height);//no bottom line so extend to full height
                else
                    this.RenderLineTo(x + width, y + height - cornerRadius);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Sides.Bottom) > 0)
            {
                if (requiresmove)
                {
                    if ((sides & Sides.Right) == 0)//no right side
                        RenderMoveTo(x + width, y + height);//go to the bottom left corner
                    else
                        RenderMoveTo(x + width, y + height - cornerRadius);
                }
                else
                {
                    //bottomright arc
                    this.RenderArcTo(x + width - cornerRadius, y + height, x + width, y + height - cornerRadius + handleoffset, x + width - cornerRadius + handleoffset, y + height);
                }
                //bottom line
                if ((sides & Sides.Left) == 0)
                    this.RenderLineTo(x, y + height);
                else
                    this.RenderLineTo(x + cornerRadius, y + height);

                //if we have the left and bottom sides we need to connect them
                if ((sides & Sides.Left) > 0)
                {
                    //bottom left arc
                    this.RenderArcTo(x, y + height - cornerRadius, x + cornerRadius - handleoffset, y + height, x, y + height - cornerRadius + handleoffset);

                }
            }
            else
                requiresmove = true;

            
        }
        
    }
}
