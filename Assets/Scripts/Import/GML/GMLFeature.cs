using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Import.GML
{
    /// <summary>
    /// Class for storing GML features
    /// </summary>
    public class GMLFeature
    {
        /// <summary>
        /// Feature ID
        /// </summary>
        public int FID { get; private set; }
        /// <summary>
        /// List of Vertices
        /// </summary>
        public List<Vertex> Vertices { get; private set; }
        /// <summary>
        /// checks whether the geometry of that feature is empty
        /// </summary>
        public Boolean IsEmpty
        {
            get { return this.Vertices.Count < 1; }
        }

        /// <summary>
        /// Creates a new GMLFeature
        /// </summary>
        /// <param name="fid">feature ID</param>
        public GMLFeature(int fid)
        {
            this.FID = fid;
            this.Vertices = new List<Vertex>();
        }

        public Vector3 GetCentroid()
        {
            Vector3 total = Vector3.zero;

            // no vertices so return 0 vector
            if (this.IsEmpty) return total;

            foreach (Vertex vertex in this.Vertices)
            {
                total += vertex.ToVector3();
            }

            return total / this.Vertices.Count;
        }

        /// <summary>
        /// Sets feature's geometry
        /// </summary>
        /// <param name="verticesData">list of vertices</param>
        /// <param name="dimension">cooridnates dimension: 2 - X and Y; 3 - X, Y and Z</param>
        public void SetGeometry(string verticesData, int dimension)
        {
            string[] vertices = verticesData.Split(new char[] { ' ' });

            switch (dimension)
            {
                // X Y Z
                case 3:
                    {
                        for (int i = 0; i < vertices.Length - 3; i = i + 3)
                        {
                            this.Vertices.Add(new Vertex(String.Format("{0} {1} {2}", vertices[i], vertices[i + 1], vertices[i + 2])));
                        }

                        break;
                    }
                // X Y
                case 2:
                    {
                        for (int i = 0; i < vertices.Length - 2; i = i + 2)
                        {
                            this.Vertices.Add(new Vertex(String.Format("{0} {1}", vertices[i], vertices[i + 1])));
                        }

                        break;
                    }
                default: break;
            }
        }



        public void ProjectToGeographic()
        {

        }

        public void ProjectToWebMercator()
        {
            this.Vertices.ForEach(v => v = v.ProjectToWebMercator());
        }

        public void SetScale(float scale)
        {
            this.Vertices.ForEach(v => v = v.Scale(scale));
        }
    }
}
