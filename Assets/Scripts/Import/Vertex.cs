using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Transformations;

namespace Import
{
    /// <summary>
    /// Class for storing vertex coordinate values
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// The value have different meanings according to the context it is used.
        /// Z axis - in Unity
        /// Latitude - for Geographic coordinates
        /// X/Y (mathematical/geodetical) - for Projected coordinates
        /// </summary>
        public double Northing { get; set; }
        /// <summary>
        /// The value have different meanings according to the context it is used.
        /// X axis - in Unity
        /// Longitude - for Geographic coordinates
        /// Y/X (mathematical/geodetical) - for Projected coordinates
        /// </summary>
        public double Easting { get; set; }
        /// <summary>
        /// The value have different meanings according to the context it is used.
        /// Y Axis - in Unity
        /// Elevation (Height) - for Geographic coordinates
        /// Z - for Projected coordinates
        /// </summary>
        public double Elevation { get; set; }

        /// <summary>
        /// Creates a new Vertex
        /// </summary>
        /// <param name="coordinates">coordinates separated by single space ' '.
        /// Z X Y - in Unity
        /// Latitude Longitude Elevation - for Geographic coordinates
        /// Northing Easting Elevation - for Projected coordinates
        /// </param>
        public Vertex(string coordinates)
        {
            string[] values = coordinates.Split(new char[] { ' ' });

            double northing = 0.0, easting = 0.0, elevation = 0.0;

            Double.TryParse(values[0], out northing);
            Double.TryParse(values[1], out easting);
            Double.TryParse(values[2], out elevation);

            this.setCoordinates(northing, easting, elevation);
        }

        /// <summary>
        /// Creates a new Vertex
        /// </summary>
        /// <param name="northing">Northing component in meters or decimal degrees</param>
        /// <param name="easting">Easting component in meters or decimal degrees</param>
        /// <param name="elevation">Elevation in meters</param>
        public Vertex(double northing, double easting, double elevation)
        {
            this.setCoordinates(northing, easting, elevation);
        }

        /// <summary>
        /// Convert the Vertex to a new Vector3. Unity uses different coordinate system
        /// so the coordinates are as follows:
        /// Longitude (X) = X (Unity)
        /// Latitude (Y) = Z (Unity)
        /// Elevation = Y (Unity)
        /// </summary>
        /// <returns>new Vector3(this)</returns>
        public Vector3 ToVector3()
        {
            return new Vector3((float)this.Easting, (float)this.Elevation, (float)this.Northing);
        }

        /// <summary>
        /// Substracts the origin from this vector
        /// </summary>
        /// <param name="origin">Vector to substract</param>
        /// <returns>new Vector3 = this - origin</returns>
        public Vector3 ReduceToVector3(Vector3 origin)
        {
            Vector3 that = this.ToVector3();
            return that - origin;
        }

        /// <summary>
        /// Scales the vertex to the specified scale.
        /// </summary>
        /// <param name="scale">scale value, default is 1 for 1:1 scale</param>
        /// <returns>this in specified scale</returns>
        public Vertex Scale(float scale = 1)
        {
            this.Northing *= (1 / scale);
            this.Easting *= (1 / scale);
            return this;
        }



        public Vertex ProjectToGeographic()
        {
            this.Northing = WebMercatorProjection.yToLat(this.Northing);
            this.Easting = WebMercatorProjection.xToLon(this.Easting);
            return this;
        }

        public Vertex ProjectToWebMercator()
        {
            this.Northing = WebMercatorProjection.latToY(this.Northing);
            this.Easting = WebMercatorProjection.lonToX(this.Easting);

            return this;
        }




        /// <summary>
        /// Calculate and set vertex coordinates
        /// </summary>
        /// <param name="northing">geographic latitude in decimal degrees</param>
        /// <param name="easting">geographic latitude in decimal degrees</param>
        /// <param name="elevation">elevation in meters</param>
        private void setCoordinates(double northing, double easting, double elevation)
        {
            this.Northing = northing;
            this.Easting = easting;
            this.Elevation = elevation;
        }
    }
}
