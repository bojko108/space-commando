using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Import
{
    /// <summary>
    /// Class for storing map data bounds. Used to reduce map 
    /// coordinates to the map's center
    /// </summary>
    public class MapBounds
    {
        /// <summary>
        /// Lower left corner of the bounds
        /// </summary>
        private Vertex lowerLeft;
        /// <summary>
        /// Upper right corner of the bounds
        /// </summary>
        private Vertex upperRight;

        /// <summary>
        /// Lower left corner of the bounds
        /// </summary>
        public Vertex LowerLeft
        {
            get { return this.lowerLeft; }
            set
            {
                this.lowerLeft = value;
                this.calculateCenter();
            }
        }
        /// <summary>
        /// Upper right corner of the bounds
        /// </summary>
        public Vertex UpperRight
        {
            get { return this.upperRight; }
            set
            {
                this.upperRight = value;
                this.calculateCenter();
            }
        }
        /// <summary>
        /// Center of the map bounds
        /// </summary>
        public Vertex Center { get; private set; }

        /// <summary>
        /// Creates a new isntance of MapBounds
        /// </summary>
        /// <param name="lowerLeft">lower left bound</param>
        /// <param name="upperRight">upper right bound</param>
        public MapBounds(Vertex lowerLeft, Vertex upperRight)
        {
            this.lowerLeft = lowerLeft;
            this.upperRight = upperRight;

            // calculate map's center
            this.calculateCenter();
        }

        /// <summary>
        /// Creates a new MapBounds object with empty boundary
        /// </summary>
        public MapBounds()
        {
            this.LowerLeft = new Vertex(0, 0, 0);
            this.UpperRight = new Vertex(0, 0, 0);

            // calculate map's center
            this.calculateCenter();
        }
        /// <summary>
        /// Sets map bounds scale
        /// </summary>
        /// <param name="scale">use 1 for scale 1:1</param>
        public void SetScale(float scale = 1)
        {
            this.LowerLeft = this.LowerLeft.Scale(scale);
            this.UpperRight = this.UpperRight.Scale(scale);
        }



        public void ProjectToGeographic()
        {

        }

        public void ProjectToWebMercator()
        {
            this.LowerLeft = this.LowerLeft.ProjectToWebMercator();
            this.UpperRight = this.UpperRight.ProjectToWebMercator();
        }

        


        /// <summary>
        /// Calculates map bounds center
        /// </summary>
        private void calculateCenter()
        {
            if (this.lowerLeft != null && this.upperRight != null)
            {
                double northing = (this.upperRight.Northing + this.lowerLeft.Northing) / 2;
                double easting = (this.upperRight.Easting + this.lowerLeft.Easting) / 2;

                this.Center = new Vertex(northing, easting, 0);
            }
            else
            {
                this.Center = new Vertex(0, 0, 0);
            }
        }
    }
}
